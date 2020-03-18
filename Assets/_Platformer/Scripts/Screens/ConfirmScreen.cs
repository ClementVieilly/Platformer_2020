///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 18/02/2020 11:14
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Screens;
using Com.IsartDigital.Platformer.Screens.Buttons;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmScreen : AScreen
{
	public delegate void ConfirmScreenEventHandler(ConfirmScreen confirmScreen);
	public event ConfirmScreenEventHandler OnBackClicked;
	public event ConfirmScreenEventHandler OnSkipClicked;

	[SerializeField] private MenuButton backButton = null;
	[SerializeField] private MenuButton skipButton = null;
    private bool isLogin = false; 

	private void Awake()
	{
        animator = GetComponent<Animator>();
        animator.SetTrigger(enter); 
		backButton.OnMenuButtonClicked += ConfirmScreenBack_Clicked;
		skipButton.OnMenuButtonClicked += ConfirmScreenSkip_Clicked;
	}

	private void ConfirmScreenBack_Clicked(Button button)
	{
        animator.SetTrigger(exit); 
        isLogin = true; 
	}

	private void ConfirmScreenSkip_Clicked(Button button)
	{
        animator.SetTrigger("ExitBis");
        isLogin = false; 
	}

    public void OnAnimEnd()
    {
        Debug.Log("oui"); 
        if(isLogin) OnBackClicked?.Invoke(this);
        else OnSkipClicked?.Invoke(this); 
    }
    public override void UnsubscribeEvents()
	{
        backButton.OnMenuButtonClicked -= ConfirmScreenBack_Clicked;
        skipButton.OnMenuButtonClicked -= ConfirmScreenSkip_Clicked;
        OnBackClicked = null;
		OnSkipClicked = null;
	}
}
