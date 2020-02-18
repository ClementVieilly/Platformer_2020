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

	private void Awake()
	{
		backButton.OnMenuButtonClicked += ConfirmScreenBack_Clicked;
		skipButton.OnMenuButtonClicked += ConfirmScreenSkip_Clicked;
	}

	private void ConfirmScreenBack_Clicked(Button button)
	{
		OnBackClicked?.Invoke(this);
	}

	private void ConfirmScreenSkip_Clicked(Button button)
	{
		OnSkipClicked?.Invoke(this);
	}

	public override void UnsubscribeEvents()
	{
		OnBackClicked = null;
		OnSkipClicked = null;
	}
}
