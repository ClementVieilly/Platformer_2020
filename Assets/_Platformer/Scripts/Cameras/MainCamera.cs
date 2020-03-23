///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 10/02/2020 14:03
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.Cameras {
	public class MainCamera : MonoBehaviour
	{
        private static MainCamera _instance;
        public static MainCamera Instance => _instance;

        private void Start()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(_instance.gameObject);
                _instance = this;
                return;
            }
            else _instance = this;
        }

        private void OnDestroy()
        {
            if (this == _instance) _instance = null;
        }
    }
}