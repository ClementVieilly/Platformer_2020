///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 04/02/2020 11:08
///-----------------------------------------------------------------

using System;
using Com.IsartDigital.Platformer.Managers;
using Com.IsartDigital.Platformer.Screens;
using Com.IsartDigital.Platformer.Screens.Buttons;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : AScreen
{
    [SerializeField] private MenuButton menuBtn = null;
    [SerializeField] private MenuButton levelSelectorBtn = null;
    [SerializeField] private MenuButton leaderboardBtn = null;

    public delegate void WinScreenEventHandler(WinScreen winScreen);//Delegates appelés au clic sur les différents boutons du WinScreen

    public event WinScreenEventHandler OnMenuClicked;
    public event WinScreenEventHandler OnLevelSelectorClicked;
    public event WinScreenEventHandler OnLeaderboardClicked;
    private int currentLevel; 

    private string enterLastScreenParam = "EnterLastScreen"; 

	private void Awake()
    {
        animator = GetComponent<Animator>();
        //menuBtn.OnMenuButtonClicked += WinScreen_OnMenuClicked;
       
		leaderboardBtn.OnMenuButtonClicked += WinScreen_OnLeaderboardClicked;
    }

    public void DisplayWinScreen(int level)
    {

        currentLevel = level; 
        if(level == 1)
        {
            animator.SetTrigger(enter);
            levelSelectorBtn.OnMenuButtonClicked += WinScreen_OnLevelSelectorClicked;
            menuBtn.OnMenuButtonClicked += WinScreen_OnMenuClicked;
        }
        else
        {
            animator.SetTrigger(enterLastScreenParam);
            levelSelectorBtn.OnMenuButtonClicked += WinScreen_EndScreenClicked;
            menuBtn.OnMenuButtonClicked += WinScreen_EndScreenClicked;
        }

    }
    private void AnimCallBackLastScreen()
    {
        OnMenuClicked?.Invoke(this);
    }

    private void WinScreen_EndScreenClicked(Button button)
    {
        animator.SetTrigger("ExitLastScreen"); 

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
