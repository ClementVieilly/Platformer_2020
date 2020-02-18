///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 04/02/2020 11:08
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Screens;
using Com.IsartDigital.Platformer.Screens.Buttons;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : AScreen
{
    [SerializeField] private MenuButton menuBtn;
    [SerializeField] private MenuButton levelSelectorBtn;
    [SerializeField] private MenuButton leaderboardBtn;

    public delegate void WinScreenEventHandler(WinScreen winScreen);//Delegates appel�s au clic sur les diff�rents boutons du WinScreen

    public WinScreenEventHandler OnMenuClicked;
    public WinScreenEventHandler OnLevelSelectorClicked;
    public WinScreenEventHandler OnLeaderboardClicked;

	private void Awake()
    {
        menuBtn.OnMenuButtonClicked += WinScreen_OnMenuClicked;
        levelSelectorBtn.OnMenuButtonClicked += WinScreen_OnLevelSelectorClicked;
		leaderboardBtn.OnMenuButtonClicked += WinScreen_OnLeaderboardClicked;
    }

	private void WinScreen_OnMenuClicked(Button sender)
    {
        OnMenuClicked?.Invoke(this);
    }

    private void WinScreen_OnLevelSelectorClicked(Button sender)
    {
        OnLevelSelectorClicked?.Invoke(this);
	}

	private void WinScreen_OnLeaderboardClicked(Button button)
	{
		OnLeaderboardClicked?.Invoke(this);
	}

	public override void UnsubscribeEvents()
    {
        OnMenuClicked = null;
        OnLevelSelectorClicked = null;
    }

    private void OnDestroy()
    {
        menuBtn.OnMenuButtonClicked = null;
        levelSelectorBtn.OnMenuButtonClicked = null;
		leaderboardBtn.OnMenuButtonClicked = null;
    }
}
