///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 12/03/2020 13:48
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Localization;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Com.IsartDigital.Platformer {
	public class LocalizedTextEditor : EditorWindow {

        public LocalizationData localizationData;
        private string currentFilePath;
        private string property = "localizationData"; 

        [MenuItem ("Window/Localized Text Editor")]
        static void Init()
        {
            EditorWindow window = EditorWindow.GetWindow(typeof(LocalizedTextEditor));
        }

        private void OnGUI()
        {
            if (localizationData != null)
            {
                SerializedObject serializedObject = new SerializedObject(this);
                SerializedProperty serializedProperty = serializedObject.FindProperty(property);
                EditorGUILayout.PropertyField(serializedProperty, true);
                serializedObject.ApplyModifiedProperties();

                if(GUILayout.Button("Save data"))
                {
                    SaveData();
                }

            }

            if(GUILayout.Button("Load Data"))
            {
                LoadGameData(); 
            }

            if(GUILayout.Button("Create New Data"))
            {
                CreateNewData();
            }
        }
        public void SaveData()
        {
            string filePath = EditorUtility.SaveFilePanel("Save localization Datafile", Application.streamingAssetsPath,"", "json");

            if(!string.IsNullOrEmpty(filePath))
            {
                string dataAsJason = JsonUtility.ToJson(localizationData);
                File.WriteAllText(filePath, dataAsJason); 
            }
        }

        private void LoadGameData()
        {
            string filePath = EditorUtility.OpenFilePanel("Select localization Datafile", Application.streamingAssetsPath,"json");
            if(currentFilePath != null) currentFilePath = filePath;

            if(!string.IsNullOrEmpty(filePath))
            {
                string dataAsJason = File.ReadAllText(filePath);
                localizationData = JsonUtility.FromJson<LocalizationData>(dataAsJason);
            }

            
        }
		private void CreateNewData()
        {
            localizationData = new LocalizationData();
        }
	}
}