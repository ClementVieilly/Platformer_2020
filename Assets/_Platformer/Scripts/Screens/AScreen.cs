///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 22/01/2020 14:26
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.Screens {
	public abstract class AScreen : MonoBehaviour {
        protected Animator animator = null;
        protected string enter = "Enter";
        protected string exit = "Exit"; 
		abstract public void UnsubscribeEvents();
	}
}