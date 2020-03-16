///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 12/03/2020 12:15
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Localization;
using Com.IsartDigital.Platformer.Screens;
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
        private string _fileName = null; 
        private string dataJson;
        public static bool toggleBool = false; 
        public static bool isToggleChanged = false;
        public bool isPreload = false;
        public bool notFinished = true;
       

        public Dictionary<string, string> localizedText;

        public  Action OnLoadFinished;
        public  Action OnChangeLanguage;

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
            }
        }

        private void Awake()
        {
            if(_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else _instance = this;

            _fileName = currentFileName;
            DontDestroyOnLoad(gameObject);
            TitleCard.OnChangeLanguage += TitleCard_OnChangeLanguage;
        }

        private void TitleCard_OnChangeLanguage(TitleCard title)
        {
            Debug.Log("callBack de l'event de la TitleCard"); 
            _fileName = _fileName == defaultLocalizedText ? frenchLocalizedText : defaultLocalizedText;
#if UNITY_ANDROID && !UNITY_EDITOR
            StartCoroutine(LoadLocalizedTextOnAndroid());
#else
            StartCoroutine(LoadLocalizedText());
#endif
        }

        public IEnumerator LoadLocalizedText()
        {
            localizedText = new Dictionary<string, string>();
            localizedText.Clear();

            string filePath = Path.Combine(Application.streamingAssetsPath,  _fileName);

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
            if(isPreload) OnLoadFinished?.Invoke();
            else OnChangeLanguage?.Invoke(); 
        }

      public  IEnumerator LoadLocalizedTextOnAndroid()
        {
            Debug.Log("je rentre dans la coroutine"); 
            while(notFinished)
            {
                string filePath = Path.Combine("jar:file://" + Application.dataPath + "!/assets", _fileName);
                localizedText = new Dictionary<string, string>();
                UnityWebRequest www = UnityWebRequest.Get(filePath);
                yield return www.SendWebRequest();
                dataJson = www.downloadHandler.text;
                notFinished = false; 
            }
            Debug.Log("j'ai recup le fichier"); 
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataJson);
            Debug.Log("le dictio   " + localizedText);
            Debug.Log("les data    "   +  loadedData); 

            for(int i = 0; i < loadedData.items.Length; i++)
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }
            if(isPreload) OnLoadFinished?.Invoke();
            else
            {
                OnChangeLanguage?.Invoke();
                Debug.Log("J'invoque l'event"); 
            }
            notFinished = true; 
        }

        public string GetLocalizedValue(string key)
        {
            return localizedText[key]; 
        }
    }
}