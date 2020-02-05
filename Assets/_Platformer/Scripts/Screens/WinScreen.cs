using Com.IsartDigital.Platformer.Screens;
using Com.IsartDigital.Platformer.Screens.Buttons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : AScreen
{
    [SerializeField] private MenuButton menuBtn;
    [SerializeField] private MenuButton levelSelectorBtn;

    public delegate void WinScreenEventHandler(WinScreen winScreen);//Delegates appelés au clic sur les différents boutons du WinScreen

    public WinScreenEventHandler OnMenuClicked;
    public WinScreenEventHandler OnLevelSelectorClicked;

    private void Awake()
    {
        menuBtn.OnMenuButtonClicked += WinScreen_OnMenuClicked;
        levelSelectorBtn.OnMenuButtonClicked += WinScreen_OnLevelSelectorClicked;
    }

    private void WinScreen_OnMenuClicked(Button sender)
    {
        OnMenuClicked?.Invoke(this);
    }

    private void WinScreen_OnLevelSelectorClicked(Button sender)
    {
        OnLevelSelectorClicked?.Invoke(this);
    }

    public void UnsubscribeEvents()
    {
        OnMenuClicked = null;
        OnLevelSelectorClicked = null;
    }

    private void OnDestroy()
    {
        menuBtn.OnMenuButtonClicked = null;
        levelSelectorBtn.OnMenuButtonClicked = null;
    }
}
