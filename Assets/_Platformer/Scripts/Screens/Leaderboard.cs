///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 18/02/2020 11:16
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer;
using Com.IsartDigital.Platformer.Screens;
using Com.IsartDigital.Platformer.Screens.Buttons;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : AScreen
{
    public delegate void LeaderboardEventHandler(Leaderboard leaderboard);
	public event LeaderboardEventHandler OnMenuClicked;
	public event LeaderboardEventHandler OnBackClicked;
	public event LeaderboardEventHandler OnSkipClicked;
	public event LeaderboardEventHandler OnNextClicked;
	public event LeaderboardEventHandler OnPreviousClicked;
	public event LeaderboardEventHandler OnStart;

	[SerializeField] private MenuButton homeButton = null;
	[SerializeField] private MenuButton backButton = null;
	[SerializeField] private MenuButton skipButton = null;
	[SerializeField] private MenuButton nextLevel = null;
	[SerializeField] private MenuButton previousLevel = null;

	[Space, SerializeField] private uint nbMaxScoreToDisplay = 6;
	private int _levelToDisplay = 1;
	public int LevelToDisplay { get => _levelToDisplay; set { _levelToDisplay = value; } }
	[SerializeField] private GameObject infosZone = null;
	[SerializeField] private Text level = null;
	[SerializeField] private GameObject scoreDisplayPrefab = null;

	private void Awake()
	{
		homeButton.OnMenuButtonClicked += Leaderboard_OnMenuClicked;
		backButton.OnMenuButtonClicked += Leaderboard_OnBackClicked;
		skipButton.OnMenuButtonClicked += Leaderboard_OnSkipClicked;
		if (nextLevel) nextLevel.OnMenuButtonClicked += Leaderboard_OnNextLevel;
		if (previousLevel) previousLevel.OnMenuButtonClicked += Leaderboard_OnPreviousLevel;
	}

	public void StartLeaderboard()
	{
		OnStart?.Invoke(this);
	}

	private void Leaderboard_OnMenuClicked(Button sender)
	{
		OnMenuClicked?.Invoke(this);
	}

	private void Leaderboard_OnBackClicked(Button sender)
	{
		OnBackClicked?.Invoke(this);
	}

	private void Leaderboard_OnSkipClicked(Button sender)
	{
		OnSkipClicked?.Invoke(this);
	}

	private void Leaderboard_OnNextLevel(Button sender)
	{
		OnNextClicked?.Invoke(this);
	}

	private void Leaderboard_OnPreviousLevel(Button sender)
	{
		OnPreviousClicked?.Invoke(this);
	}

	public void UpdateDisplay(ScoreObject[] scores, ScoreObject playerScore, bool isLogged, string username = "undefined")
	{
		bool playerDisplayed = false;
		ScoreDisplay display = null;

		ClearDisplay();

		for (int i = 0; i < scores.Length && i < (playerDisplayed ? nbMaxScoreToDisplay - 1 : nbMaxScoreToDisplay); i++)
		{
			display = AddScoreDisplay(scores[i]);

			if (playerScore != null && scores[i].username == playerScore.username)
			{
				playerDisplayed = true;
				display.SetTextColor(Color.blue);
			}
		}

		if (!playerDisplayed && isLogged)
		{
			if (playerScore == null)
				display = AddUndefinedScoreDisplay(username);
			else
				display = AddScoreDisplay(playerScore);
			
			display.SetTextColor(Color.blue);
		}
	}

	public void ClearDisplay()
	{
		if (infosZone == null) return;

		for (int i = infosZone.transform.childCount - 1; i >= 0; i--)
			Destroy(infosZone.transform.GetChild(i).gameObject);

		level.text = "Level " + _levelToDisplay.ToString();
	}

	private ScoreDisplay AddScoreDisplay(ScoreObject scoreObject)
	{
		ScoreDisplay display = Instantiate(scoreDisplayPrefab, infosZone.transform).GetComponent<ScoreDisplay>();
		display.Username = scoreObject.username;
		display.Time = scoreObject.completion_time.ToString();
		display.Score = scoreObject.nb_score.ToString();
		display.Lives = scoreObject.nb_lives.ToString();

		return display;
	}

	private ScoreDisplay AddUndefinedScoreDisplay(string username)
	{
		ScoreDisplay display = Instantiate(scoreDisplayPrefab, infosZone.transform).GetComponent<ScoreDisplay>();
		display.Username = username;
		display.Time = "?";
		display.Score = "?";
		display.Lives = "?";

		return display;
	}

	public override void UnsubscribeEvents()
	{
		OnStart = null;
		OnMenuClicked = null;
		OnBackClicked = null;
		OnNextClicked = null;
		OnPreviousClicked = null;
		OnSkipClicked = null;
	}
}
