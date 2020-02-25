///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 18/02/2020 11:12
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Screens;
using Com.IsartDigital.Platformer.Screens.Buttons;
using UnityEngine;
using UnityEngine.UI;

public class LoginScreen : AScreen
{
	public delegate void LoginScreenEventHandler(LoginScreen loginScreen);
	public event LoginScreenEventHandler OnConnectClicked;
	public event LoginScreenEventHandler OnSkipClicked;

	[SerializeField] private InputField usernameField = null;
	[SerializeField] private InputField passwordField = null;
	public string Username { get => usernameField.text; }
	public string Password { get => passwordField.text; }

	[SerializeField] private Text feedbackText = null;

	[SerializeField] private MenuButton _connectButton = null;
	public MenuButton ConnectButton { get => _connectButton; }
	[SerializeField] private MenuButton skipButton = null;

	private void Awake()
	{
		_connectButton.OnMenuButtonClicked += LoginScreenConnect_Clicked;
		skipButton.OnMenuButtonClicked += LoginScreenSkip_Clicked;

		passwordField.contentType = InputField.ContentType.Password;
	}

	private void LoginScreenConnect_Clicked(Button button)
	{
		_connectButton.Button.interactable = false;
		skipButton.Button.interactable = false;
		usernameField.interactable = false;
		passwordField.interactable = false;

		OnConnectClicked?.Invoke(this);
	}

	private void LoginScreenSkip_Clicked(Button button)
	{
		OnSkipClicked?.Invoke(this);
	}

	public void SendFeedback(string message)
	{
		if (message.Length != 0)
		{
			_connectButton.Button.interactable = true;
			skipButton.Button.interactable = true;
			usernameField.interactable = true;
			passwordField.interactable = true;
		}

		feedbackText.text = message;
	}

	public override void UnsubscribeEvents()
	{
		OnConnectClicked = null;
		OnSkipClicked = null;
	}
}
