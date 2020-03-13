///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 12/02/2020 14:16
///-----------------------------------------------------------------

using UnityEditor;
using UnityEngine;
using System.Collections;

namespace Com.IsartDigital.Platformer
{
    [CustomPropertyDrawer(typeof(Sound))]
    public class SoundDrawer : PropertyDrawer
    {
        private float lineNumber = 20;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            #region Infos
            //string
            SerializedProperty name = property.FindPropertyRelative("_name");
            //SoundTypes
            SerializedProperty type = property.FindPropertyRelative("_type");
            //AudioClip
            SerializedProperty clip = property.FindPropertyRelative("_clip");

            #endregion

            #region Volume properties

            //float
            SerializedProperty volume = property.FindPropertyRelative("_volume");
            //float
            SerializedProperty volumeVariance = property.FindPropertyRelative("_volumeVariance");

            #endregion

            #region Pitch properties

            //float
            SerializedProperty pitch = property.FindPropertyRelative("_pitch");
            //float
            SerializedProperty pitchVariance = property.FindPropertyRelative("_pitchVariance");
            //bool
            SerializedProperty isPitchedBetweenValues = property.FindPropertyRelative("_isPitchedBetweenValues");
            //float
            SerializedProperty minPitchValue = property.FindPropertyRelative("_minPitchValue");
            //flaot
            SerializedProperty maxPitchValue = property.FindPropertyRelative("_maxPitchValue");
            //flaot
            SerializedProperty rolloffMode = property.FindPropertyRelative("rolloffMode");

            SerializedProperty minDistance = property.FindPropertyRelative("minDistance");

            SerializedProperty volumeSpatialization = property.FindPropertyRelative("volumeSpatialization");

            #endregion

            #region Other properties

            //bool
            SerializedProperty isLoop = property.FindPropertyRelative("_isLoop");
            //AudioMixerGroup
            SerializedProperty mixerGroup = property.FindPropertyRelative("_mixerGroup");
            //AudioSource
            SerializedProperty source = property.FindPropertyRelative("_source");
            //bool
            SerializedProperty showInEditor = property.FindPropertyRelative("showInEditor");

            #endregion

            #region Positions

            float lineHeight = position.height / lineNumber;
            Rect globalRect = new Rect(position.x, position.y, position.width, position.height * 0.9f);

            Rect titleRect = new Rect(position.x,
                position.y,
                position.width,
                lineHeight * 1.1f);

            Rect topInfosLeftRect = new Rect(position.x,
                position.y + lineHeight * 1f,
                position.width * 0.65f,
                lineHeight);

            Rect topInfosRightRect = new Rect(position.x + position.width * 0.68f,
                position.y + lineHeight * 1f + 2.5f,
                position.width * 0.3f,
                lineHeight);

            Rect bottomInfosLeftRect = new Rect(position.x,
                position.y + lineHeight * 2.2f,
                position.width * 0.65f,
                lineHeight);

            Rect bottomInfosMiddleRect = new Rect(position.x + position.width * 0.7f,
                position.y + lineHeight * 2.2f + 2.8f,
                position.width * 0.1f,
                lineHeight);

            Rect bottomInfosRightRect = new Rect(position.x + position.width * 0.7f + 40,
                position.y + lineHeight * 2.1f + 2.8f,
                position.width * 0.5f,
                lineHeight);

            Rect volumeLeftRect = new Rect(position.x + 15,
                position.y + lineHeight * 4f,
                200,
                lineHeight);

            Rect volumeRightRect = new Rect(position.x + 60,
                position.y + lineHeight * 4f + 2.5f,
                position.width - 70,
                lineHeight);

            Rect volumeVarLeftRect = new Rect(position.x + 15,
                position.y + lineHeight * 5f,
                200,
                lineHeight);

            Rect volumeVarRightRect = new Rect(position.x + 110,
                position.y + lineHeight * 5f + 2.5f,
                position.width - 120,
                lineHeight);

            Rect pitchLeftRect = new Rect(position.x + 15,
                position.y + lineHeight * 7f,
                200,
                lineHeight);

            Rect pitchRightRect = new Rect(position.x + 60,
                position.y + lineHeight * 7f + 2.5f,
                position.width - 70,
                lineHeight);

            Rect pitchBoolLeftRect = new Rect(position.x + 15,
                position.y + lineHeight * 8f + 2.5f,
                200,
                lineHeight);

            Rect pitchBoolMiddleRect = new Rect(position.x + 60,
                position.y + lineHeight * 8f + 2.5f,
                position.width * 0.6f - 70,
                lineHeight);

            Rect pitchBoolRightRect = new Rect(position.x + 60 + 175,
                position.y + lineHeight * 8f + 2.5f,
                position.width * 0.3f,
                lineHeight);

            Rect pitchVarLeftRect = new Rect(position.x + 15,
                position.y + lineHeight * 9.5f,
                200,
                lineHeight);

            Rect pitchVarRightRect = new Rect(position.x + 110,
                position.y + lineHeight * 9.5f + 2.5f,
                position.width - 120,
                lineHeight);

            Rect pitchMinLeftRect = new Rect(position.x + 15,
                position.y + lineHeight * 9.5f + 2.5f,
                200,
                lineHeight);

            Rect pitchMinRightRect = new Rect(position.x + 110,
                position.y + lineHeight * 9.5f + 2.5f,
                position.width * 0.2f,
                lineHeight);

            Rect pitchMaxLeftRect = new Rect(position.x + position.width * 0.5f,
                position.y + lineHeight * 9.5f + 2.5f,
                200,
                lineHeight);

