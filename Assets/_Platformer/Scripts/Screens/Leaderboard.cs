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
	public LeaderboardEventHandler OnBackToTitleClicked;
	public LeaderboardEventHandler OnMenuClicked;

	private const string TITLE_SCENE_NAME_1 = "TitleScreen";
	private const string TITLE_SCENE_NAME_2 = "Main";

	[SerializeField] private MenuButton homeButton;

	private void Awake()
	{
		if (SceneManager.GetActiveScene().name == TITLE_SCENE_NAME_1 || SceneManager.GetActiveScene().name == TITLE_SCENE_NAME_2)
			homeButton.OnMenuButtonClicked += LeaderboardBackToTitle_Clicked;
		else
			homeButton.OnMenuButtonClicked += Leaderboard_OnMenuClicked;
	}

	private void LeaderboardBackToTitle_Clicked(Button sender)
	{
		OnBackToTitleClicked?.Invoke(this);
		homeButton.OnMenuButtonClicked -= LeaderboardBackToTitle_Clicked;
	}

	private void Leaderboard_OnMenuClicked(Button sender)
	{
		OnMenuClicked?.Invoke(this);
	}

	public override void UnsubscribeEvents()
	{
		OnBackToTitleClicked = null;
	}
}
