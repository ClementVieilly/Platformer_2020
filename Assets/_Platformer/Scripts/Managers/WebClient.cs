///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 04/02/2020 16:14
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.UnityEvents;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Com.IsartDigital.Platformer.Managers
{
	public class WebClient : MonoBehaviour
	{
		[SerializeField] private Text usernameTextField = null;
		[SerializeField] private Text passwordTextField = null;

		[SerializeField] private Button logButton = null;

		private string jsonWebToken = null;

		private bool isPreviousRequestOver = false;
		private bool isPreviousRequestSucces = false;
		private bool mustTestNext = false;

		private Coroutine tryToLogCoroutine = null;
		private Coroutine currentSubCoroutine = null;

		[Serializable]
		private class Credentials
		{
			public string username = null;
			public string password = null;

			public Credentials(string username, string password)
			{
				this.username = username;
				this.password = password;
			}
		}

		[SerializeField] private WebClientUnityEvent _onLogged;

		public event UnityAction<WebClient> OnLogged
		{
			add { _onLogged.AddListener(value); }
			remove { _onLogged.RemoveListener(value); }
		}

		private void Start()
		{
			OnLogged += StopMyCoroutines;
			AddOnLogListener();
		}

		/// <summary>
		/// Stop all WebClient coroutines
		/// </summary>
		private void StopMyCoroutines(WebClient webClient)
		{
			StopCoroutine(tryToLogCoroutine);
			StopCoroutine(currentSubCoroutine);
		}

		private void AddOnLogListener()
		{
			logButton.onClick.AddListener(OnLog);
		}

		private void RemoveOnLogListener()
		{
			logButton.onClick.RemoveListener(OnLog);
		}

		private void OnLog()
		{
			if (usernameTextField.text.Length == 0 || passwordTextField.text.Length == 0)
			{
				Debug.LogWarning("WebClient::OnLog : username or password input field is empty.");
				return;
			}

			tryToLogCoroutine = StartCoroutine(TryToLog());
		}

		private IEnumerator TryToLog()
		{
			RemoveOnLogListener();

			currentSubCoroutine = StartCoroutine(Signin());

			while (!isPreviousRequestOver)
				yield return null;

			if (isPreviousRequestSucces)
			{
				_onLogged?.Invoke(this);
				yield break;
			}

			currentSubCoroutine = StartCoroutine(Signup());

			while (!isPreviousRequestOver)
				yield return null;

			if (isPreviousRequestSucces)
			{
				_onLogged?.Invoke(this);
				yield break;
			}

			Debug.Log("User already exists. You should either enter the good password or choose a different username.");
			AddOnLogListener();
			currentSubCoroutine = null;
			tryToLogCoroutine = null;
		}

		private IEnumerator Signup()
		{
			isPreviousRequestSucces = false;
			isPreviousRequestOver = false;
			string url = "https://platformer-sequoia.herokuapp.com/users/signup";

			Credentials credentials = new Credentials(usernameTextField.text, passwordTextField.text);
			string json = JsonUtility.ToJson(credentials);

			using (UnityWebRequest request = PostJson(url, json))
			{
				yield return request.SendWebRequest();

				if (request.isNetworkError)
					Debug.Log("NetworkError: " + request.error);
				else if (request.isHttpError)
					Debug.Log("HttpError: " + request.error + ": " + request.downloadHandler.text);
				else
				{
					isPreviousRequestSucces = true;
					jsonWebToken = request.downloadHandler.text;
					Debug.Log("User registered !");
				}
				
				isPreviousRequestOver = true;
			}
		}

		private IEnumerator Signin()
		{
			mustTestNext = true;
			isPreviousRequestSucces = false;
			isPreviousRequestOver = false;
			string url = "https://platformer-sequoia.herokuapp.com/users/signin";

			Credentials credentials = new Credentials(usernameTextField.text, passwordTextField.text);
			string json = JsonUtility.ToJson(credentials);

			using (UnityWebRequest request = PostJson(url, json))
			{
				yield return request.SendWebRequest();

				if (request.isNetworkError)
					Debug.Log("NetworkError: " + request.error);
				else if (request.isHttpError)
					Debug.Log("HttpError: " + request.error + ": " + request.downloadHandler.text);
				else
				{
					isPreviousRequestSucces = true;
					jsonWebToken = request.downloadHandler.text;
					Debug.Log("Welcome back !");
				}

				isPreviousRequestOver = true;
			}
		}

		/*private IEnumerator Me()
		{
			string url = "http://localhost:8000/users/me";

			using (UnityWebRequest request = UnityWebRequest.Get(url))
			{
				request.SetRequestHeader("Authorization", "Bearer " + jsonWebToken ?? "");

				yield return request.SendWebRequest();

				if (request.isNetworkError)
					Debug.Log("NetworkError: " + request.error);
				else if (request.isHttpError)
					Debug.Log("HttpError: " + request.error + ": " + request.downloadHandler.text);
				else
					Debug.Log(request.downloadHandler.text);
			}
		}*/

		private UnityWebRequest PostJson(string url, string json)
		{
			UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);

			request.downloadHandler = new DownloadHandlerBuffer();
			request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
			request.uploadHandler.contentType = "application/json";

			return request;
		}
	}
}