///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:36
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.Managers {
	public class GameManager : MonoBehaviour {

		private void Awake()
		{
			DontDestroyOnLoad(this.gameObject);
		}
	}
}