///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 22/01/2020 10:17
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.ScriptableObjects;
using System;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects {
	public class PlayerPlane : ALevelObject
	{
		[SerializeField] private PlayerController controller = null;
		[SerializeField] private PlayerSettings settings = null;

		private bool _isGrounded = true;
		public bool IsGrounded
		{
			get { return _isGrounded; }
			protected set
			{
				_isGrounded = value;
				/*animator.SetBool(settings.IsGroundedParameter, value);
				animator.SetFloat(settings.VerticalVelocityParam, 0f);*/
			}
		}

		#region horizontal move
		private float horizontalAxis = 0f;
		private float previousDirection = 0f;
		private float horizontalMoveElapsedTime = 0f;
		// Vitesse au moment de commencer la décélération
		private float topSpeed = 0f;
		#endregion

		private float jump = 0f;
		private float jumpElapsedTime = 0f;
		private float hangElapsedTime = 0f;
		private bool startHang = false;
		private bool hasHanged = false;
		private float gravity = 0f;
		private bool jumpButtonHasPressed = false;

		//Paramètres feature Planer
		private bool firstJumpPress = true;
		private bool planeStarted = false;
		private float planeElapsedTime;

		private Rigidbody2D rigidBody = null;
		private Animator animator = null;

		private Action DoAction = null;


		override public void Init()
		{
			throw new NotImplementedException();
		}

		private void Awake()
		{
			rigidBody = GetComponent<Rigidbody2D>();
			animator = GetComponent<Animator>();

			gravity = rigidBody.gravityScale;

			SetModeSpawn();
		}

		private void Start()
		{
			controller.Init();
		}

		private void Update()
		{
			CheckInputs();
		}

		private void CheckInputs()
		{
			horizontalMoveElapsedTime += Time.deltaTime;

			if (horizontalAxis != controller.HorizontalAxis)
			{
				horizontalMoveElapsedTime = 0f;
				if (controller.HorizontalAxis == 0f)
					topSpeed = Mathf.Abs(rigidBody.velocity.x);
			}

			if (horizontalAxis != 0f)
				previousDirection = horizontalAxis;

			horizontalAxis = controller.HorizontalAxis;
			jump = controller.Jump;
		}

		private void FixedUpdate()
		{
			DoAction();
		}

		private void SetModeNormal()
		{
			DoAction = DoActionNormal;
		}

		private void SetModeSpawn()
		{
			DoAction = DoActionSpawn;
		}

		private void SetModeAir()
		{
			DoAction = DoActionInAir;
		}

		private void DoActionNormal()
		{
			// Réflexion sur l'orientation des pentes
			/*if (_isGrounded)
			{
				Vector2 tan = hitInfos.normal;
				tan = new Vector2(tan.y, -tan.x);
				Debug.DrawRay(transform.position, tan * 2, Color.red);
			}*/

			MoveHorizontalOnGround();

			ComputeIsGrounded();

			if (jump != 0f && !jumpButtonHasPressed && _isGrounded)
			{
				SetModeAir();
				startHang = true;
				hasHanged = false;
				jumpElapsedTime = 0f;
				hangElapsedTime = 0f;
				jumpButtonHasPressed = true;
				rigidBody.velocity = new Vector2(rigidBody.velocity.x, settings.MinJumpForce);
			}
			else if (jump == 0f)
			{
				jumpButtonHasPressed = false;
				firstJumpPress = true;
			}
				

			if (!_isGrounded)
			{
				SetModeAir();
				hasHanged = true;
			}

			// Updating Animator
			/*animator.SetInteger(settings.HorizontalOrientationParam, rigidBody.velocity.x == 0 ? 0 : (int)Mathf.Sign(rigidBody.velocity.x));
			animator.SetFloat(settings.HorizontalSpeedParam, Mathf.Abs(rigidBody.velocity.x));

			if (!_isGrounded)
				animator.SetFloat(settings.VerticalVelocityParam, rigidBody.velocity.y);*/
		}

		private void ComputeIsGrounded()
		{
			Vector3 origin = rigidBody.position + Vector2.up * settings.IsGroundedRaycastDistance;

			Debug.DrawRay(origin, Vector2.down * (settings.IsGroundedRaycastDistance + settings.JumpTolerance), Color.blue);

			RaycastHit2D hitInfos = Physics2D.Raycast(origin, Vector2.down, settings.IsGroundedRaycastDistance + settings.JumpTolerance, settings.GroundLayerMask);
			IsGrounded = hitInfos.collider != null;
		}

		private void MoveHorizontalOnGround()
		{
			float ratio;
			float horizontalMove;

			if (horizontalAxis != 0f)
			{
				ratio = settings.RunAccelerationCurve.Evaluate(horizontalMoveElapsedTime);
				horizontalMove = Mathf.Lerp(0f, settings.RunSpeed, ratio);
			}
			else
			{
				ratio = settings.RunDecelerationCurve.Evaluate(horizontalMoveElapsedTime);
				horizontalMove = Mathf.Lerp(0f, topSpeed, ratio);
			}

			rigidBody.velocity = new Vector2(previousDirection * horizontalMove, rigidBody.velocity.y);
		}

		private void DoActionSpawn()
		{
			//if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Spawn"))
			SetModeNormal();
		}

		private void DoActionInAir()
		{
			ComputeIsGrounded();

			if (_isGrounded)
			{
				SetModeNormal();
				return;
			}

			MoveHorizontalInAir();

			// Gère l'appui long sur le jump
			if (jump != 0f && jumpElapsedTime < settings.MaxJumpTime)
			{
				rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y + settings.JumpHoldForce);
				jumpElapsedTime += Time.fixedDeltaTime;
			}
			else if (!hasHanged && (jump == 0f || jumpElapsedTime >= settings.MaxJumpTime))
			{
				jumpElapsedTime = settings.MaxJumpTime;
				startHang = true;
			}

			// Gère le hang time du jump
			if (!hasHanged && startHang && Mathf.Abs(rigidBody.velocity.y) <= settings.JumpHangThreshold)
			{
				hasHanged = true;
				rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);
				rigidBody.gravityScale = 0f;
			}

			if (hasHanged)
				hangElapsedTime += Time.fixedDeltaTime;

			if (hangElapsedTime >= settings.JumpHangTime)
				rigidBody.gravityScale = gravity;

			//Coder ici pour régler la feature planer
			if (jump == 0f) firstJumpPress = false;
			if (!firstJumpPress) planeStarted = jump != 0f ? true : false;

			float ratio;
			ratio = settings.PlaneAccelerationCurve.Evaluate(planeElapsedTime);
			float fallValue;

			if (planeStarted)
			{
				planeElapsedTime += Time.fixedDeltaTime;
				fallValue = -settings.PlaneVerticalSpeed;
				fallValue = -Mathf.Lerp(0f, settings.PlaneVerticalSpeed, ratio); ;
			}
			else
			{
				planeElapsedTime = 0;
				fallValue = -settings.FallVerticalSpeed;
			}

			//fallValue = planeStarted ? -settings.PlaneVerticalSpeed : -settings.FallVerticalSpeed;

			if (rigidBody.velocity.y <= fallValue)
				rigidBody.velocity = new Vector2(rigidBody.velocity.x, fallValue);

			//Chute standard
			//if (rigidBody.velocity.y <= -settings.FallVerticalSpeed)
			//	rigidBody.velocity = new Vector2(rigidBody.velocity.x, -settings.FallVerticalSpeed);
		}

		private void MoveHorizontalInAir()
		{
			float ratio;
			float horizontalMove;
			float horizontalSpeed;

			if (horizontalAxis != 0f)
			{
				horizontalSpeed = planeStarted ? settings.PlaneHorizontalSpeed : settings.FallHorizontalSpeed;

				ratio = settings.InAirAccelerationCurve.Evaluate(horizontalMoveElapsedTime);
				horizontalMove = Mathf.Lerp(0f, horizontalSpeed, ratio);
			}
			else
			{
				ratio = settings.InAirDecelerationCurve.Evaluate(horizontalMoveElapsedTime);
				horizontalMove = Mathf.Lerp(0f, topSpeed, ratio);
			}

			rigidBody.velocity = new Vector2(previousDirection * horizontalMove, rigidBody.velocity.y);
		}
	}
}