///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 04/02/2020 11:16
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Screens;
using Com.IsartDigital.Platformer.Screens.Buttons;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : AScreen
{
    public delegate void LeaderboardEventHandler(Leaderboard leaderboard);
	public LeaderboardEventHandler OnMenuClicked;
	public LeaderboardEventHandler OnBackClicked;
	public LeaderboardEventHandler OnSkipClicked;

	[SerializeField] private MenuButton homeButton = null;
	[SerializeField] private MenuButton backButton = null;
	[SerializeField] private MenuButton skipButton = null;

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

	public override void UnsubscribeEvents()
	{
		OnMenuClicked = null;
	}
}
