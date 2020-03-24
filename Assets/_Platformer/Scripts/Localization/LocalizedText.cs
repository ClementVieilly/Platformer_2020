///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 12/03/2020 12:15
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Com.IsartDigital.Platformer.Localization
{
	public class LocalizedText : MonoBehaviour
	{
		[SerializeField] private string key = null;
		private Text componentText;

		private void Start()
		{
#if UNITY_WEBGL
			return;
#else
			LocalizationManager.Instance.OnChangeLanguage += LocalizationManager_OnChangeLanguage;
			componentText = GetComponent<Text>();
			componentText.text = LocalizationManager.Instance.GetLocalizedValue(key);
#endif
		}

		private void LocalizationManager_OnChangeLanguage()
		{
			if (componentText != null) componentText.text = LocalizationManager.Instance.GetLocalizedValue(key);
		}
	}
}