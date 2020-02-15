///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 04/02/2020 11:10
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Screens;
using Com.IsartDigital.Platformer.Screens.Buttons;
using UnityEngine;
using UnityEngine.UI;

public class LoseScreen : AScreen
{
    [SerializeField] private MenuButton retryBtn;
    [SerializeField] private MenuButton levelSelectorBtn;

    public delegate void LoseScreenEventHandler(LoseScreen loseScreen);//Delegates appelés au clic sur les différents boutons du WinScreen

    public LoseScreenEventHandler OnRetryClicked;
    public LoseScreenEventHandler OnLevelSelectorClicked;

    private void Awake()
    {
        retryBtn.OnMenuButtonClicked += LoseScreen_OnRetryClicked;
        levelSelectorBtn.OnMenuButtonClicked += LoseScreen_OnLevelSelectorClicked;
    }

    private void LoseScreen_OnRetryClicked(Button sender)
    {
        OnRetryClicked?.Invoke(this);
    }

    private void LoseScreen_OnLevelSelectorClicked(Button sender)
    {
        OnLevelSelectorClicked?.Invoke(this);
    }

    public override void UnsubscribeEvents()
    {
        OnRetryClicked = null;
        OnLevelSelectorClicked = null;
    }

    private void OnDestroy()
    {
        retryBtn.OnMenuButtonClicked = null;
        levelSelectorBtn.OnMenuButtonClicked = null;
    }
}
