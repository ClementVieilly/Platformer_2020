///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 12/02/2020 14:16
///-----------------------------------------------------------------

using UnityEditor;
using UnityEngine;
using System.Collections;
using Com.IsartDigital.Platformer.Sounds;

namespace Com.IsartDigital.Platformer
{
    [CustomPropertyDrawer(typeof(Sound))]
    public class SoundDrawer : PropertyDrawer
    {
        private float lineNumber = 24;
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
            //float
            SerializedProperty maxPitchValue = property.FindPropertyRelative("_maxPitchValue");

            #endregion

            #region Spatialization properties

            //AudioRolloffMode
            SerializedProperty _rolloffMode = property.FindPropertyRelative("_rolloffMode");
            //float
            SerializedProperty _minDistance = property.FindPropertyRelative("_minDistance");
            //float
            SerializedProperty _maxDistance = property.FindPropertyRelative("_maxDistance");
            //AnimationCurve
            SerializedProperty _volumeSpatialization = property.FindPropertyRelative("_volumeSpatialization");

            #endregion

            #region Fade properties

            //bool
            SerializedProperty _isFadeIn = property.FindPropertyRelative("_isFadeIn");
            //bool
            SerializedProperty _isFadeOut = property.FindPropertyRelative("_isFadeOut");
            //AnimationCurve
            SerializedProperty _fadeInCurve = property.FindPropertyRelative("_fadeInCurve");
            //AnimationCurve
            SerializedProperty _fadeOutCurve = property.FindPropertyRelative("_fadeOutCurve");
            //float
            SerializedProperty _fadeInDuration = property.FindPropertyRelative("_fadeInDuration");
            //float
            SerializedProperty _fadeOutDuration = property.FindPropertyRelative("_fadeOutDuration");
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
                        
            Rect spatializeCurveTitleRightRect = new Rect(position.x + position.width * 0.2f,
                position.y + lineHeight * 15.5f + 2.5f,
                position.width * 0.6f,
                lineHeight);

            Rect fadeHeaderRect = new Rect(position.x + position.width * 0.4f,
                position.y + lineHeight * 17.5f + 2.5f,
                200,
                lineHeight);

            Rect fadeInCurveTitleLeftRect = new Rect(position.x + 15,
                position.y + lineHeight * 18.5f + 2.5f,
                position.width * 0.15f,
                lineHeight);

            Rect fadeInCurveTitleRightRect = new Rect(position.x + position.width * 0.2f,
                position.y + lineHeight * 18.5f + 2.5f,
                position.width * 0.4f,
                lineHeight);

            Rect fadeInBoolLeftRect = new Rect(position.x + position.width * 0.62f,
                position.y + lineHeight * 18.5f + 2.5f,
                position.width * 0.05f,
                lineHeight);

            Rect fadeInBooRightRect = new Rect(position.x + position.width * 0.62f + 35,
                position.y + lineHeight * 18.5f + 2.5f,
                position.width * 0.1f,
                lineHeight);


            Rect fadeInTimelLeftRect = new Rect(position.x + position.width * 0.77f,
                position.y + lineHeight * 18.5f + 2.5f,
                position.width * 0.1f,
                lineHeight);

            Rect fadeInTimeRightRect = new Rect(position.x + position.width * 0.78f + 35,
                position.y + lineHeight * 18.5f + 2.5f,
                position.width * 0.1f,
                lineHeight);


            Rect fadeOutCurveTitleLeftRect = new Rect(position.x + 15,
                position.y + lineHeight * 19.5f + 2.5f,
                position.width * 0.15f,
                lineHeight);

            Rect fadeOutCurveTitleRightRect = new Rect(position.x + position.width * 0.2f,
                position.y + lineHeight * 19.5f + 2.5f,
                position.width * 0.4f,
                lineHeight);

            Rect fadeOutBoolLeftRect = new Rect(position.x + position.width * 0.62f,
                position.y + lineHeight * 19.5f + 2.5f,
                position.width * 0.05f,
                lineHeight);

            Rect fadeOutBooRightRect = new Rect(position.x + position.width * 0.62f + 35,
                position.y + lineHeight * 19.5f + 2.5f,
                position.width * 0.1f,
                lineHeight);
            
            Rect fadeOutTimelLeftRect = new Rect(position.x + position.width * 0.77f,
                position.y + lineHeight * 19.5f + 2.5f,
                position.width * 0.1f,
                lineHeight);

            Rect fadeOutTimeRightRect = new Rect(position.x + position.width * 0.78f + 35,
                position.y + lineHeight * 19.5f + 2.5f,
                position.width * 0.1f,
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
            EditorGUI.PropertyField(spatializeRightRect, _rolloffMode, GUIContent.none);
            
            GUI.Label(spatializeLeft2Rect, "Min Distance");
            EditorGUI.PropertyField(spatializeRight2Rect, _minDistance, GUIContent.none);
            
            GUI.Label(spatializeLeft3Rect, "Max Distance");
            EditorGUI.PropertyField(spatializeRight3Rect, _maxDistance, GUIContent.none);

            GUI.Label(spatializeCurveTitleLeftRect, "Custom Curve");
            EditorGUI.PropertyField(spatializeCurveTitleRightRect, _volumeSpatialization, GUIContent.none);

            GUI.Label(fadeHeaderRect, "Fade settings", GUIStyle.none);

            GUI.Label(fadeInCurveTitleLeftRect, "Fade In Curve");
            EditorGUI.PropertyField(fadeInCurveTitleRightRect, _fadeInCurve, GUIContent.none);

            _isFadeIn.boolValue = EditorGUI.Toggle(fadeInBoolLeftRect, _isFadeIn.boolValue);
            GUI.Label(fadeInBooRightRect, _isFadeIn.boolValue ? "is On" : "is Off");

            GUI.Label(fadeInTimelLeftRect, "Duration");
            EditorGUI.PropertyField(fadeInTimeRightRect, _fadeInDuration, GUIContent.none);

            GUI.Label(fadeOutCurveTitleLeftRect, "Fade Out Curve");
            EditorGUI.PropertyField(fadeOutCurveTitleRightRect, _fadeOutCurve, GUIContent.none);

            _isFadeOut.boolValue = EditorGUI.Toggle(fadeOutBoolLeftRect, _isFadeOut.boolValue);
            GUI.Label(fadeOutBooRightRect, _isFadeOut.boolValue ? "is On" : "is Off");

            GUI.Label(fadeOutTimelLeftRect, "Duration");
            EditorGUI.PropertyField(fadeOutTimeRightRect, _fadeOutDuration, GUIContent.none);

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