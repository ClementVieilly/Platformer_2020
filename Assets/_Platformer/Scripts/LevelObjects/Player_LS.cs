///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:38
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.ScriptableObjects;
using System;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects
{
	[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
	public class Player_LS : ALevelObject
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

		private bool jump;
		private float jumpElapsedTime = 0f;
		private float hangElapsedTime = 0f;
		private bool startHang = false;
		private bool hasHanged = false;
		private float gravity = 0f;
		private bool jumpButtonIsPressed = false;

		private Rigidbody2D rigidBody = null;
		private Animator animator = null;

		private Action DoAction = null;

		#region Life
		public int Life 
		{ 
			get {return _life; }
			set 
			{ 
				_life = value;
				CheckRestingLife();
			}
		}
		private int _life;
		public event Action OnDie;
		#endregion

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
			InitLife();
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

			if (jump && !jumpButtonIsPressed && _isGrounded)
			{
				SetModeAir();
				startHang = true;
				hasHanged = false;
				jumpElapsedTime = 0f;
				hangElapsedTime = 0f;
				jumpButtonIsPressed = true;
				rigidBody.velocity = new Vector2(rigidBody.velocity.x, settings.MinJumpForce);
			}
			else if (!jump)
				jumpButtonIsPressed = false;

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
			if (jump && jumpElapsedTime < settings.MaxJumpTime)
			{
				rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y + settings.JumpHoldForce);
				jumpElapsedTime += Time.fixedDeltaTime;
			}
			else if (!hasHanged && (!jump || jumpElapsedTime >= settings.MaxJumpTime))
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

			if (rigidBody.velocity.y <= -settings.FallVerticalSpeed)
				rigidBody.velocity = new Vector2(rigidBody.velocity.x, -settings.FallVerticalSpeed);
		}

		private void MoveHorizontalInAir()
		{
			float ratio;
			float horizontalMove;

			if (horizontalAxis != 0f)
			{
				ratio = settings.InAirAccelerationCurve.Evaluate(horizontalMoveElapsedTime);
				horizontalMove = Mathf.Lerp(0f, settings.FallHorizontalSpeed, ratio);
			}
			else
			{
				ratio = settings.InAirDecelerationCurve.Evaluate(horizontalMoveElapsedTime);
				horizontalMove = Mathf.Lerp(0f, topSpeed, ratio);
			}

			rigidBody.velocity = new Vector2(previousDirection * horizontalMove, rigidBody.velocity.y);
		}

        #region LifeMethods
        private void InitLife()
		{
			_life = settings.StartLife;
		}

		private bool CheckRestingLife()
		{
			if (Life == 0) Die();
			return Life > 0;
		}

		public void AddLife(int EarnedLife = 1)
		{
			Life += EarnedLife;
		}

		public bool LooseLife(int LoseLife = 1)
		{
			Life -= LoseLife;
			return CheckRestingLife();
		}

		public void Die()
		{
			OnDie?.Invoke();
		}

		public void setPosition(Vector2 position)
		{
			transform.position = position;
		}
		#endregion

	}
}