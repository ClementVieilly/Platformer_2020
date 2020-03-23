///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 23/03/2020 13:59
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Common {
	public class DisableGameObjectOnPlatform : MonoBehaviour
	{
		[SerializeField] private bool disableOnEditor = false;
		[SerializeField] private bool disableOnPC = false;
		[SerializeField] private bool disableOnWebGL = false;
		[SerializeField] private bool disableOnAndroid = false;

		private void Start()
		{
#if UNITY_EDITOR
			if (disableOnEditor) gameObject.SetActive(false);
#elif UNITY_STANDALONE_WIN
			if (disableOnPC) gameObject.SetActive(false);
#elif UNITY_WEBGL
			if (disableOnWebGL) gameObject.SetActive(false);
#elif UNITY_ANDROID
			if (disableOnAndroid) gameObject.SetActive(false);
#endif
		}
	}
}