///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 10/02/2020 14:03
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer {
	public class MainCamera : MonoBehaviour
	{
	    private void Awake()
	    {
			DontDestroyOnLoad(gameObject);
	    }
	}
}