///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 15/01/2020 15:17
///-----------------------------------------------------------------

using Com.IsartDigital.Common.ScriptableObjects;
using UnityEngine;

namespace Com.IsartDigital.Platformer.ScriptableObjects
{
	[CreateAssetMenu(menuName = "Platformer/Player Settings")]
	public class PlayerSettings : ScriptableObject
	{
		[Header("Animator Parameters")]
		[SerializeField] private AnimatorParameter _isGroundedParam = null;
		[SerializeField] private AnimatorParameter _horizontalOrientationParam = null;
		[SerializeField] private AnimatorParameter _horizontalSpeedParam = null;
		[SerializeField] private AnimatorParameter _verticalVelocityParam = null;

		public int IsGroundedParameter => _isGroundedParam.ParameterID;
		public int HorizontalOrientationParam => _horizontalOrientationParam.ParameterID;
		public int HorizontalSpeedParam => _horizontalSpeedParam.ParameterID;
		public int VerticalVelocityParam => _verticalVelocityParam.ParameterID;

		[Space, Header("Physics")]
		[SerializeField] private float _speed = 5f;
		[SerializeField] private LayerMask _groundLayerMask;
		[SerializeField] private float _isGroundedRaycastDistance = 0.25f;
		[SerializeField] private float _jumpTolerance = 0.2f;
		[SerializeField] private float _jumpForce = 10f;
		public float Speed => _speed;
		public int GroundLayerMask => _groundLayerMask;
		public float IsGroundedRaycastDistance => _isGroundedRaycastDistance;
		public float JumpTolerance => _jumpTolerance;
		public float JumpForce => _jumpForce;
	}
}