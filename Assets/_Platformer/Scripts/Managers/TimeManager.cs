///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:37
///-----------------------------------------------------------------

using System;
using System.Collections;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Managers
{
	public class TimeManager : MonoBehaviour
	{
        private Action DoAction = null; 

		[SerializeField] private float slowDownFactor = .05f;
        private float elapsedTime = 0;  
        private float _timer = 0; 
		public bool waiting = false;

        private void Start()
        {
            SetModeVoid(); 
        }

        public float Timer { get => _timer; }
        public void StartTimer()
        {
            SetModeTimer(); 
        }

        public void SetModeTimer()
        {
            DoAction = DoActionTimer; 
        }

        private void SetModePause()
        {
            DoAction = DoActionVoid; 
        }
        

        public void SetModeVoid()
        {
            _timer = 0; 
            DoAction = DoActionVoid;
        }
        public void SlowTime()
		{
			Time.timeScale = slowDownFactor;
			Time.fixedDeltaTime = Time.timeScale * .02f;
		}

		public void ResetTime()
		{
			Time.timeScale = 1f;
		}

		public void HitStop(float duration)
		{
			if (waiting) return;

			Time.timeScale = 0;
			StartCoroutine(Wait(duration));
		}

		private IEnumerator Wait(float duration)
		{
			waiting = true;

			yield return new WaitForSecondsRealtime(duration);

			Time.timeScale = 1;
			waiting = false;
		}

        private void Update()
        {
            DoAction(); 
        }

        private void DoActionVoid()
        {

        }

        private void DoActionTimer()
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= 1)
            {
                _timer++;
                Debug.Log(_timer);
                elapsedTime = 0; 
            }
        }
    }
}