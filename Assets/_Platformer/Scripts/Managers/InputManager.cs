///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:35
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.Managers {
	public class InputManager : MonoBehaviour {
		private static InputManager instance;
		public static InputManager Instance { get { return instance; } }
		
		private void Awake(){
			if (instance){
				Destroy(gameObject);
				return;
			}
			
			instance = this;
		}
		
		private void Start () {
			
		}
		
		private void Update () {
			
		}
		
		private void OnDestroy(){
			if (this == instance) instance = null;
		}
	}
}