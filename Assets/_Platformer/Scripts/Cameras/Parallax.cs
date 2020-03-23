///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 25/02/2020 11:41
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.LevelObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Cameras {
	public class Parallax : MonoBehaviour
	{
		[SerializeField] private Transform cam = null;
		[SerializeField] private Level level = null;
		[SerializeField] private float yOffset = 0f;

		[SerializeField] private float parallaxRatioX = 0.3f;
		[SerializeField] private float parallaxRatioY = 0.3f;

		[SerializeField] private bool isLockedY = false;

		private bool firstUpdate = true;

		public static bool isShaking = false;

		private Vector2 refPos;
		private Vector3 lastCamPos;
		private Vector3 velocity;
		private Vector3 startPos;

		private Transform[] objs;

		private void Awake()
		{
			startPos = gameObject.transform.position;
		}

		private void Start()
		{
			refPos = level.StartPos;

			objs = GetComponentsInChildren<Transform>();
			lastCamPos = cam.position;

			Vector2 gap = new Vector2();
			Transform obj;

			for (int i = objs.Length - 1; i >= 0; i--)
			{
				obj = objs[i];
				gap.x = (obj.localPosition.x - refPos.x) * parallaxRatioX;
				if (!isLockedY) gap.y = yOffset;
				obj.position += (Vector3)gap;
			}
			gameObject.transform.position = startPos;
		}

		private void LateUpdate()
		{
			if (isShaking) return;

			UpdatePos();
			if (transform.position != startPos && firstUpdate)
			{
				SetStartPos();
			}
		}

		private void SetStartPos()
		{
			transform.position = startPos;
			firstUpdate = false;
		}

		private void UpdatePos()
		{
            if (!cam) return;
			velocity = cam.position - lastCamPos;
			if (isLockedY) velocity.y = 0;
			transform.position -= new Vector3(velocity.x * parallaxRatioX, velocity.y * parallaxRatioY);
			lastCamPos = cam.position;
		}
	}
}