            Rect pitchMaxRightRect = new Rect(position.x + position.width * 0.5f + 95,
                position.y + lineHeight * 9.5f + 2.5f,
                position.width * 0.2f,
                lineHeight);

            Rect spatializeHeaderRect = new Rect(position.x + position.width * 0.4f,
                position.y + lineHeight * 11f + 2.5f,
                200,
                lineHeight);

            Rect spatializeLeftRect = new Rect(position.x + 15,
                position.y + lineHeight * 12.5f + 2.5f,
                200,
                lineHeight);

            Rect spatializeRightRect = new Rect(position.x + 85,
                position.y + lineHeight * 12.5f + 2.5f,
                position.width * 0.2f,
                lineHeight);

            Rect spatializeLeft2Rect = new Rect(position.x + position.width * 0.4f,
                position.y + lineHeight * 12.5f + 2.5f,
                position.width * 0.2f,
                lineHeight);

            Rect spatializeRight2Rect = new Rect(position.x + position.width * 0.4f + 75,
                position.y + lineHeight * 12.5f + 2.5f,
                position.width * 0.2f,
                lineHeight);

            Rect spatializeLeft3Rect = new Rect(position.x + position.width * 0.4f,
                position.y + lineHeight * 13.5f + 2.5f,
                position.width * 0.2f,
                lineHeight);

            Rect spatializeRight3Rect = new Rect(position.x + position.width * 0.4f + 75,
                position.y + lineHeight * 13.5f + 2.5f,
                position.width * 0.2f,
                lineHeight);

            Rect spatializeCurveTitleLeftRect = new Rect(position.x + 15,
                position.y + lineHeight * 15.5f + 2.5f,
                200,
                lineHeight);
                        
            Rect spatializeCurveTitleRightRect = new Rect(position.x + 15 + position.width * 0.3f,
                position.y + lineHeight * 15.5f + 2.5f,
                200,
                lineHeight);

            Rect foldoutRect = showInEditor.boolValue ? titleRect : position;
            #endregion

            showInEditor.boolValue = EditorGUI.Foldout(foldoutRect, showInEditor.boolValue, name.stringValue);

            if (!showInEditor.boolValue) return;

            EditorGUI.BeginProperty(position, label, property);

            //Rect for bakcground
            GUI.Box(globalRect, "");

            EditorGUI.PropertyField(topInfosLeftRect, name, GUIContent.none);
            EditorGUI.PropertyField(topInfosRightRect, type, GUIContent.none);
            EditorGUI.PropertyField(bottomInfosLeftRect, clip, GUIContent.none);
            isLoop.boolValue = EditorGUI.Toggle(bottomInfosMiddleRect, isLoop.boolValue);

            string infoLoop = isLoop.boolValue ? "is Loop" : "no Loop";
            GUI.Label(bottomInfosRightRect, infoLoop);

            GUI.Label(volumeLeftRect, "Volume");
            EditorGUI.Slider(volumeRightRect, volume, 0, 1, GUIContent.none);

            GUI.Label(volumeVarLeftRect, "Volume variance");
            EditorGUI.Slider(volumeVarRightRect, volumeVariance, 0, 1, GUIContent.none);

            GUI.Label(pitchLeftRect, "Pitch");
            EditorGUI.Slider(pitchRightRect, pitch, 0, 1, GUIContent.none);

            isPitchedBetweenValues.boolValue = EditorGUI.Toggle(pitchBoolLeftRect, isPitchedBetweenValues.boolValue);
            GUI.Label(pitchBoolMiddleRect, "is Pitched Between Value ?");

            string statePitchBool = isPitchedBetweenValues.boolValue ? "BETWEEN VALUES" : "AROUND VARIANCE";
            GUI.Label(pitchBoolRightRect, statePitchBool);

            if (isPitchedBetweenValues.boolValue)
            {
                GUI.Label(pitchMinLeftRect, "Minimum Pitch");
                EditorGUI.PropertyField(pitchMinRightRect, minPitchValue, GUIContent.none);

                GUI.Label(pitchMaxLeftRect, "Maximum Pitch");
                EditorGUI.PropertyField(pitchMaxRightRect, maxPitchValue, GUIContent.none);
            }
            else
            {
                GUI.Label(pitchVarLeftRect, "Pitch variance");
                EditorGUI.Slider(pitchVarRightRect, pitchVariance, 0, 1, GUIContent.none);
            }

            GUI.Label(spatializeHeaderRect, "Spatialization settings",GUIStyle.none);

            GUI.Label(spatializeLeftRect, "Rolloff Mode");
            EditorGUI.PropertyField(spatializeRightRect, rolloffMode, GUIContent.none);
            
            GUI.Label(spatializeLeft2Rect, "Min Distance");
            EditorGUI.PropertyField(spatializeRight2Rect, minDistance, GUIContent.none);
            
            GUI.Label(spatializeLeft3Rect, "Max Distance");
            EditorGUI.PropertyField(spatializeRight3Rect, minDistance, GUIContent.none);

            GUI.Label(spatializeCurveTitleLeftRect, "Custom Curve Spatialization");
            EditorGUI.PropertyField(spatializeCurveTitleRightRect, volumeSpatialization, GUIContent.none);


            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property,
                                        GUIContent label)
        {
            // Use Unity's default height, which is a single line
            // in the inspector
            //bool
            SerializedProperty showInEditor = property.FindPropertyRelative("showInEditor");

            //return base.GetPropertyHeight(property, label) * (lineNumber + 2);
            if (showInEditor.boolValue) return base.GetPropertyHeight(property, label) * (lineNumber + 2);
            else return base.GetPropertyHeight(property, label);
        }
    }
}