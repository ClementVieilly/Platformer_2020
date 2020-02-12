///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 11/02/2020 17:16
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer {
	
	[CreateAssetMenu(
		menuName = "Platformer/Sound"
	)]
	
	public class SoundsSettings : ScriptableObject {
        [SerializeField] private string _character_Footsteps_Stone; 
        [SerializeField] private string _character_Footsteps_Wood; 
        [SerializeField] private string _character_Jump; 
        [SerializeField] private string _character_WallJump; 
        [SerializeField] private string _character_Landing; 
        [SerializeField] private string _character_Plane_Flap01; 
        [SerializeField] private string _character_Plane_Flap02; 
        [SerializeField] private string _character_Plane_Wind; 


        public string FootstepsStone { get => _character_Footsteps_Stone;  }
        public string FootstepsWood { get => _character_Footsteps_Wood;  }
        public string Jump { get => _character_Jump;  }
        public string WallJump { get => _character_WallJump;  }
        public string PlaneFlap01 { get => _character_Plane_Flap01;  }
        public string PlaneWind { get => _character_Plane_Wind;  }
    }
}