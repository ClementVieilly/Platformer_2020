///-----------------------------------------------------------------
/// Author : 
/// Date : 29/02/2020 16:25
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Common.Curve {
	public class CurveLine : MonoBehaviour {
        [Header("path")]
        [SerializeField] private Vector3[] _pathPoints = null;
        [Space]
        [Header("ThingsToInstatiate")]
        [SerializeField] private GameObject _objectToInstantiate = null;
        [SerializeField] private Transform _objectToInstantiateParent = null;
        [Space]
        [Header("settings")]
        [SerializeField] private float _spacing = 1;
        [SerializeField,Range(1,6)] private int _nLinesToSpawn = 1;

        private GameObject[] _lastPathObject = null;

        public GameObject[] LastPath
        {
            get { return _lastPathObject; }
            set { _lastPathObject = value; }
        }
        public float Spacing => _spacing;
        public int NLinesToSpawn => _nLinesToSpawn;

        public Vector3[] PathPoints
        {
            get { return _pathPoints; }
            set { _pathPoints = value; }
        }

        public Transform ObjectToInstantiateParent => _objectToInstantiateParent;

        public GameObject ObjectToInstantiate => _objectToInstantiate;

        public Vector3 GetPoint(float ratio)
        {
            return transform.TransformPoint(CurveUtils.GetPoints(_pathPoints[0], _pathPoints[1], _pathPoints[2],_pathPoints[3], ratio));
        }

        public Vector3 GetVelocity(float ratio)
        {
            return transform.TransformPoint(CurveUtils.GetFirstDerivate(_pathPoints[0], _pathPoints[1], _pathPoints[2], _pathPoints[3], ratio)) -
                transform.position;
        }

        public Vector3 GetDirection(float ratio)
        {
            return GetVelocity(ratio).normalized;
        }

        public void Reset()
        {
            _pathPoints = new Vector3[]
            {
                new Vector3(1f,0f,0f),
                new Vector3(2f,0f,0f),
                new Vector3(3f,0f,0f),
                new Vector3 (4f,0f,0f)
            };
        }
    }
}