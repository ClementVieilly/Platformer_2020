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
		private static WebClient _instance;

		private string _jsonWebToken = null;
		public string JsonWebToken { get => _jsonWebToken; }

		private bool isPreviousRequestOver = false;
		private bool isPreviousRequestSucces = false;

		private Coroutine tryToLogCoroutine = null;
		private Coroutine currentSubCoroutine = null;

		private ScoreObject[] _scores = null;
		public ScoreObject[] Scores { get => _scores; }

		[Serializable]
		public class WebCredentials
		{
			public string username = null;
			public string password = null;

			public WebCredentials(string username, string password)
			{
				this.username = username;
				this.password = password;
			}
		}

		[Serializable]
		public class ScoreObject
		{
			public string username = "default";
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

		private WebCredentials _credentials = null;
		public WebCredentials Credentials
		{
			set
			{
				if (_credentials == null)
					_credentials = value;
			}
		}

		private Button logButton = null;

		[SerializeField] private WebClientUnityEvent _onLogged;
		[SerializeField] private WebClientUnityEvent _onScoreGet;

		public event UnityAction<WebClient> OnLogged
		{
			add { _onLogged.AddListener(value); }
			remove { _onLogged.RemoveListener(value); }
		}

		public event UnityAction<WebClient> OnScoreGet
		{
			add { _onScoreGet.AddListener(value); }
			remove { _onScoreGet.RemoveListener(value); }
		}

		private void Awake()
		{
			if (_instance)
			{
				Destroy(gameObject);
				return;
			}
			_instance = this;

			DontDestroyOnLoad(gameObject);
		}

		private void Start()
		{
			OnLogged += StopMyCoroutines;
		}

		/// <summary>
		/// Stop all WebClient coroutines
		/// </summary>
		private void StopMyCoroutines(WebClient webClient)
		{
			StopCoroutine(tryToLogCoroutine);
			StopCoroutine(currentSubCoroutine);
		}

		public void AddOnLogButtonListener(Button button)
		{
			logButton = button;
			logButton.onClick.AddListener(OnLogButtonClicked);
		}

		private void RemoveOnLogButtonListener()
		{
			logButton.onClick.RemoveListener(OnLogButtonClicked);
		}

		private void OnLogButtonClicked()
		{
			if (_credentials.username.Length == 0 || _credentials.password.Length == 0)
			{
				Debug.LogWarning("WebClient::OnLog : username or password input field is empty.");
				return;
			}

			tryToLogCoroutine = StartCoroutine(TryToLogCoroutine());
		}

		public void OnButtonAllScores()
		{
			StartCoroutine(GetAllScoresForLevelCoroutine(1));
		}

		public void OnButtonOneScore()
		{
			StartCoroutine(GetPlayerScoreForLevelCoroutine(7, 2));
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

			WebCredentials credentials = new WebCredentials(_credentials.username, _credentials.password);
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
					_jsonWebToken = request.downloadHandler.text;
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

			WebCredentials credentials = new WebCredentials(_credentials.username, _credentials.password);
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
					_jsonWebToken = request.downloadHandler.text;
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
				request.SetRequestHeader("Authorization", "Bearer " + _jsonWebToken ?? "");

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
			isPreviousRequestSucces = false;
			isPreviousRequestOver = false;
			string url = "https://platformer-sequoia.herokuapp.com/scores/" + level;

			using (UnityWebRequest request = UnityWebRequest.Get(url))
			{
				request.SetRequestHeader("Authorization", "Bearer " + _jsonWebToken ?? "");

				yield return request.SendWebRequest();

				if (request.isNetworkError)
					Debug.Log("NetworkError: " + request.error);
				else if (request.isHttpError)
					Debug.Log("HttpError: " + request.error + ": " + request.downloadHandler.text);
				else
				{
					Debug.Log(request.downloadHandler.text);
					isPreviousRequestSucces = true;
					_scores = JsonHelper.GetJsonArray<ScoreObject>(request.downloadHandler.text);
					_onScoreGet?.Invoke(this);
				}

				isPreviousRequestOver = true;
			}
		}

		private IEnumerator GetPlayerScoreForLevelCoroutine(int userId, int level)
		{
			isPreviousRequestSucces = false;
			isPreviousRequestOver = false;
			string url = "https://platformer-sequoia.herokuapp.com/scores/" + userId + "/" + level;

			using (UnityWebRequest request = UnityWebRequest.Get(url))
			{
				request.SetRequestHeader("Authorization", "Bearer " + _jsonWebToken ?? "");

				yield return request.SendWebRequest();

				if (request.isNetworkError)
					Debug.Log("NetworkError: " + request.error);
				else if (request.isHttpError)
					Debug.Log("HttpError: " + request.error + ": " + request.downloadHandler.text);
				else
				{
					Debug.Log(request.downloadHandler.text);
					isPreviousRequestSucces = true;
					_scores = JsonHelper.GetJsonArray<ScoreObject>(request.downloadHandler.text);
					_onScoreGet?.Invoke(this);
				}

				isPreviousRequestOver = true;
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