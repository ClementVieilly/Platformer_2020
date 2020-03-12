///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 12/03/2020 12:15
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Managers
{
    public class LocalizationManager : MonoBehaviour
    {

        private static LocalizationManager _instance;
        public static LocalizationManager Instance => _instance;

        private string defaultLocalizedText = "localizedText_en.json";
        private string frenchLocalizedText = "localizedText_fr.json";
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

            DontDestroyOnLoad(gameObject);
            fileName = defaultLocalizedText; 
            LoadLocalizedText();
        }

        public void LoadLocalizedText()
        {
            localizedText = new Dictionary<string, string>();
            string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
            if(File.Exists(filePath))
            {
                string dataJson = File.ReadAllText(filePath);
                LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataJson);

                for(int i = 0; i < loadedData.items.Length; i++)
                {
                    localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
                }
            }
            else Debug.LogError("FilePath doesn't exist !");

            isReady = true;
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
            LoadLocalizedText();
            OnChangeLanguage?.Invoke();
        }
    }
}