///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:36
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.Managers {
	public class GameManager : MonoBehaviour {

		private static GameManager _instance;
		//public static GameManager Instance => _instance;

		private void Awake()
		{
			if (_instance)
			{
				Destroy(gameObject);
				return;
			}
			_instance = this;

			DontDestroyOnLoad(this.gameObject);
		}

		private void OnDestroy()
		{
			if (this == _instance) _instance = null;
		}
	}
}