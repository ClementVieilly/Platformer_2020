///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:40
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.Platforms {
	public class DestructiblePlatform : Platform {

        private static List<DestructiblePlatform> _list = new List<DestructiblePlatform>();
        public static List<DestructiblePlatform> List => _list;

        [SerializeField] private float duration = 0f;
        [SerializeField] private GameObject triggeredCollider = null;
        private float elapsedTime = 0f;

        private Action DoAction = null;
        private Action PreviousDoAction = null;

        private void Start()
        {
            _list.Add(this);
            SetModeVoid();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            SetModeNormal();
        }

        private void Update()
        {
            DoAction();
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
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= duration) 
            {
                triggeredCollider.SetActive(false);
                SetModeVoid();
            }
        }

        private void OnDestroy()
        {
            _list.Remove(this);
        }

        public static void ResetAll()
        {
            Debug.Log("reset destructible platforms");
            for (int i = List.Count - 1; i >= 0; i--)
            {
                List[i].triggeredCollider.SetActive(true);
                List[i].elapsedTime = 0;
            }
        }

        public static void PauseAll()
        {
            for (int i = List.Count - 1; i >= 0; i--)
            {
                List[i].PreviousDoAction = List[i].DoAction;
                List[i].SetModeVoid();
            }
        }
        
        public static void ResumeAll()
        {
            for (int i = List.Count - 1; i >= 0; i--)
            {
                List[i].DoAction = List[i].PreviousDoAction;
            }
        }
    }
}