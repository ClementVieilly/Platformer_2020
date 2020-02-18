///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 18/02/2020 11:16
///-----------------------------------------------------------------

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

	[SerializeField] private MenuButton homeButton = null;
	[SerializeField] private MenuButton backButton = null;
	[SerializeField] private MenuButton skipButton = null;

	[Space, SerializeField] private uint nbMaxScoreToDisplay = 6;
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

	private void AddScoreDisplay()
	{
	}

	public override void UnsubscribeEvents()
	{
		OnMenuClicked = null;
	}
}
