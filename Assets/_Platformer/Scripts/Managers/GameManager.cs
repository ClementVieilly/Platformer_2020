///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:36
///-----------------------------------------------------------------

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
		private LevelManager levelManager = null;

		private List<ScoreObject[]> scores = new List<ScoreObject[]>();
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
				uiManager.OnLeaderboardStart += UIManager_OnLeaderboardStart;
				uiManager.SetWebClient(webClient);
			}
		}

		private void UIManager_OnLevelLoaded(LevelManager levelManager)
		{
			this.levelManager = levelManager;
			levelManager.OnWin += LevelManager_OnWin;
		}

		private void LevelManager_OnWin(LevelManager levelManager)
		{
			CheckScoresSizes(levelManager.LevelNumber);

			if (playerScores[levelManager.LevelNumber - 1] != null)
			{
				playerScores[levelManager.LevelNumber - 1].completion_time = (int)levelManager.CompletionTime;
				playerScores[levelManager.LevelNumber - 1].completion_time = levelManager.Score;
				playerScores[levelManager.LevelNumber - 1].completion_time = levelManager.Lives;
			}
		}

		private void UIManager_OnLeaderboardStart(Leaderboard leaderboard)
		{
			StartCoroutine(GetScoresForLevelCoroutine(leaderboard));
			//Display le chargement
			//leaderboard.Wait();
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
				{
					scores[level - 1] = (ScoreObject[])webClient.Scores.Clone();
					SortScores(level - 1);
				}
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

			if (scores[level - 1] != null)
				leaderboard.UpdateDisplay(scores[level - 1], playerScores[level - 1], webClient.IsLogged, webClient.Credentials.username);
		}

		private void SortScores(int level)
		{
			if (scores[level] == null) return;

			IComparer nullComparer = null;
			Array.Sort(scores[level], nullComparer);
		}

		private void CheckScoresSizes(int level)
		{
			while (scores.Count < level)
				scores.Add(null);

			while (playerScores.Count < level)
				playerScores.Add(null);
		}

		private void OnDestroy()
		{
			if (this == _instance) _instance = null;
		}
	}
}