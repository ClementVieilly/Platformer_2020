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
	public class Player : ALevelObject
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

		private bool jumpButtonIsPressed = false;

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

			controller.Init();

			SetModeSpawn();
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

		private void DoActionNormal()
		{
			Vector3 origin = rigidBody.position + Vector2.up * settings.IsGroundedRaycastDistance;

			Debug.DrawRay(origin, Vector2.down * (settings.IsGroundedRaycastDistance + settings.JumpTolerance), Color.blue);

			RaycastHit2D hitInfos = Physics2D.Raycast(origin, Vector2.down, settings.IsGroundedRaycastDistance + settings.JumpTolerance, settings.GroundLayerMask);
			IsGrounded = hitInfos.collider != null;

			// Réflexion sur l'orientation des pentes
			/*if (_isGrounded)
			{
				Vector2 tan = hitInfos.normal;
				tan = new Vector2(tan.y, -tan.x);
				Debug.DrawRay(transform.position, tan * 2, Color.red);
			}*/

			rigidBody.velocity = new Vector2(controller.HorizontalAxis * settings.Speed, rigidBody.velocity.y);

			if (controller.Jump != 0f && !jumpButtonIsPressed && _isGrounded)
			{
				jumpButtonIsPressed = true;
				rigidBody.AddForce(Vector2.up * settings.JumpForce, ForceMode2D.Impulse);
			}
			else if (controller.Jump == 0f)
				jumpButtonIsPressed = false;

			/*animator.SetInteger(settings.HorizontalOrientationParam, rigidBody.velocity.x == 0 ? 0 : (int)Mathf.Sign(rigidBody.velocity.x));
			animator.SetFloat(settings.HorizontalSpeedParam, Mathf.Abs(rigidBody.velocity.x));

			if (!_isGrounded)
				animator.SetFloat(settings.VerticalVelocityParam, rigidBody.velocity.y);*/
		}

		private void DoActionSpawn()
		{
			//if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Spawn"))
				SetModeNormal();
		}
	}
}