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
using UnityEngine.Networking;

namespace Com.IsartDigital.Platformer.Managers
{
    public class LocalizationManager : MonoBehaviour
    {

        private static LocalizationManager _instance;
        public static string currentFileName = "localizedText_en.json"; 
        public static LocalizationManager Instance => _instance;

        private string defaultLocalizedText = "localizedText_en.json";
        private string frenchLocalizedText =  "localizedText_fr.json";
        public string fileName;
        private string dataJson;
        public static bool toggleBool = false; 
        public static bool isFisrtTime = false; 

        private Dictionary<string, string> localizedText;

        public Action  OnChangeLanguage;
        public Action  OnLoadFinished;
        private void Awake()
        {
            if(_instance)
            {
                Destroy(gameObject);
                return;
            }
            else _instance = this;
        }
        private void Start()
        {
            fileName = currentFileName;
            DontDestroyOnLoad(gameObject);

            Debug.Log("Start");
#if UNITY_ANDROID && !UNITY_EDITOR
           if(isFisrtTime) {
            StartCoroutine(LoadLocalizedTextOnAndroid());
            isFisrtTime = true; 
            }
#else
            if(isFisrtTime)
            {
                StartCoroutine(LoadLocalizedText());
                isFisrtTime = true; 
            }
#endif
        }

        public IEnumerator LoadLocalizedText()
        {
            localizedText = new Dictionary<string, string>();
            string filePath = Path.Combine(Application.streamingAssetsPath,  fileName);

             while (!File.Exists(filePath))
             {
                 Debug.Log("FilePath introuvable"); 
                 yield return null; 
             }

            string dataJson = File.ReadAllText(filePath);
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataJson);

            for(int i = 0; i < loadedData.items.Length; i++)
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }
            OnLoadFinished?.Invoke();
        }

        IEnumerator LoadLocalizedTextOnAndroid()
        {
            Debug.Log(fileName); 
            while(string.IsNullOrEmpty(dataJson))
            {
                localizedText = new Dictionary<string, string>();
                string filePath = Path.Combine("jar:file://" + Application.dataPath + "!/assets", fileName);

                UnityWebRequest www = UnityWebRequest.Get(filePath);
                yield return www.Send();
                dataJson = www.downloadHandler.text;
            }

            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataJson);
            for(int i = 0; i < loadedData.items.Length; i++)
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }
            dataJson = "";
            OnLoadFinished?.Invoke(); 
        }

        public string GetLocalizedValue(string key)
        {
            return localizedText[key]; 
        }

        public void ChooseLanguage()
        {
            fileName = fileName == defaultLocalizedText ? frenchLocalizedText : defaultLocalizedText;
#if UNITY_ANDROID && !UNITY_EDITOR
            StartCoroutine(LoadLocalizedTextOnAndroid());
#else
            StartCoroutine(LoadLocalizedText());
#endif
        }

    }
}