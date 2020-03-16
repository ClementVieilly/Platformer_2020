///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 16/03/2020 15:49
///-----------------------------------------------------------------

using System;
using Com.IsartDigital.Platformer.Managers;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Screens {
	public class PreLoad : AScreen {
      
        public Action OnLauchTitleCard; 

        private void Awake()
        {
            LocalizationManager.OnLoadFinished += LocalizationManager_OnLoadFinished;
        }

        private void LocalizationManager_OnLoadFinished()
        {
            Debug.Log("Le Load est finit");
            Debug.Log(OnLauchTitleCard); 
            OnLauchTitleCard?.Invoke(); 
        }

        public void LauchLoadText()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
         
            StartCoroutine(LocalizationManager.Instance.LoadLocalizedTextOnAndroid());
#else
            Debug.Log(LocalizationManager.Instance); 
            StartCoroutine(LocalizationManager.Instance.LoadLocalizedText());
#endif
        }

        public override void UnsubscribeEvents()
        {
            LocalizationManager.OnLoadFinished -= LocalizationManager_OnLoadFinished;
        }
    }
}