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

        [SerializeField] private GameObject stateTag = null; 
        
        public event Action<Player> OnDie; 
        private bool _isGrounded = true;
        private bool _isOnWall = true;
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

        public bool IsOnWall
        {
            get { return _isOnWall; }
            protected set
            {
                _isOnWall = value;
                /*animator.SetBool(settings.IsOnWallParameter, value); ? 
				animator.SetFloat(settings., 0f);  ? */
            }
        }
        private Vector2 _lastCheckpointPos;

        public Vector2 LastCheckpointPos { get => _lastCheckpointPos; set => _lastCheckpointPos = value; }

        private float horizontalAxis = 0f;

        private float previousDirection = 0f;
        private float horizontalMoveElapsedTime = 0f;
        // Vitesse au moment de commencer la décélération
        private float topSpeed = 0f;
        
        private float facingRightWall = 1; 
        private bool firstJumpPress = true;
        private bool planeStarted = false;
        private float planeElapsedTime; //  ?? 
        private float timerFallToPlane;
        
        private bool jump = false;

        // ElapsedTime des différents états
        private float jumpElapsedTime = 0f;
        private float hangElapsedTime = 0f;
        private float wallJumpElaspedTime = 0f;

        private bool startHang = false;
        private bool hasHanged = false;
        private float gravity = 0f;
        private bool jumpButtonHasPressed = false;
        //Wall Jump 
        private bool wasOnWall = false;
        private float delayWallJump = 0.5f;


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
            
             
            _lastCheckpointPos = transform.position; 
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

            if(horizontalAxis != controller.HorizontalAxis)
            {
                horizontalMoveElapsedTime = 0f;
                if(controller.HorizontalAxis == 0f)
                    topSpeed = Mathf.Abs(rigidBody.velocity.x);
            }

            if(horizontalAxis != 0f)
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
            stateTag.name = "Normal"; 
            DoAction = DoActionNormal;
        }

        private void SetModeSpawn()
        {
            DoAction = DoActionSpawn;
        }

        private void SetModeAir()
        {
            stateTag.name = "Air"; 
            DoAction = DoActionInAir;
        }

        private void SetModePlane()
        {
            stateTag.name = "Plane"; 
            DoAction = DoActionPlane; 
        }

        /* private void SetModeWallJump() 
         {
             DoAction = DoActionWallJump; 
         }*/



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

            CheckIsGrounded();

            //Détéection du jump
            if(jump && !jumpButtonHasPressed && _isGrounded)
            {
                SetModeAir();
                startHang = true;
                hasHanged = false;
                jumpElapsedTime = 0f;
                hangElapsedTime = 0f;
                jumpButtonHasPressed = true;
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, settings.MinJumpForce);
            }
            else if(!jump) jumpButtonHasPressed = false;
           
            if(!_isGrounded)
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

        private void CheckIsGrounded()
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

            if(horizontalAxis != 0f)
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
            CheckIsGrounded();

            if(_isGrounded)
            {
                SetModeNormal();
                return;
            }

            MoveHorizontalInAir();

            CheckIsOnWall();
            if(_isOnWall)
            {
                if(jump && !jumpButtonHasPressed)
                {
                    jumpButtonHasPressed = true;
                    wasOnWall = true;
                    horizontalMoveElapsedTime = 0f;
                    topSpeed = 20;
                    previousDirection = -facingRightWall;
                    rigidBody.velocity = new Vector2(20 * previousDirection, settings.MinJumpForce);
                }
            }

            // Gère l'appui long sur le jump
            if(jump && jumpElapsedTime < settings.MaxJumpTime)
            {

                rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y + settings.JumpHoldForce);
                jumpElapsedTime += Time.fixedDeltaTime;
            }
            else if(!hasHanged && (!jump || jumpElapsedTime >= settings.MaxJumpTime))
            {
                jumpElapsedTime = settings.MaxJumpTime;
                startHang = true;
            }

            // Gère le hang time du jump
            if(!hasHanged && startHang && Mathf.Abs(rigidBody.velocity.y) <= settings.JumpHangThreshold)
            {
                hasHanged = true;
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);
                rigidBody.gravityScale = 0f;
            }

            // Est en train de hang
            if(hasHanged)
                hangElapsedTime += Time.fixedDeltaTime;

            // fin du hang
            if(hangElapsedTime >= settings.JumpHangTime)
                rigidBody.gravityScale = gravity;

            // Gère le fait qu'on ait bien relaché le bouton de jump avant de pouvoir planer
            if(!jump) jumpButtonHasPressed = false;

            //Passe en mode planage
            if(jump && !jumpButtonHasPressed) SetModePlane();  

            //Chute du Player
            if(rigidBody.velocity.y <= - settings.FallVerticalSpeed)
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, - settings.FallVerticalSpeed);
        }

        private void DoActionPlane()
        {
            if(!jumpButtonHasPressed) jumpButtonHasPressed = true; 

            MoveHorizontalPlane(); 

            //Planage vertical
            if(rigidBody.velocity.y <= settings.PlaneVerticalSpeed)
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, - settings.PlaneVerticalSpeed);

            if(!jump) SetModeAir();
        }

        private void CheckIsOnWall()
        {
            Vector3 originRight = rigidBody.position + Vector2.right * settings.IsGroundedRaycastDistance;
            Vector3 originLeft = rigidBody.position + Vector2.left * settings.IsGroundedRaycastDistance;

            Debug.DrawRay(originRight, Vector2.right * (settings.IsOnWallRayCastDistance + settings.JumpTolerance), Color.red);
            Debug.DrawRay(originLeft, Vector2.left * (settings.IsOnWallRayCastDistance + settings.JumpTolerance), Color.yellow);

            RaycastHit2D hitInfosRightRaycast = Physics2D.Raycast(originRight, Vector2.right, settings.IsOnWallRayCastDistance + settings.JumpTolerance, settings.GroundLayerMask);
            RaycastHit2D hitInfosLeftRaycast = Physics2D.Raycast(originLeft, Vector2.left, settings.IsOnWallRayCastDistance + settings.JumpTolerance, settings.GroundLayerMask);

            if(hitInfosLeftRaycast.collider != null)
            {
                IsOnWall = true;
                facingRightWall = -1;
            }
            else if(hitInfosRightRaycast.collider != null)
            {
                IsOnWall = true;
                facingRightWall = 1; 
            }
            else IsOnWall = false;
        }

        private void MoveHorizontalInAir()
        {
            float ratio;
            float horizontalMove;
            float horizontalSpeed;

            if (horizontalAxis != 0 && wasOnWall && horizontalAxis != facingRightWall)
            {
                // Super wall jump
                horizontalSpeed = settings.FallHorizontalSpeed; 
                ratio = settings.InAirDecelerationCurve.Evaluate(horizontalMoveElapsedTime);
                horizontalMove = Mathf.Lerp(horizontalSpeed, topSpeed , ratio);

                if(ratio == 0) wasOnWall = false; 
            }
            else if (horizontalAxis != 0 && wasOnWall)
            {
                wallJumpElaspedTime += Time.fixedDeltaTime;

                horizontalSpeed = settings.FallHorizontalSpeed;
                ratio = settings.InAirDecelerationCurve.Evaluate(horizontalMoveElapsedTime);

                previousDirection = -facingRightWall;

                horizontalMove = horizontalSpeed;

                if(wallJumpElaspedTime >= delayWallJump)
                {
                    wallJumpElaspedTime = 0;
                    topSpeed = 0;
                }

                if(ratio == 0) wasOnWall = false;
            }
            else if (horizontalAxis != 0f)
            {
                ratio = settings.InAirAccelerationCurve.Evaluate(horizontalMoveElapsedTime);
                horizontalMove = Mathf.Lerp(horizontalAxis == facingRightWall ? 0f : topSpeed /*super wall jump 2*/, settings.FallHorizontalSpeed, ratio);

                wasOnWall = false;
            }
            else
            {
                ratio = settings.InAirDecelerationCurve.Evaluate(horizontalMoveElapsedTime);
                horizontalMove = Mathf.Lerp(0f, topSpeed, ratio);

                wasOnWall = false;
            }

            rigidBody.velocity = new Vector2(previousDirection * horizontalMove, rigidBody.velocity.y);
        }

        private void MoveHorizontalPlane()
        {
            float ratio;
            float horizontalMove;
            float horizontalSpeed;

            if(horizontalAxis != 0f)
            {
                horizontalSpeed = settings.PlaneHorizontalSpeed; 
                ratio = settings.InAirAccelerationCurve.Evaluate(horizontalMoveElapsedTime);

                horizontalMove = Mathf.Lerp(horizontalAxis == facingRightWall ? 0f : topSpeed /*super wall jump 2*/, horizontalSpeed, ratio);

                wasOnWall = false;
            }
            else
            {
                ratio = settings.InAirDecelerationCurve.Evaluate(horizontalMoveElapsedTime);
                horizontalMove = Mathf.Lerp(0f, topSpeed, ratio);

                wasOnWall = false;
            }

            rigidBody.velocity = new Vector2(previousDirection * horizontalMove, rigidBody.velocity.y);
        }

        //public void Die()
        //{
        //    Debug.Log("Je viens de prendre un café");
        //    OnDie?.Invoke(this); 
        //    transform.position = _lastCheckpointPos;
        //    //Pour l'instant Player s'occupe de se faire respawn lui même 
        //    //Faire en sorte que le LM s'abonne a l'event onDie du player et respawn a la position recup sur le chekcpoint
        //}
    }
}