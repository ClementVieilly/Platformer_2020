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
		[SerializeField] private float _runSpeed = 5f;
		[SerializeField] private AnimationCurve _runAccelerationCurve = null;
		[SerializeField] private AnimationCurve _runDecelerationCurve = null;
		[SerializeField] private float _fallHorizontalSpeed = 5f;
		[SerializeField] private float _fallVerticalSpeed = 5f;
		[SerializeField] private AnimationCurve _inAirAccelerationCurve = null;
		[SerializeField] private AnimationCurve _inAirDecelerationCurve = null;
		[SerializeField] private LayerMask _groundLayerMask;
		[SerializeField] private float _isGroundedRaycastDistance = 0.25f;
		[SerializeField] private float _jumpTolerance = 0.2f;
		[SerializeField] private float _minJumpForce = 10f;
		[SerializeField] private float _jumpHoldForce = 1f;
		[SerializeField] private float _maxJumpTime = 0.5f;
		[SerializeField] private float _jumpHangThreshold = 0.5f;
		[SerializeField] private float _jumpHangTime = 0.5f;
		[SerializeField] private float _planeVerticalSpeed = 1f;
		[SerializeField] private float _planeHorizontalSpeed = 1f;

		public float RunSpeed => _runSpeed;
		public AnimationCurve RunAccelerationCurve => _runAccelerationCurve;
		public AnimationCurve RunDecelerationCurve => _runDecelerationCurve;
		public float FallHorizontalSpeed => _fallHorizontalSpeed;
		public float FallVerticalSpeed => _fallVerticalSpeed;
		public AnimationCurve InAirAccelerationCurve => _inAirAccelerationCurve;
		public AnimationCurve InAirDecelerationCurve => _inAirDecelerationCurve;
		public int GroundLayerMask => _groundLayerMask;
		public float IsGroundedRaycastDistance => _isGroundedRaycastDistance;
		public float JumpTolerance => _jumpTolerance;
		public float MinJumpForce => _minJumpForce;
		public float JumpHoldForce => _jumpHoldForce;
		public float MaxJumpTime => _maxJumpTime;
		public float JumpHangThreshold => _jumpHangThreshold;
		public float JumpHangTime => _jumpHangTime;
		public float PlaneVerticalSpeed => _planeVerticalSpeed;
		public float PlaneHorizontalSpeed => _planeHorizontalSpeed;
	}
}