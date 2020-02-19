///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 04/02/2020 16:14
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Managers;
using Com.IsartDigital.Platformer.UnityEvents;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Com.IsartDigital.Platformer.WebScripts
{
	public class WebClient : MonoBehaviour
	{
		public delegate void WebClientFeedbackEventHandler(string message);

		private static WebClient _instance;

		private string jsonWebToken = null;

		private bool _isPreviousRequestOver = true;
		public bool IsPreviousRequestOver { get => _isPreviousRequestOver; }
		private bool isPreviousRequestSucces = false;

		private ScoreObject[] _scores = null;
		public ScoreObject[] Scores { get => _scores; }

		[Serializable]
		public class WebCredentials
		{
			public string id = null;
			public string username = null;
			public string password = null;

			public WebCredentials(string username, string password)
			{
				this.username = username;
				this.password = password;
			}
		}

		private WebCredentials _credentials = null;
		public WebCredentials Credentials { set { _credentials = value; } }

		private bool _isLogged = false;
		public bool IsLogged { get => _isLogged; }
		public bool wantToLog = true;
		private bool _canTryToLog = true;
		public bool CanTryToLog { get => _canTryToLog; }

		public event WebClientFeedbackEventHandler OnFeedback;

		[SerializeField] private WebClientUnityEvent _onLogged = null;
		[SerializeField] private WebClientUnityEvent _onScoreGet = null;

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

		public void SuscribeToLevelManager(LevelManager levelManager)
		{
			levelManager.OnWin += LevelManager_OnWin;
		}

		private void LevelManager_OnWin(LevelManager levelManager)
		{
			if (!_isLogged) return;

			ScoreObject scoreObject = new ScoreObject(Mathf.RoundToInt(levelManager.CompletionTime), levelManager.Score, levelManager.Lives);
			StartCoroutine(RegisterPlayerScoreForLevelCoroutine(levelManager.LevelNumber, scoreObject));
		}

		public void GetScoresForLevel(int level)
		{
			StartCoroutine(GetAllScoresForLevelCoroutine(level));
		}

		public void GetPlayerScoreForLevel(int level)
		{
			StartCoroutine(GetPlayerScoreForLevelCoroutine(level));
		}

		public void TryToLog()
		{
			StartCoroutine(TryToLogMainCoroutine());
		}

		private IEnumerator TryToLogMainCoroutine()
		{
			_canTryToLog = false;

			OnFeedback?.Invoke(string.Empty);

			if (_credentials.username.Length == 0 || _credentials.password.Length == 0)
			{
				yield return new WaitForSeconds(0.5f);

				Debug.LogWarning("WebClient::TryToLogMainCoroutine: username or password input field is empty.");

				_canTryToLog = true;
				OnFeedback?.Invoke("Username or password input field is empty.");
				yield break;
			}

			StartCoroutine(TryToLogCoroutine());
		}

		private IEnumerator TryToLogCoroutine()
		{
			_canTryToLog = false;

			StartCoroutine(SigninCoroutine());

			while (!_isPreviousRequestOver)
				yield return null;

			if (isPreviousRequestSucces)
			{
				_isLogged = true;
				wantToLog = false;
				_onLogged?.Invoke(this);
				yield break;
			}

			StartCoroutine(SignupCoroutine());

			while (!_isPreviousRequestOver)
				yield return null;

			if (isPreviousRequestSucces)
			{
				_isLogged = true;
				wantToLog = false;
				_onLogged?.Invoke(this);
				yield break;
			}

			OnFeedback?.Invoke("User already exists. You should either enter the good password or choose a different username.");
			Debug.Log("WebClient::TryToLogCoroutine: User already exists. You should either enter the good password or choose a different username.");

			_canTryToLog = true;
		}

		private IEnumerator SignupCoroutine()
		{
			isPreviousRequestSucces = false;
			_isPreviousRequestOver = false;
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

					string[] response = request.downloadHandler.text.Split(new char[] { '/' }, 2);
					_credentials.id = response[0];
					jsonWebToken = response[1];

					Debug.Log("User registered !");
				}

				_isPreviousRequestOver = true;
			}
		}

		private IEnumerator SigninCoroutine()
		{
			isPreviousRequestSucces = false;
			_isPreviousRequestOver = false;
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

					string[] response = request.downloadHandler.text.Split(new char[] { '/' }, 2);
					_credentials.id = response[0];
					jsonWebToken = response[1];

					Debug.Log("Welcome back !");
				}

				_isPreviousRequestOver = true;
			}
		}

		private IEnumerator RegisterPlayerScoreForLevelCoroutine(int level, ScoreObject scoreObject)
		{
			isPreviousRequestSucces = false;
			_isPreviousRequestOver = false;
			string url = "https://platformer-sequoia.herokuapp.com/scores/" + _credentials.id + "/" + level;

			string json = JsonUtility.ToJson(scoreObject);

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

				_isPreviousRequestOver = true;
			}
		}

		private IEnumerator GetAllScoresForLevelCoroutine(int level)
		{
			isPreviousRequestSucces = false;
			_isPreviousRequestOver = false;
			string url = "https://platformer-sequoia.herokuapp.com/scores/" + level;

			_scores = null;

			using (UnityWebRequest request = UnityWebRequest.Get(url))
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
					_scores = JsonHelper.GetJsonArray<ScoreObject>(request.downloadHandler.text);
					_onScoreGet?.Invoke(this);
				}

				_isPreviousRequestOver = true;
			}
		}

		private IEnumerator GetPlayerScoreForLevelCoroutine(int level)
		{
			isPreviousRequestSucces = false;
			_isPreviousRequestOver = false;
			string url = "https://platformer-sequoia.herokuapp.com/scores/" + _credentials.id + "/" + level;

			_scores = null;

			using (UnityWebRequest request = UnityWebRequest.Get(url))
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

					_scores = JsonHelper.GetJsonArray<ScoreObject>(request.downloadHandler.text);
					_onScoreGet?.Invoke(this);
				}

				_isPreviousRequestOver = true;
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