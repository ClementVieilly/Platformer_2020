///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:40
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.Platforms {
	public class DestructiblePlatform : Platform {

        private static List<DestructiblePlatform> _list = new List<DestructiblePlatform>();
        public static List<DestructiblePlatform> List => _list;

        [SerializeField] private float duration = 0f;
        [SerializeField] private GameObject triggeredCollider = null;
        [SerializeField] private PlateformType matter;
        private string soundToPlay;
        private float elapsedTime = 0f;

        private Action DoAction = null;
        private Action PreviousDoAction = null;

        //Shake 
        private Vector2 spriteOriginalPos;
        [SerializeField] private float shakeMagnitudeX = 0.2f;
        [SerializeField] private float shakeMagnitudeY = 0.2f;
       // [SerializeField] private GameObject sprite = null ;

        private Animator animator = null; 
        private void Start()
        {
            _list.Add(this);
            SetModeVoid();
            spriteOriginalPos = transform.position;
            animator = GetComponent<Animator>();

            if (matter == PlateformType.GLASS) soundToPlay = sounds.Env_DestructiblePlatform_Glass;
            else if (matter == PlateformType.WOOD) soundToPlay = sounds.Env_DestructiblePlatform_Wood;
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
        public void SetModeNormal()
        {
            DoAction = DoActionNormal;
            animator.SetTrigger("Destruction");
            SoundManager.Instance.Play(soundToPlay, this);
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
            else
            {
                float x = UnityEngine.Random.Range(-1f, 1f) * shakeMagnitudeX;
                float y = UnityEngine.Random.Range(-1f, 1f) * shakeMagnitudeY;
                transform.position = new Vector2(x, y) + spriteOriginalPos;
            }
        }

        private void OnDestroy()
        {
            _list.Remove(this);
        }

        public static void ResetAll()
        {
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

    public enum PlateformType
    {
        WOOD,
        GLASS
    }
}