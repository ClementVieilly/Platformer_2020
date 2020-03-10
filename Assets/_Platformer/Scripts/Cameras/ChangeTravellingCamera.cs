///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 10/03/2020 11:08
///-----------------------------------------------------------------

using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Cameras {
	public class ChangeTravellingCamera : ChangeCamera
	{
		[Space(10)]
		[SerializeField] private CinemachineDollyCart camPath = null;
		[SerializeField] private float camSpeed = 0;
		public static List<ChangeTravellingCamera> list = new List<ChangeTravellingCamera>();
		private bool isPlaying = false;

		protected override void Start()
		{
			base.Start();
			camPath.m_Speed = 0;
			list.Add(this);
		}

		protected override void OnTriggerEnter2D(Collider2D collision)
		{
			base.OnTriggerEnter2D(collision);
			camPath.m_Speed = camSpeed;
			isPlaying = true;
		}

		protected override void OnTriggerExit2D(Collider2D collision)
		{
			base.OnTriggerExit2D(collision);
			isPlaying = false;
		}

		private void Pause()
		{
			if (isPlaying) camPath.m_Speed = 0;
		}

		private void Resume()
		{
			if (isPlaying) camPath.m_Speed = camSpeed;
		}
		private void ResetCam()
		{
			camPath.m_Speed = 0;
			camPath.m_Position = 0;
			isPlaying = false;
		}

		private void OnDestroy()
		{
			list.Remove(this);
		}

		public static void ResetAll()
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				list[i].ResetCam();
			}
		}

		public static void PauseAll()
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (list[i].isPlaying)
					list[i].Pause();
			}
		}

		public static void ResumeAll()
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (list[i].isPlaying)
					list[i].Resume();
			}
		}
	}
}