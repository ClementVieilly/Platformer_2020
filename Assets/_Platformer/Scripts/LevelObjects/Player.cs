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

        [Header("Particle Systems")]
        [SerializeField] private ParticleSystem runningPS;
        [SerializeField] private ParticleSystem jumpingPS;
        [SerializeField] private ParticleSystem landingPS;
        [SerializeField] private ParticleSystem wallJumpPS;

        private RaycastHit2D hitInfos; 
        private RaycastHit2D hitInfosNormal; 

        #region Life
        public int Life
        {
            get { return _life; }
            set
            {
                _life = value;
                CheckRestingLife();
            }
        }
        private int _life;
        public event Action OnDie;
        #endregion
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

        private bool _isOnWall = true;
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

        private bool isSlinding = false;
        private Vector2 penteVelocity;

        private bool isOnCorner = false;

        private float elapsedTimerBeforeSetModeAir = 0f;
        private bool canJump = false;

        private Vector2 _lastCheckpointPos;

        public Vector2 LastCheckpointPos { get => _lastCheckpointPos; set => _lastCheckpointPos = value; }

        private float horizontalAxis = 0f;

        private float previousDirection = 0f;
        private float horizontalMoveElapsedTime = 0f;
        // Vitesse au moment de commencer la décélération
        private float topSpeed = 0f;
        // Vitesse au moment de la décélération après un wall jump
        private float topSpeedWallJump = 0f;

		private float facingRightWall = 1;

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
		private bool isFacingWallJump = false;

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

            landingPS.Play();

            DoAction = DoActionNormal;
        }

        private void SetModeSpawn()
        {
            DoAction = DoActionSpawn;
        }

        private void SetModeAir()
        {
            elapsedTimerBeforeSetModeAir = 0;
            stateTag.name = "Air"; 
            DoAction = DoActionInAir;
        }

        private void SetModePlane()
        {
            stateTag.name = "Plane"; 
            DoAction = DoActionPlane; 
        }

        private void DoActionNormal()
        {
            // Réflexion sur l'orientation des pentes
            if (IsGrounded && hitInfos.collider)
            {
                                Vector2 tan = hitInfosNormal.normal;
                tan = new Vector2(tan.y, -tan.x);
                penteVelocity = tan;
                float angle = Vector2.Angle(tan, Vector3.up);
                canJump = true;

                if (angle > settings.AngleMinPente && angle < settings.AngleMaxPente)
                {
                    rigidBody.gravityScale = 0f;
                    isSlinding = false;
                }
                else
                {
                    isSlinding = true; 
                    rigidBody.gravityScale = gravity;
                }
            }

            MoveHorizontalOnGround();

            CheckIsGrounded();

            //Détéection du jump
            if(jump && !jumpButtonHasPressed && canJump)
            {
                rigidBody.gravityScale = gravity; 
                SetModeAir();
                startHang = true;
                hasHanged = false;
                jumpElapsedTime = 0f;
                hangElapsedTime = 0f;
                jumpButtonHasPressed = true;
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, settings.MinJumpForce);

                jumpingPS.Play();
            }
            else if(!jump) jumpButtonHasPressed = false;

            if(!_isGrounded)
            {
                elapsedTimerBeforeSetModeAir += Time.deltaTime;

                if(elapsedTimerBeforeSetModeAir > settings.CoyoteTime)
                {
                    rigidBody.gravityScale = gravity;
                    SetModeAir();
                    hasHanged = true;
                    canJump = false;
                }

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

            Vector2 lineCastStart = new Vector2(rigidBody.position.x - settings.IsGroundedLineCastDistance, rigidBody.position.y - settings.JumpTolerance); 
            Vector2 lineCastEnd = new Vector2(rigidBody.position.x + settings.IsGroundedLineCastDistance, rigidBody.position.y - settings.JumpTolerance); 

            //RayCast vertical pour recup sa normal pour calculer les pentes
            hitInfosNormal = Physics2D.Raycast(origin, Vector2.down, settings.IsGroundedRaycastDistance + settings.JumpTolerance, settings.GroundLayerMask);
            Debug.DrawRay(origin, Vector2.down - new Vector2(0, settings.IsGroundedRaycastDistance + settings.JumpTolerance), Color.blue);

            //LineCast horizontal aux pieds
            hitInfos = Physics2D.Linecast(lineCastStart,lineCastEnd,settings.GroundLayerMask);
            Debug.DrawLine(lineCastStart, lineCastEnd, Color.red);

            IsGrounded = hitInfos.collider != null;
        }

        private void MoveHorizontalOnGround()
        {
            float ratio;
            float horizontalMove;

            if(horizontalAxis != 0f)
            {
                runningPS.Play();

                ratio = settings.RunAccelerationCurve.Evaluate(horizontalMoveElapsedTime);
                horizontalMove = Mathf.Lerp(0f, settings.RunSpeed, ratio);
            }
            else
            {
                ratio = settings.RunDecelerationCurve.Evaluate(horizontalMoveElapsedTime);
                horizontalMove = Mathf.Lerp(0f, topSpeed, ratio);
            }
            if(!isSlinding)
            {
                rigidBody.velocity = penteVelocity.normalized * horizontalMove * previousDirection; 
            }
            else rigidBody.velocity = new Vector2(previousDirection * horizontalMove, rigidBody.velocity.y);
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
                    wallJumpPS.Play();

                    jumpButtonHasPressed = true;
                    wasOnWall = true;
                    horizontalMoveElapsedTime = 0f;
					isFacingWallJump = false;
                    topSpeed = settings.WallJumpHorizontalForce;
					previousDirection = -facingRightWall;
                    rigidBody.velocity = new Vector2(settings.WallJumpHorizontalForce * previousDirection, settings.MinJumpForce);
                }
            }

            //Gère le cas ou le joueur est sur un coin de plateforme et lui donne un impulsion pour qu'il soit sur la plateforme
            if(isOnCorner)
            {
                rigidBody.velocity += new Vector2(settings.ImpulsionInCorner.x * previousDirection, settings.ImpulsionInCorner.y);
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
            if(_isOnWall && rigidBody.velocity.y <= -settings.FallOnWallVerticalSpeed)
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, -settings.FallOnWallVerticalSpeed);
            else if(rigidBody.velocity.y <= - settings.FallVerticalSpeed)
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, - settings.FallVerticalSpeed);
        }

        private void DoActionPlane()
        {
            CheckIsOnWall();
            if(_isOnWall || !jump)
            {
                SetModeAir();
                return;
            }

            CheckIsGrounded();
            if(_isGrounded)
            {
                landingPS.Play();

                SetModeNormal();
                return; 
            }

            if(!jumpButtonHasPressed) jumpButtonHasPressed = true; 

            MoveHorizontalPlane(); 

            //Planage vertical
            if(rigidBody.velocity.y <= settings.PlaneVerticalSpeed)
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, - settings.PlaneVerticalSpeed);
        }

        private void CheckIsOnWall()
        {
            //Points de départ et d'arrivée des LinesCast 
            Vector2 leftStart = new Vector2(rigidBody.position.x - settings.IsOnWallLineCastDistance, rigidBody.position.y + settings.HalfPlayerHeight); 
            Vector2 leftEnd = new Vector2(rigidBody.position.x - settings.IsOnWallLineCastDistance, rigidBody.position.y - settings.HalfPlayerHeight);

            Vector2 rightStart = new Vector2(rigidBody.position.x + settings.IsOnWallLineCastDistance, rigidBody.position.y + settings.HalfPlayerHeight);
            Vector2 rightEnd = new Vector2(rigidBody.position.x + settings.IsOnWallLineCastDistance, rigidBody.position.y - settings.HalfPlayerHeight);

            Vector2 leftStartCorner = new Vector2(rigidBody.position.x - settings.IsOnWallLineCastDistance, rigidBody.position.y + settings.LinecastCornerPosY); 
            Vector2 leftEndCorner = new Vector2(rigidBody.position.x - settings.IsOnWallLineCastDistance, rigidBody.position.y + settings.LinecastCornerPosY/2);

            Vector2 rightStartCorner = new Vector2(rigidBody.position.x + settings.IsOnWallLineCastDistance, rigidBody.position.y + settings.LinecastCornerPosY );
            Vector2 rightEndCorner = new Vector2(rigidBody.position.x + settings.IsOnWallLineCastDistance, rigidBody.position.y + settings.LinecastCornerPosY /2);

            Debug.DrawLine(rightStart, rightEnd, Color.yellow);
            Debug.DrawLine(leftStart, leftEnd, Color.red);
            Debug.DrawLine(rightStartCorner, rightEndCorner, Color.white);
            Debug.DrawLine(leftStartCorner, leftEndCorner, Color.black);

            //LineCast verticaux pour tester la collision au mur
            RaycastHit2D hitInfosLeft = Physics2D.Linecast(leftEnd, leftStart, settings.GroundLayerMask); 
            RaycastHit2D hitInfosRight = Physics2D.Linecast(rightStart, rightEnd, settings.GroundLayerMask); 
            RaycastHit2D hitInfosCornerRight = Physics2D.Linecast(rightStartCorner, rightEndCorner, settings.GroundLayerMask); 
            RaycastHit2D hitInfosCornerLeft = Physics2D.Linecast(leftStartCorner, leftEndCorner, settings.GroundLayerMask); 

            if(hitInfosLeft.collider != null)
            {
                IsOnWall = true;
                facingRightWall = -1;
            }
            else if(hitInfosRight.collider != null)
            {
                IsOnWall = true;
                facingRightWall = 1; 
            }
            else IsOnWall = false;

            if(hitInfosRight.collider && !hitInfosCornerRight.collider) isOnCorner = true; 
            else if(hitInfosLeft.collider && !hitInfosCornerLeft.collider) isOnCorner = true;
            else isOnCorner = false; 
            
        }

        private void MoveHorizontalInAir()
        {
            float ratio;
            float horizontalMove;

            if (horizontalAxis != 0 && wasOnWall && horizontalAxis != facingRightWall) // On fait le wallJump en maintenant la direction opposée au mur
			{
                ratio = settings.InAirDecelerationCurve.Evaluate(horizontalMoveElapsedTime);
                horizontalMove = Mathf.Lerp(settings.FallHorizontalSpeed, settings.WallJumpHorizontalForce, ratio);

                if(ratio == 0) wasOnWall = false; 
            }
            else if (horizontalAxis != 0 && wasOnWall) // On fait le wallJump en maintenant la direction vers le mur
            {
				if (!isFacingWallJump) isFacingWallJump = true;
				
                wallJumpElaspedTime += Time.fixedDeltaTime;

				ratio = settings.InAirDecelerationCurve.Evaluate(horizontalMoveElapsedTime);
				horizontalMove = Mathf.Lerp(0f, topSpeed, ratio);

				previousDirection = -facingRightWall;

				if (wallJumpElaspedTime >= settings.DelayWallJump)
                {
					isFacingWallJump = false;
					topSpeedWallJump = 0f;
                    topSpeed = 0f;
					wasOnWall = false;
                    wallJumpElaspedTime = 0;
					horizontalMoveElapsedTime = 0f;
				}
            }
            else if (horizontalAxis != 0f) // On maintiens une direction lors de la chute
            {
                ratio = settings.InAirAccelerationCurve.Evaluate(horizontalMoveElapsedTime);
                horizontalMove = Mathf.Lerp(horizontalAxis == facingRightWall ? 0f : topSpeedWallJump, settings.FallHorizontalSpeed, ratio);

                wasOnWall = false;
            }
			else if (isFacingWallJump) // On relache la direction après un wallJump en maintenant la direction vers le mur
			{
                wallJumpElaspedTime += Time.fixedDeltaTime;

				ratio = settings.InAirDecelerationCurve.Evaluate(horizontalMoveElapsedTime);
				horizontalMove = Mathf.Lerp(0f, topSpeed, ratio);

				previousDirection = -facingRightWall;

				topSpeedWallJump = horizontalMove;

				if (wallJumpElaspedTime >= settings.DelayWallJump)
				{
					isFacingWallJump = false;
					topSpeed = 0f;
					wasOnWall = false;
					wallJumpElaspedTime = 0;
					horizontalMoveElapsedTime = 0f;
				}

				wasOnWall = false;
			}
			else // On ne touche pas la direction
            {
                ratio = settings.InAirDecelerationCurve.Evaluate(horizontalMoveElapsedTime);
                horizontalMove = Mathf.Lerp(0f, topSpeed, ratio);

				topSpeedWallJump = horizontalMove;

				if (wasOnWall)
				{
					topSpeed = Math.Abs(rigidBody.velocity.x);
					wasOnWall = false;
				}
            }

			rigidBody.velocity = new Vector2(previousDirection * horizontalMove, rigidBody.velocity.y);
        }

        private void MoveHorizontalPlane()
        {
            float ratio;
            float horizontalMove;

            if(horizontalAxis != 0f)
            {
                ratio = settings.PlaneAccelerationCurve.Evaluate(horizontalMoveElapsedTime);
                horizontalMove = Mathf.Lerp(topSpeed, settings.PlaneHorizontalSpeed, ratio);
                wasOnWall = false;
            }
            else
            {
                ratio = settings.PlaneDecelerationCurve.Evaluate(horizontalMoveElapsedTime);
                horizontalMove = Mathf.Lerp(0f, topSpeed, ratio);

                wasOnWall = false;
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
            if(Life == 0) Die();
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