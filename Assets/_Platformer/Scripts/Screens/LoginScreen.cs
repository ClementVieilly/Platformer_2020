///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 04/02/2020 11:12
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Screens;
using Com.IsartDigital.Platformer.Screens.Buttons;
using UnityEngine;
using UnityEngine.UI;

public class LoginScreen : AScreen
{
	public delegate void LoginScreenEventHandler(LoginScreen loginScreen);
	public LoginScreenEventHandler OnConnectClicked;
	public LoginScreenEventHandler OnSkipClicked;

	[SerializeField] private MenuButton connectButton = null;
	[SerializeField] private MenuButton skipButton = null;

	private void Awake()
	{
		connectButton.OnMenuButtonClicked += LoginScreenSkip_Clicked;
		skipButton.OnMenuButtonClicked += LoginScreenSkip_Clicked;
	}

	private void LoginScreenConnect_Clicked(Button button)
	{
		OnConnectClicked?.Invoke(this);
	}

	private void LoginScreenSkip_Clicked(Button button)
	{
		OnSkipClicked?.Invoke(this);
	}

	public override void UnsubscribeEvents()
	{
		OnConnectClicked = null;
		OnSkipClicked = null;
	}
}
