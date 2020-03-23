///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:36
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.LevelObjects;
using Com.IsartDigital.Platformer.WebScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Managers
{
	public class GameManager : MonoBehaviour
	{
		private static GameManager _instance;

		private UIManager uiManager = null;
		private WebClient webClient = null;

		private List<List<ScoreObject>> scores = new List<List<ScoreObject>>();
		private List<ScoreObject> playerScores = new List<ScoreObject>();

		private void Awake()
		{
			if (_instance)
			{
				Destroy(gameObject);
				return;
			}
			_instance = this;

			DontDestroyOnLoad(gameObject);
		}

		private void Start()
		{
			webClient = FindObjectOfType<WebClient>();

			if (UIManager.Instance)
			{
				uiManager = UIManager.Instance;
				uiManager.OnLevelLoaded += UIManager_OnLevelLoaded;
				uiManager.OnLeaderboardStart += UIManager_OnLeaderboardEvent;
				uiManager.OnLeaderBoardChangeLevel += UIManager_OnLeaderboardEvent;
				uiManager.SetWebClient(webClient);
			}
		}

		private void OnApplicationPause(bool pause)
		{
			SetSoundPlay(pause);
		}

		private void UIManager_OnLevelLoaded(LevelManager levelManager)
		{
			levelManager.OnWin += LevelManager_OnWin;
			levelManager.SetPlayer(FindObjectOfType<Player>());
            levelManager.InitPlayerPos(); 
		}

		private void LevelManager_OnWin(LevelManager levelManager)
		{
			if (!webClient.IsLogged) return;

			CheckScoresSizes(levelManager.LevelNumber);

			bool mustRegisterNewScore = false;

			ScoreObject newScore = new ScoreObject(Mathf.RoundToInt(levelManager.CompletionTime), levelManager.Score, levelManager.Lives);
			if (playerScores[levelManager.LevelNumber - 1] != null && playerScores[levelManager.LevelNumber - 1] < newScore)
			{
				mustRegisterNewScore = true;
				playerScores[levelManager.LevelNumber - 1].completion_time = newScore.completion_time;
				playerScores[levelManager.LevelNumber - 1].nb_score = newScore.nb_score;
				playerScores[levelManager.LevelNumber - 1].nb_lives = newScore.nb_lives;
			}

			StartCoroutine(VerifyAndRegisterPlayerScoreCoroutine(mustRegisterNewScore, levelManager.LevelNumber, newScore));
		}

		private IEnumerator VerifyAndRegisterPlayerScoreCoroutine(bool mustRegisterNewScore, int level, ScoreObject newScore)
		{
			if (!mustRegisterNewScore)
			{
				Debug.Log("GameManager::VerifyAndRegisterPlayerScoreCoroutine: Start getting playerScore");
				webClient.GetPlayerScoreForLevel(level);

				while (!webClient.IsPreviousRequestOver)
					yield return null;

				ScoreObject currentScore = ScoreObject.Worst;
				if (webClient.Scores != null)
					currentScore = webClient.Scores[0];

				if (currentScore < newScore)
					mustRegisterNewScore = true;
			}

			if (mustRegisterNewScore)
				webClient.RegisterPlayerScoreForLevel(level, newScore);
		}

		private void UIManager_OnLeaderboardEvent(Leaderboard leaderboard)
		{
			StartCoroutine(GetScoresForLevelCoroutine(leaderboard));

			leaderboard.ShowLoading();
		}

		private IEnumerator GetScoresForLevelCoroutine(Leaderboard leaderboard)
		{
			int level = leaderboard.LevelToDisplay;

			CheckScoresSizes(level);

			while (!webClient.IsPreviousRequestOver)
				yield return null;

			if (scores[level - 1] == null)
			{
				Debug.Log("GameManager::GetScoresForLevelCoroutine: Start getting scores");
				webClient.GetScoresForLevel(level);

				while (!webClient.IsPreviousRequestOver)
					yield return null;

				Debug.Log("GameManager::GetScoresForLevelCoroutine: Start sorting scores");
				if (webClient.Scores != null)
					scores[level - 1] = new List<ScoreObject>(webClient.Scores);
			}

			if (webClient.IsLogged && playerScores[level - 1] == null)
			{
				Debug.Log("GameManager::GetScoresForLevelCoroutine: Start getting playerScore");
				webClient.GetPlayerScoreForLevel(level);

				while (!webClient.IsPreviousRequestOver)
					yield return null;

				if (webClient.Scores != null)
					playerScores[level - 1] = webClient.Scores[0];
			}

			SortScores(level - 1);

			if (scores[level - 1] != null)
				leaderboard.UpdateDisplay(scores[level - 1].ToArray(), playerScores[level - 1], webClient.IsLogged, webClient.Credentials != null ? webClient.Credentials.username : "undefined");
			else
			{
				leaderboard.ClearDisplay();

				if (webClient.IsLogged)
					leaderboard.AddUndefinedScoreDisplay(webClient.Credentials.username);
			}

			leaderboard.HideLoading();
		}

		private void SortScores(int level)
		{
			if (scores[level] == null) return;

			if (playerScores[level] != null && scores[level].Contains(playerScores[level]))
				scores[level].Add(playerScores[level]);

			scores[level].Sort();
		}

		private void CheckScoresSizes(int level)
		{
			while (scores.Count < level)
				scores.Add(null);

			while (playerScores.Count < level)
				playerScores.Add(null);
		}

		private void SetSoundPlay(bool isPlay)
		{
			AudioListener.pause = isPlay;
		}

		private void OnDestroy()
		{
			if (this == _instance) _instance = null;
		}


	}
}