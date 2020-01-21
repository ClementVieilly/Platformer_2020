///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 21/01/2020 12:05
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.Controllers {
    public abstract class AController
    {

        protected string _horizontalAxis = "Horizontal";
        protected string _jump = "Jump";

        public AController()
        {
            Debug.Log(Input.GetAxis(_jump));
            
        }

        public float HorizontalAxis { get { return Input.GetAxis(_horizontalAxis); } }
        public float Jump { get { return Input.GetAxis(_jump); } }
        
    }
}