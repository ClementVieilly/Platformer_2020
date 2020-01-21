///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:35
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.LevelObjects.Controllers;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Managers {
	public class InputManager : MonoBehaviour {
		private static InputManager instance;
		public static InputManager Instance { get { return instance; } }

        [HideInInspector] public AController controller;

        //delegate gérant l'événement de pause
        public delegate void OnKeyPause();
        public OnKeyPause OnKeyPausePressed; 

		private void Awake(){
			if (instance){
				Destroy(gameObject);
				return;
			}
			
			instance = this;

            //défini le controller en fonction du device
            #if UNITY_ANDROID && !UNITY_EDITOR
			            controller = new TouchController();
            #else
                        controller = new KeyboardController();
            #endif

        }

        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) OnKeyPausePressed?.Invoke(); //envoie de l'event de pause 
        }



        private void OnDestroy(){
			if (this == instance) instance = null;
		}
	}
}