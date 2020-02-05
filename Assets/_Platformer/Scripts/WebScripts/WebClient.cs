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

namespace Com.IsartDigital.Platformer.WebScripts
{
	public class WebClient : MonoBehaviour
	{
		[SerializeField] private Text usernameTextField = null;
		[SerializeField] private Text passwordTextField = null;

		[SerializeField] private Button logButton = null;
		[SerializeField] private Button testButton = null;

		private string jsonWebToken = null;

		private bool isPreviousRequestOver = false;
		private bool isPreviousRequestSucces = false;

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

		[Serializable]
		private class ScoreObject
		{
			public int completion_time = 0;
			public int nb_score = 0;
			public int nb_lives = 0;

			public ScoreObject(int completionTime, int nbScore, int nbLives)
			{
				completion_time = completionTime;
				nb_score = nbScore;
				nb_lives = nbLives;
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
			OnLogged += AddOnLogButtonListener;
			AddOnLogButtonListener(null);

			testButton.onClick.AddListener(OnTestButton);
		}

		private void OnTestButton()
		{
			//StartCoroutine(GetAllScoresForLevelCoroutine(1));
			//StartCoroutine(GetPlayerScoreForLevelCoroutine(1));
		}

		/// <summary>
		/// Stop all WebClient coroutines
		/// </summary>
		private void StopMyCoroutines(WebClient webClient)
		{
			StopCoroutine(tryToLogCoroutine);
			StopCoroutine(currentSubCoroutine);
		}

		private void AddOnLogButtonListener(WebClient webClient)
		{
			logButton.onClick.AddListener(OnLogButtonClicked);
		}

		private void RemoveOnLogButtonListener()
		{
			logButton.onClick.RemoveListener(OnLogButtonClicked);
		}

		private void OnLogButtonClicked()
		{
			if (usernameTextField.text.Length == 0 || passwordTextField.text.Length == 0)
			{
				Debug.LogWarning("WebClient::OnLog : username or password input field is empty.");
				return;
			}

			tryToLogCoroutine = StartCoroutine(TryToLogCoroutine());
		}

		private IEnumerator TryToLogCoroutine()
		{
			RemoveOnLogButtonListener();

			currentSubCoroutine = StartCoroutine(SigninCoroutine());

			while (!isPreviousRequestOver)
				yield return null;

			if (isPreviousRequestSucces)
			{
				_onLogged?.Invoke(this);
				yield break;
			}

			currentSubCoroutine = StartCoroutine(SignupCoroutine());

			while (!isPreviousRequestOver)
				yield return null;

			if (isPreviousRequestSucces)
			{
				_onLogged?.Invoke(this);
				yield break;
			}

			Debug.Log("User already exists. You should either enter the good password or choose a different username.");
			AddOnLogButtonListener(null);
			currentSubCoroutine = null;
			tryToLogCoroutine = null;
		}

		private IEnumerator SignupCoroutine()
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

		private IEnumerator SigninCoroutine()
		{
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

		private IEnumerator RegisterPlayerScoreForLevelCoroutine(int userId, int level)
		{
			isPreviousRequestSucces = false;
			isPreviousRequestOver = false;
			string url = "https://platformer-sequoia.herokuapp.com/scores/" + userId + "/" + level;

			ScoreObject score = new ScoreObject(100, 1, 1);
			string json = JsonUtility.ToJson(score);

			using (UnityWebRequest request = PostJson(url, json))
			{
				request.SetRequestHeader("Authorization", "Bearer " + jsonWebToken ?? "");

				yield return request.SendWebRequest();

				if (request.isNetworkError)
					Debug.Log("NetworkError: " + request.error);
				else if (request.isHttpError)
					Debug.Log("HttpError: " + request.error + ": " + request.downloadHandler.text);
				else
				{
					isPreviousRequestSucces = true;
					Debug.Log(request.downloadHandler.text);
				}

				isPreviousRequestOver = true;
			}
		}

		private IEnumerator GetAllScoresForLevelCoroutine(int level)
		{
			string url = "https://platformer-sequoia.herokuapp.com/scores/" + level;

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
		}

		private IEnumerator GetPlayerScoreForLevelCoroutine(int userId, int level)
		{
			string url = "https://platformer-sequoia.herokuapp.com/scores/" + userId + "/" + level;

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
		}

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