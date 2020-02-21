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
		[SerializeField] private AnimatorParameter _horizontalSpeedParam = null;
		[SerializeField] private AnimatorParameter _verticalVelocityParam = null;
		[SerializeField] private AnimatorParameter _isPlaningParam = null;
		[SerializeField] private AnimatorParameter _isOnWallParam = null;
		[SerializeField] private AnimatorParameter _idleLong = null;
		[SerializeField] private AnimatorParameter _die = null;

		public int IsGroundedParameter => _isGroundedParam.ParameterID;
		public int HorizontalSpeedParam => _horizontalSpeedParam.ParameterID;
		public int VerticalVelocityParam => _verticalVelocityParam.ParameterID;
		public int IsPlaningParam => _isPlaningParam.ParameterID;
		public int IsOnWallParam => _isOnWallParam.ParameterID;
		public int IdleLong => _idleLong.ParameterID;
		public int Die => _die.ParameterID;

		[Space, Header("Physics")]
		[SerializeField] private float _runSpeed = 5f;
		[SerializeField] private AnimationCurve _runAccelerationCurve = null;
		[SerializeField] private AnimationCurve _runDecelerationCurve = null;
		[SerializeField] private float _fallHorizontalSpeed = 5f;
		[SerializeField] private float _fallVerticalSpeed = 5f;
		[SerializeField] private float _fallOnWallVerticalSpeed = 10f;
		[SerializeField] private AnimationCurve _inAirAccelerationCurve = null;
		[SerializeField] private AnimationCurve _inAirDecelerationCurve = null;
        [SerializeField] private float _planeVerticalSpeed = 1f;
        [SerializeField] private float _planeHorizontalSpeed = 1f;
        [SerializeField] private AnimationCurve _planeAccelerationCurve = null;
		[SerializeField] private AnimationCurve _planeDecelerationCurve = null;
		[SerializeField] private float _angleMinPente = 70f;
		[SerializeField] private float _angleMaxPente = 110f;


        [SerializeField] private LayerMask _groundLayerMask = 0;
		[SerializeField] private float _isGroundedRaycastDistance = 0.25f;
		//[SerializeField] private float _isGroundedLineCastDistance = 0.25f;
		//[SerializeField] private float _isOnWallRaycastDistance = 0.40f;
		//[SerializeField] private float _isOnWallLineCastDistance = 0.40f;
		[SerializeField] private float _jumpTolerance = 0.2f;
		[SerializeField] private float _minJumpForce = 10f;
		[SerializeField] private float _wallJumpHorizontalForce = 30f;
		[SerializeField] private float _wallJumpVerticalForce = 30f;
		[SerializeField] private float _jumpHoldForce = 1f;
		[SerializeField] private float _maxJumpTime = 0.5f;
		[SerializeField] private float _jumpHangThreshold = 0.5f;
		[SerializeField] private float _jumpHangTime = 0.5f;
		[SerializeField] private float _delayWallJump = 0.5f;
		[SerializeField] private int _startLife = 3;
        [SerializeField] private float _coyoteTime = 0f;
        //[SerializeField] private float _halfPlayerHeight = 0.5f;
        //[SerializeField] private float _linecastCornerPosY = 2;
        [SerializeField] private Vector2 _impulsionInCorner = Vector2.zero;

        public float RunSpeed => _runSpeed;
        public float AngleMinPente => _angleMinPente;
        public float AngleMaxPente => _angleMaxPente;
		public AnimationCurve RunAccelerationCurve => _runAccelerationCurve;
		public AnimationCurve RunDecelerationCurve => _runDecelerationCurve;
		public float FallHorizontalSpeed => _fallHorizontalSpeed;
		public float FallVerticalSpeed => _fallVerticalSpeed;
		public float FallOnWallVerticalSpeed => _fallOnWallVerticalSpeed;
		public AnimationCurve InAirAccelerationCurve => _inAirAccelerationCurve;
		public AnimationCurve InAirDecelerationCurve => _inAirDecelerationCurve;
        public float PlaneVerticalSpeed => _planeVerticalSpeed;
        public float PlaneHorizontalSpeed => _planeHorizontalSpeed;
        public AnimationCurve PlaneAccelerationCurve => _planeAccelerationCurve;
		public AnimationCurve PlaneDecelerationCurve => _planeDecelerationCurve;
        public int GroundLayerMask => _groundLayerMask;
		public float IsGroundedRaycastDistance => _isGroundedRaycastDistance;
		//public float IsGroundedLineCastDistance => _isGroundedLineCastDistance;
		//public float IsOnWallRayCastDistance => _isOnWallRaycastDistance;
		//public float IsOnWallLineCastDistance => _isOnWallLineCastDistance;
		public float JumpTolerance => _jumpTolerance;
		public float MinJumpForce => _minJumpForce;
		public float WallJumpHorizontalForce => _wallJumpHorizontalForce;
		public float WallJumpVerticalForce => _wallJumpVerticalForce;
		public float JumpHoldForce => _jumpHoldForce;
		public float MaxJumpTime => _maxJumpTime;
		public float JumpHangThreshold => _jumpHangThreshold;
		public float JumpHangTime => _jumpHangTime;
		public float DelayWallJump => _delayWallJump;
		public int StartLife => _startLife;
		public float CoyoteTime => _coyoteTime;
		//public float HalfPlayerHeight => _halfPlayerHeight;
		//public float LinecastCornerPosY => _linecastCornerPosY;
		public Vector2 ImpulsionInCorner => _impulsionInCorner;
    }
}