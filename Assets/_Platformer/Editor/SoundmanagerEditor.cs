///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 11/02/2020 21:13
///-----------------------------------------------------------------

using UnityEngine;
using UnityEditor;
using Com.IsartDigital.Platformer.Managers;

namespace Com.IsartDigital.Platformer {
	[CustomEditor(typeof(SoundManager))]
	public class SoundmanagerEditor : Editor
	{
		private int index;
		private string soundName;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			//GUILayout.Space(10);

			DrawButtonArea();
		}

		private void DrawButtonArea()
		{
			SoundManager soundmanager = (SoundManager)target;

			GUILayout.Label("Utilities");

			if (GUILayout.Button("Add Sound"))
			{
				soundmanager.AddSound();
			}

			GUILayout.Space(5);

			GUILayout.BeginHorizontal();

			if (GUILayout.Button("Remove Sound", GUILayout.Width(150)))
			{
				soundmanager.RemoveSound(soundName);
			}
			soundName = EditorGUILayout.TextField(soundName);

			GUILayout.EndHorizontal();

			GUILayout.Space(3);

			GUILayout.TextArea("Indicate the name of the sound you want to delete.");

			GUILayout.Space(5);

			if (GUILayout.Button("Remove last sound"))
			{
				soundmanager.RemoveLastSound();
			}
		}
	}
}