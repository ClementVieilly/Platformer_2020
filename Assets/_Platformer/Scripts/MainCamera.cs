///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 10/02/2020 14:03
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer {
	public class MainCamera : MonoBehaviour
	{
		private static MainCamera _instance;

		private void Awake()
	    {
			if (_instance != null && _instance != this)
			{
				Destroy(gameObject);
				return;
			}
			else _instance = this;

			DontDestroyOnLoad(gameObject);
	    }
	}
}