///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 05/02/2020 10:14
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.IsartDigital.Platformer.WebScripts
{
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
}