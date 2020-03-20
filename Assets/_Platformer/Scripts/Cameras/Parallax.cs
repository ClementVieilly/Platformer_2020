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
		public static List<Parallax> list = new List<Parallax>();

		[SerializeField] private Transform cam;
		[SerializeField] private float parallaxRatioX = 0.3f;
		[SerializeField] private float parallaxRatioY = 0.3f;
		[SerializeField] private bool isLockedY = false;
		[SerializeField] private Level level;
		private bool firstUpdate = true;

		private Vector2 refPos;
		private Vector3 lastCamPos;
		private Vector3 movementSinceLastFrame;
		private Vector3 startPos;

		private Action DoAction;
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

			if (GetComponent<Collider2D>() != null) SetModeVoid();
			else SetModeNormal();

			Vector2 pos = new Vector2();
			Transform obj;

			//for (int i = objs.Length - 1; i >= 0; i--)
			//{
			//	obj = objs[i];
			//	pos.x = (obj.localPosition.x - refPos.x) * parallaxRatioX;
			//	//obj.position += (Vector3)pos;
			//	if (obj.localPosition.x >= 0) obj.position += (Vector3)pos;
			//	else obj.position -= (Vector3)pos;
			//}

			for (int i = 0; i < objs.Length; i++)
			{
				obj = objs[i];
				pos.x = (obj.localPosition.x - refPos.x) * parallaxRatioX;
				obj.position += (Vector3)pos;
			}

			gameObject.transform.position = startPos;
		}

		private void LateUpdate()
		{
			DoAction();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.gameObject.GetComponent<Player>())
			SetModeNormal();
		}
		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.gameObject.GetComponent<Player>())
				SetModeVoid();
		}

		private void SetModeVoid()
		{
			Debug.Log("is void");
			DoAction = DoActionVoid;
		}

		private void SetModeNormal()
		{
			DoAction = DoActionNormal;
		}

		private void DoActionVoid()	{}

		private void DoActionNormal()
		{
			UpdatePos();
			if (transform.position != startPos && firstUpdate)
			{
				SetStartPos();
			}
		}

		private void UpdatePos()
		{
			movementSinceLastFrame = cam.position - lastCamPos;
			if (isLockedY) movementSinceLastFrame.y = 0;
			transform.position -= new Vector3(movementSinceLastFrame.x * parallaxRatioX, movementSinceLastFrame.y * parallaxRatioY);
			lastCamPos = cam.position;
		}



		private void SetStartPos()
		{
			transform.position = startPos;
			firstUpdate = false;
		}

	}
}