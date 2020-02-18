///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 04/02/2020 11:16
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Screens;
using Com.IsartDigital.Platformer.Screens.Buttons;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Leaderboard : AScreen
{
    public delegate void LeaderboardEventHandler(Leaderboard leaderboard);
	public LeaderboardEventHandler OnMenuClicked;

	[SerializeField] private MenuButton homeButton = null;

	private void Awake()
	{
		homeButton.OnMenuButtonClicked += Leaderboard_OnMenuClicked;
	}

	private void Leaderboard_OnMenuClicked(Button sender)
	{
		OnMenuClicked?.Invoke(this);
	}

	public override void UnsubscribeEvents()
	{
		OnMenuClicked = null;
	}
}
