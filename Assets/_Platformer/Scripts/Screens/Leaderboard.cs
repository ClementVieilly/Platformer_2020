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
	public event LeaderboardEventHandler OnStart;

	[SerializeField] private MenuButton homeButton = null;
	[SerializeField] private MenuButton backButton = null;
	[SerializeField] private MenuButton skipButton = null;

	[Space, SerializeField] private uint nbMaxScoreToDisplay = 6;
	private int _levelToDisplay = 1;
	public int LevelToDisplay { get => _levelToDisplay; set { _levelToDisplay = value; } }
	[SerializeField] private GameObject infosZone = null;
	[SerializeField] private Text level = null;
	[SerializeField] private GameObject scoreDisplayPrefab = null;

	private uint currentDisplays = 0;

	private void Awake()
	{
		homeButton.OnMenuButtonClicked += Leaderboard_OnMenuClicked;
		backButton.OnMenuButtonClicked += Leaderboard_OnBackClicked;
		skipButton.OnMenuButtonClicked += Leaderboard_OnSkipClicked;
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

	public void UpdateDisplay(ScoreObject[] scores, ScoreObject playerScore, bool isLogged)
	{
		ScoreDisplay display;

		for (int i = 0; i < scores.Length && i < (isLogged ? nbMaxScoreToDisplay - 1 : nbMaxScoreToDisplay); i++)
		{
			display = AddScoreDisplay(scores[i]);

			if (playerScore != null && scores[i].username == playerScore.username) display.SetTextColor(Color.blue);
		}
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

	public override void UnsubscribeEvents()
	{
		OnStart = null;
		OnMenuClicked = null;
	}
}
