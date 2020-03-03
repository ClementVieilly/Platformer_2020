///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:35
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Controllers;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Managers
{
	public delegate void InputManagerEventHandler(InputManager inputManager);

	public class InputManager : MonoBehaviour
	{
		private static InputManager instance;
		public static InputManager Instance { get { return instance; } }

		private AController _controller;
		public AController Controller { get => _controller; }

		// Delegate gérant l'événement de pause
		public InputManagerEventHandler OnKeyPausePressed;

		private void Awake()
		{
			if (instance)
			{
				Destroy(gameObject);
				return;
			}

			instance = this;

			DontDestroyOnLoad(this.gameObject);

			// Définit le controller en fonction du device
#if UNITY_ANDROID && !UNITY_EDITOR
			_controller = new TouchController();
			_controller.Init();
#else
			_controller = new KeyboardController();
#endif
		}

		private void Update()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			UpdateAndroidInputs();
#else
			UpdateKeyboardInputs();
#endif
		}

		/// <summary>
		/// Update keyboard inputs for PC Standalone and WebGL 
		/// </summary>
		private void UpdateKeyboardInputs()
		{
			if (Input.GetKeyDown(KeyCode.Escape)) OnKeyPausePressed?.Invoke(instance); //Envoi de l'event de pause 
		}

		/// <summary>
		/// Update touch inputs for Android
		/// </summary>
		private void UpdateAndroidInputs()
		{
			_controller.Update();
		}

		private void OnDestroy()
		{
			if (this == instance) instance = null;
		}
	}
}