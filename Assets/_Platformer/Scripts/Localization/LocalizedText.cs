///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 12/03/2020 12:15
///-----------------------------------------------------------------

using System;
using Com.IsartDigital.Platformer.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Com.IsartDigital.Platformer.Localization {
	public class LocalizedText : MonoBehaviour {

        [SerializeField]  private string key;
        private Text componentText; 
		private void Start () {
            LocalizationManager.Instance.OnLoadFinished += LocalizationManager_OnLoadFinished; 
            componentText = GetComponent<Text>();
        }

        private void LocalizationManager_OnLoadFinished()
        {
            componentText.text = LocalizationManager.Instance.GetLocalizedValue(key);
        }

    }
}