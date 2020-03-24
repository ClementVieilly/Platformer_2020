///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 16/03/2020 15:49
///-----------------------------------------------------------------

using System;
using Com.IsartDigital.Platformer.Managers;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Screens
{
	public class PreLoad : AScreen
	{
		public Action OnLauchTitleCard;

		private void Awake()
		{
#if !UNITY_WEBGL
			LocalizationManager.Instance.OnLoadFinished += LocalizationManager_OnLoadFinished;
#endif
		}

		private void LocalizationManager_OnLoadFinished()
		{
			OnLauchTitleCard?.Invoke();
			LocalizationManager.Instance.isPreload = false;
		}

		public void LauchLoadText()
		{
			LocalizationManager.Instance.isPreload = true;
#if UNITY_ANDROID && !UNITY_EDITOR
         
            StartCoroutine(LocalizationManager.Instance.LoadLocalizedTextOnAndroid());
#elif UNITY_WEBGL
			OnLauchTitleCard?.Invoke();
#else
			StartCoroutine(LocalizationManager.Instance.LoadLocalizedText());
#endif
		}

		public override void UnsubscribeEvents()
		{
			LocalizationManager.Instance.OnLoadFinished -= LocalizationManager_OnLoadFinished;
		}
	}
}