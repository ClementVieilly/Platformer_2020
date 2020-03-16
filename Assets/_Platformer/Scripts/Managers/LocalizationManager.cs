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
        public string fileName = null; 
        private string dataJson;
        public static bool toggleBool = false; 
        public static bool isToggleChanged = false; 

        public Dictionary<string, string> localizedText;

        public Action  OnChangeLanguage;
        public Action  OnLoadFinished;
        private void Awake()
        {
            if(_instance != null && _instance != this)
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
        
#if UNITY_ANDROID && !UNITY_EDITOR
         
            StartCoroutine(LoadLocalizedTextOnAndroid());
#else
                StartCoroutine(LoadLocalizedText());
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
            while(string.IsNullOrEmpty(dataJson))
            {
                Debug.Log("j'entre dans la coroutine"); 
                string filePath = Path.Combine("jar:file://" + Application.dataPath + "!/assets", fileName);
                localizedText = new Dictionary<string, string>();
                Debug.Log(localizedText); 
                UnityWebRequest www = UnityWebRequest.Get(filePath);
                yield return www.SendWebRequest();
                dataJson = www.downloadHandler.text;
                Debug.Log("je sors du while"); 
            }
            Debug.Log("dataJson   :" + dataJson); 
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataJson);
            Debug.Log("loaded Data  " + loadedData); 
            for(int i = 0; i < loadedData.items.Length; i++)
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }
            dataJson = "";
            loadedData = null; 
            OnLoadFinished?.Invoke();
        }

        public string GetLocalizedValue(string key)
        {
            return localizedText[key]; 
        }

        public void ChooseLanguage()
        {
            if(!isToggleChanged) return; 
            fileName = fileName == defaultLocalizedText ? frenchLocalizedText : defaultLocalizedText;
            Debug.Log("changement de langage  " + localizedText); 
#if UNITY_ANDROID && !UNITY_EDITOR
            StartCoroutine(LoadLocalizedTextOnAndroid());
#else
            StartCoroutine(LoadLocalizedText());
#endif
        }

    }
}