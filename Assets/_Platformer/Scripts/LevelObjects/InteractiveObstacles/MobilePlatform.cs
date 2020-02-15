///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 27/01/2020 16:48
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.InteractiveObstacles {
	public class MobilePlatform : MonoBehaviour {

        [SerializeField] private Transform[] allPoints;
        private Vector2 startPos;
        private float elapsedTime;
        private uint index = 1;
        [SerializeField]private float duration;
        [SerializeField]private string playerTag = "Player"; 

        [SerializeField] private bool _isStarted = false;
        private static List<MobilePlatform> _list = new List<MobilePlatform>();

        public bool IsStarted
        {
            get
            {
                return _isStarted;
            }
            set
            {
                _isStarted = value;
            }
        }

        private Action DoAction;

        private void Start()
        {
            _list.Add(this);
            SetStartPosition();
            SetModeNormal();
        }

        private void SetModeVoid() 
        {
            DoAction = DoActionVoid;
        }

        private void SetModeNormal() 
        {
            DoAction = DoActionNormal;
        }

        private void DoActionVoid()
        {

        }

        private void DoActionNormal()
        {
            if (!IsStarted) return;

            elapsedTime += Time.deltaTime;
            transform.position = Vector2.Lerp(index > 0 ? allPoints[index - 1].position : allPoints[allPoints.Length - 1].position, allPoints[index].position, elapsedTime / duration);

            if (elapsedTime >= duration)
            {
                if (index >= allPoints.Length - 1) index = 0;
                else index++;
                
                elapsedTime = 0;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("sur une plateforme mobile");
            if (collision.CompareTag(playerTag))
            {
                collision.transform.SetParent(transform);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(playerTag))
            {
                collision.transform.SetParent(null);
            }
        }

        private void Update()
        {
            DoAction();
        }

        private void SetStartPosition()
        {
            startPos = allPoints[0].position;
        }

        private void OnDestroy()
        {
            _list.Remove(this);
        }

        public static void ResetAll()
        {
            for (int i = _list.Count - 1; i >= 0; i--)
            {
                _list[i].SetStartPosition();
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
                _list[i].SetModeNormal();
            }
        }
    }
}