///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 04/02/2020 14:41
///-----------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.InteractiveObstacles {
    public class TimedDoor : MonoBehaviour
    {
        private static List<TimedDoor> _list = new List<TimedDoor>();

        private bool isOpening;
        private bool isClosing;

        private float elapsedTime = 0f;
        [SerializeField] private float openingSpeed = 3f;
        [SerializeField] private float closingSpeed = 3f;

        [SerializeField] private Transform startPos;
        [SerializeField] private Transform endPos;

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
                transform.position = Vector3.MoveTowards(transform.position , endPos.position, openingSpeed);

                yield return null;
            }
            elapsedTime = 0;
        }

        private IEnumerator CloseDoor()
        {
            while ( (transform.position != startPos.position) && isClosing)
            {
                elapsedTime += Time.deltaTime;

                transform.position = Vector3.MoveTowards(transform.position, startPos.position, closingSpeed);

                yield return null;
            }
            elapsedTime = 0;
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
            }
        }
    }
}