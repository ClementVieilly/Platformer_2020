///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 04/02/2020 14:41
///-----------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.InteractiveObstacles {
    public class TimedDoor : MonoBehaviour
    {
        private static List<TimedDoor> _list = new List<TimedDoor>();

        private bool isOpening = false;
        private bool isClosing = false;

        [SerializeField] private float openingSpeed = 3f;
        [SerializeField] private float closingSpeed = 3f;

        [SerializeField] private Transform startPos = null;
        [SerializeField] private Transform endPos = null;

        private bool isPaused = false;

        private void Start()
        {
            _list.Add(this);
        }

        private void OnDestroy()
        {
            _list.Remove(this);
        }

        public void Open()
        {
            isOpening = true;
            isClosing = false;
            StartCoroutine(OpenDoor());
        }
        public void Close()
        {
            isClosing = true;
            isOpening = false;
            StartCoroutine(CloseDoor());
        }

        private IEnumerator OpenDoor()
        {
            while ((transform.position != endPos.position) && isOpening)
            {
                while (isPaused)
                {
                    yield return null;
                }

                transform.position = Vector3.MoveTowards(transform.position , endPos.position, openingSpeed *  Time.deltaTime);
                yield return null;
            }
        }

        private IEnumerator CloseDoor()
        {
            while ( (transform.position != startPos.position) && isClosing)
            {
                while (isPaused)
                {
                    yield return null;
                }

                transform.position = Vector3.MoveTowards(transform.position, startPos.position, closingSpeed * Time.deltaTime);
                yield return null;
            }
        }

        private void ResetPosition()
        {
            transform.position = startPos.position;
        }

        public static void ResetAll()
        {
            for (int i = _list.Count - 1; i >= 0; i--)
            {
                _list[i].ResetPosition();
                _list[i].isOpening = false;
                _list[i].isClosing = false;
                _list[i].isPaused = false;
            }
        }

        public static void PauseAll()
        {
            for (int i = _list.Count - 1; i >= 0; i--)
            {
                _list[i].isPaused = true;
            }
        }
        
        public static void ResumeAll()
        {
            for (int i = _list.Count - 1; i >= 0; i--)
            {
                _list[i].isPaused = false;
            }
        }
    }
}