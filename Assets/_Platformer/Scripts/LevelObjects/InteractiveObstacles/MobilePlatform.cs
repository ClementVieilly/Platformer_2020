///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 27/01/2020 16:48
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.InteractiveObstacles {
	public class MobilePlatform : MonoBehaviour {

        [SerializeField] private Transform[] allPoints = null;
        private Vector2 startPos = Vector2.zero;
        private float elapsedTime = 0f;
        private uint index = 1;
        private uint startIndex = 0;
        [SerializeField] private float duration = 0f;
        [SerializeField] private float timeBeforeStart = 0f;
        [SerializeField] private string playerTag = "Player"; 
        [SerializeField] private bool isStarted = false; 
        [SerializeField] private bool oneWay = false; 

        private static List<MobilePlatform> _list = new List<MobilePlatform>();

        private Transform touchedObject = null;

        private Action DoAction;

        private void Start()
        {
            if(isStarted) SetModeNormal();
            else SetModeWait();

            _list.Add(this);
            SetStartPosition();
            startIndex = index;
        }

        public void SetModeWait() 
        {
            DoAction = DoActionWait;
        }

        public void SetModeNormal() 
        {
            DoAction = DoActionNormal;
        }

        public void SetModeVoid()
        {
            DoAction = DoActionVoid; 
        }

        private void DoActionVoid() {}

        private void DoActionWait()
        {
			if (timeBeforeStart == 0f) return;

            elapsedTime += Time.deltaTime;

			if (elapsedTime >= timeBeforeStart)
			{
				elapsedTime = 0f;
				SetModeNormal();
			}
		}

		private void DoActionNormal()
        {
            elapsedTime += Time.deltaTime;
            Vector3 previousPos = transform.position;
            
            transform.position = Vector2.Lerp(index > 0 ? 
				allPoints[index - 1].position : allPoints[allPoints.Length - 1].position, 
				allPoints[index].position, elapsedTime / duration);

            if (touchedObject != null) touchedObject.position += transform.position - previousPos;
            if (elapsedTime >= duration)
            {
                if(oneWay && index == allPoints.Length - 1)
                {
                    SetModeVoid();
                    return; 
                }

                if(index >= allPoints.Length - 1) index = 0;
                else index++;
                elapsedTime = 0;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag(playerTag))
                touchedObject = collision.transform;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider.CompareTag(playerTag))
                touchedObject = null;
        }

        private void FixedUpdate()
        {
            DoAction();
        }

        private void SetStartPosition()
        {
			transform.position = startPos = allPoints[0].position;
        }

        private void OnDestroy()
        {
            _list.Remove(this);
        }

        public static void ResetAll()
        {
            for (int i = _list.Count - 1; i >= 0; i--)
            {
                _list[i].transform.position = _list[i].startPos;
                _list[i].elapsedTime = 0;
                _list[i].index = _list[i].startIndex;
            }
        }

        public static void PauseAll()
        {
            for (int i = _list.Count - 1; i >= 0; i--)
            {
                _list[i].SetModeVoid();
            }
        }

        public static void ResumeAll()
        {
            for (int i = _list.Count - 1; i >= 0; i--)
            {
				if (_list[i].isStarted)
					_list[i].SetModeNormal();
				else
					_list[i].SetModeWait();
			}
		}
    }
}