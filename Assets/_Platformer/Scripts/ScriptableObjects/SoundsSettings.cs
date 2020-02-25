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
        [SerializeField] private string _character_Footsteps_Stone = null;
        [SerializeField] private string _character_Footsteps_Wood = null;
        [SerializeField] private string _character_Jump = null;
        [SerializeField] private string _character_WallJump = null;
        [SerializeField] private string _character_Landing = null;
        [SerializeField] private string _character_Plane_Flap01 = null;
        [SerializeField] private string _character_Plane_Flap02 = null;
        [SerializeField] private string _character_Plane_Wind = null;
        [SerializeField] private string _ambiance_level_one = null;
        [SerializeField] private string _ambiance_level_two = null;

        public string FootstepsStone { get => _character_Footsteps_Stone;  }
        public string FootstepsWood { get => _character_Footsteps_Wood;  }
        public string Jump { get => _character_Jump;  }
        public string WallJump { get => _character_WallJump;  }
        public string Landing { get => _character_Landing;  }
        public string PlaneFlap01 { get => _character_Plane_Flap01;  }
        public string PlaneFlap02 { get => _character_Plane_Flap02;  }
        public string PlaneWind { get => _character_Plane_Wind;  }
        public string Ambiance_Level_One { get => _ambiance_level_one;  }
        public string Ambiance_Level_Two { get => _ambiance_level_two;  }
    }
}