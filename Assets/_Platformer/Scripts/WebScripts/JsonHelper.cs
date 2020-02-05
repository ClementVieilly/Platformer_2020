///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 05/02/2020 10:14
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.IsartDigital.Platformer.WebScripts {
	//Classe qui permet de gérer les json qui sont des array
	public class JsonHelper
	{
		public static T[] GetJsonArray<T>(string json)
		{
			string lNewJson = "{ \"array\": " + json + "}";
			Wrapper<T> lWrapper = JsonUtility.FromJson<Wrapper<T>>(lNewJson);
			return lWrapper.array;
		}

		[Serializable]
		protected class Wrapper<T>
		{
			public T[] array;
		}
	}

	/*
	//On crée une nouvelle class pour obtenir du json qui sera envoyer au serveur (il faut qu'elle soit sérializable)
	[Serializable]
	public class BaseToJson
	{
		[SerializeField] protected string username;
		[SerializeField] protected string password;
		[SerializeField] protected string nLevel;
		[SerializeField] protected string time;
		[SerializeField] protected string collectible;
		[SerializeField] protected string remainingLife;
		public void Credentials(string username, string password)
		{
			this.username = username;
			this.password = password;
		}
		public void MyScore(string username, string nLevel)
		{
			this.username = username;
			this.nLevel = nLevel;
		}
		public void SetScore(string username, string password, string nLevel, string completionTime, string collectible, string remainingLife)
		{
			this.username = username;
			this.password = password;
			this.nLevel = nLevel;
			time = completionTime;
			this.collectible = collectible;
			this.remainingLife = remainingLife;
		}
	}
	//On crée une class qui permet de lire le json et de le parser pour le lire (il faut qu'elle soit sérializable)
	[Serializable]
	public class BaseFromJson
	{
		[SerializeField] protected int user_id;
		[SerializeField] protected int level_id;
		[SerializeField] protected int completion_time;
		[SerializeField] protected int collectible_collected;
		[SerializeField] protected int remaining_life;
		[SerializeField] protected int score_id;
		[SerializeField] protected int level_nb;
		[SerializeField] protected string username;
		public int UserId => user_id;
		public int LevelId => level_id;
		public int CompletionTime => completion_time;
		public int CollectibleCollected => collectible_collected;
		public int RemainingLife => remaining_life;
		public int ScoreId => score_id;
		public int LevelNb => level_nb;
		public string Username => username;
	}*/
}