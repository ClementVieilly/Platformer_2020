///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 12/03/2020 12:15
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Managers
{
    public class LocalizationManager : MonoBehaviour
    {

        private static LocalizationManager _instance;
        public static LocalizationManager Instance => _instance;

        private string filePathName = "_Platformer/Resources/"; 
        private string extensionName = ".json"; 
        private string defaultLocalizedText = "localizedText_en";
        private string frenchLocalizedText =  "localizedText_fr";
        private string fileName; 

        private bool isReady = false;

        private Dictionary<string, string> localizedText;

        public Action  OnChangeLanguage;
        private void Awake()
        {
            if(_instance)
            {
                Destroy(gameObject);
                return;
            }
            else _instance = this;

            fileName = defaultLocalizedText; 
            StartCoroutine(LoadLocalizedText());
            DontDestroyOnLoad(gameObject);
        }

        public IEnumerator LoadLocalizedText()
        {
            localizedText = new Dictionary<string, string>();
            string filePath = Path.Combine(Application.persistentDataPath, filePathName + fileName + extensionName);

            while (!File.Exists(filePath))
            {
                Debug.Log("oui"); 
                yield return null; 
            }

            string dataJson = Resources.Load<TextAsset>(fileName).text;
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataJson);

            for(int i = 0; i < loadedData.items.Length; i++)
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }
            isReady = true;
            StopAllCoroutines(); 
        }

        public string GetLocalizedValue(string key)
        {
            if(isReady) return localizedText[key];
            else return "Not ready"; 
        }

        public void ChooseLanguage()
        {
            fileName = fileName == defaultLocalizedText ? frenchLocalizedText : defaultLocalizedText;
            Debug.Log(fileName);
            StartCoroutine(LoadLocalizedText());
            OnChangeLanguage?.Invoke();
        }
    }
}