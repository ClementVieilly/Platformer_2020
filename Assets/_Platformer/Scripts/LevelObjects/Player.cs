///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:38
///-----------------------------------------------------------------

using Cinemachine;
using Com.IsartDigital.Platformer.LevelObjects.Platforms;
using Com.IsartDigital.Platformer.Managers;
using Com.IsartDigital.Platformer.Screens;
using Com.IsartDigital.Platformer.ScriptableObjects;
using System;
using System.Collections;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class Player : ALevelObject
    {
        [SerializeField] private GameObject stateTag = null;

        [Header("Settings")]
		[SerializeField] private PlayerController controller = null;
        [SerializeField] private PlayerSettings settings = null;

        [Header("Linecasts and raycasts")]
		[SerializeField] private Transform wallLinecastRightStartPos = null; 
        [SerializeField] private Transform wallLinecastRightEndPos = null;
        [SerializeField] private Transform wallLinecastLeftStartPos = null;
        [SerializeField] private Transform wallLinecastLeftEndPos = null;
        [SerializeField] private Transform groundLinecastStartPos = null;
        [SerializeField] private Transform groundLinecastEndPos = null;
        [SerializeField] private Transform traversableRaycastOrigin = null;

        [Header("Particle Systems")]
        [SerializeField] private ParticleSystem walkingPS = null;
        [SerializeField] private ParticleSystem jumpingWingsPS = null;
        [SerializeField] private ParticleSystem jumpDustGroundPS = null;
        [SerializeField] private ParticleSystem jumpDustAirPS = null;
        [SerializeField] private ParticleSystem landingPS = null;
        [SerializeField] private ParticleSystem wallJumpPS = null;
		[SerializeField] private ParticleSystem onWallJumpPS = null;
        [SerializeField] private ParticleSystem planePS = null;
        [SerializeField] private ParticleSystem onWallPS = null;

		[Header("Debug")]
		[SerializeField] private bool drawGroundLinecast = true;
		[SerializeField] private bool drawGroundRaycast = true;
		[SerializeField] private bool drawTraversableRaycast = true;

		private RaycastHit2D hitInfos;
        private RaycastHit2D hitInfosNormal;

        #region Life
        private int _life;
        public int Life
		{
            get => _life;
            set => _life = value;
        }

        public event Action OnDie;
        #endregion
        private bool _isGrounded = true;
        public bool IsGrounded
        {
            get { return _isGrounded; }
            protected set
            {
                _isGrounded = value;
                animator.SetBool(settings.IsGroundedParameter, value);
				animator.SetFloat(settings.VerticalVelocityParam, 0f);
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

        //Pentes 
        private Vector2 penteVelocity;

        //Corner
        //private bool isOnCorner = false;
        //private bool wasInCorner = false;

        private float elapsedTimerBeforeSetModeAir = 0f;
        private bool canJump = false;

        //Pos au spawn et au respawn
        private Vector2 startPosition;
        private Vector2 lastCheckpointPos;
        public Vector2 LastCheckpointPos => lastCheckpointPos;

        //HorizontalMove
        private float horizontalAxis = 0f;
        private float previousDirection = 0f;
        private float horizontalMoveElapsedTime = 0f;

        // Vitesse au moment de commencer la décélération
        private float topSpeed = 0f;

        private bool _jump = false;
        private bool jump 
        {
            get { return _jump; }
            set
			{
				_jump = value;
				if (!_jump)
				{
					OnPlayerEndJump?.Invoke();
					OnPlayerEndPlane?.Invoke();
				}
			}
        }

        // ElapsedTime des différents états
        private float jumpElapsedTime = 0f;
        private float hangElapsedTime = 0f;
        private float wallJumpElaspedTime = 0f;
        
        //Jump
        private bool startHang = false;
        private bool hasHanged = false;
        private float gravity = 0f;
        private bool jumpButtonHasPressed = false;

        //Wall Jump 
        private bool wasOnWall = false;
        private float facingRightWall = 1;

        //Animations
        private Vector3 scaleLeft = new Vector3(0.5f, 0.5f, 1f);
        private Vector3 scaleRight = new Vector3(-0.5f, 0.5f, 1f);
        private float idleElapsedTime = 0;
        private float animCounter = 0;
        private bool isPlaying = false;
        private float timeBetweenIdleAndIdleLong = 5f;

        //Component
        private Rigidbody2D rigidBody = null;
        private Animator animator = null;

        // Properties for Pause
        private Action PreviousDoAction = null;
        private Vector2 lastVelocity = Vector2.zero;

        //Event for HUD controller update
        public delegate void PlayerMoveEventHandler(float horizontalAxis);
        public static event PlayerMoveEventHandler OnPlayerMove;
        public static Action OnPlayerJump;
        public static Action OnPlayerEndJump;
		public static Action OnPlayerPlane;
		public static Action OnPlayerEndPlane;

		//Cinemachine Virtual Camera
		[Header("Cinemachine")]
        [SerializeField] private CinemachineVirtualCamera vCam = null;
        public CinemachineVirtualCamera VCam => vCam;
        [SerializeField] private GameObject vCamIdle = null;
        private CinemachineFramingTransposer vCamBody = null;
        private float lastLookAheadTime = 0f;
        private float lastLookAheadSmoothing = 0f;

        //Lock player for cinematics
        private bool isLocked = false;
        private float lockTimer = 0;

        //Ps parameter
        private float jumpPSTimer = 0; 
        private float jumpPSDuration = 0.4f;

        private Action DoAction = null;

        private string platformDestructibleTag = "PlatformDestructible"; 

        override public void Init()
        {
			if (UIManager.Instance)
				Life = settings.StartLife;
			else
				Life = int.MaxValue;

			lastCheckpointPos = transform.position;
			startPosition = transform.position;
            vCamBody = vCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        public void Reset()
        {
            InitLife();
            gameObject.SetActive(true);
            GetComponent<Collider2D>().enabled = true;
            SetPosition(startPosition);
			lastCheckpointPos = transform.position;

            rigidBody.simulated = true;
            rigidBody.WakeUp();
            SetModeSpawn();
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
            Init();
        }

        private void Update()
        {
            CheckInputs();
        }

		private void CheckInputs()
        {
            if (isLocked) 
            {
                horizontalAxis = 0;
                return;
            }
            horizontalMoveElapsedTime += Time.deltaTime;

            if(horizontalAxis != controller.HorizontalAxis)
            {
                horizontalMoveElapsedTime = 0f;
                if(controller.HorizontalAxis == 0f)
                    topSpeed = Mathf.Abs(rigidBody.velocity.x);
            }

			if (horizontalAxis != 0 && !wasOnWall)
				previousDirection = horizontalAxis;

			horizontalAxis = controller.HorizontalAxis;
            OnPlayerMove?.Invoke(horizontalAxis);

            if (jump != controller.Jump) jump = controller.Jump;
        }

        private void FixedUpdate()
        {
            DoAction();
        }

        #region Player behavior
        private void SetModeNormal()
        {
            stateTag.name = "Normal"; 
            DoAction = DoActionNormal;
            landingPS.Play();
            animator.SetBool(settings.IsPlaningParam, false);
            planePS.Stop();
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
            animator.SetBool(settings.IsPlaningParam, false);
            planePS.Stop();
        }

        private void SetModePlane()
        {
			if (SoundManager.Instance)
				SoundManager.Instance.Play(sounds.Character_Planer,this);

			OnPlayerPlane?.Invoke();

            stateTag.name = "Plane"; 
            DoAction = DoActionPlane;
            animator.SetBool(settings.IsPlaningParam, true);
			wasOnWall = false;
            planePS.Play();
        }

        public void SetModePause()
        {
            PreviousDoAction = DoAction;
            lastVelocity = rigidBody.velocity;
            rigidBody.Sleep();
            rigidBody.simulated = false;
            DoAction = DoActionVoid; 
        }

        public void SetModeResume()
        {
            rigidBody.velocity = lastVelocity;
			rigidBody.simulated = true;
            rigidBody.WakeUp();
            DoAction = PreviousDoAction;
        }

        private void DoActionNormal()
        {
            CheckIsGrounded();
            canJump = true;
            // Réflexion sur l'orientation des pentes
            if(IsGrounded)
            {
                Vector2 tan = hitInfosNormal.normal;
                tan = new Vector2(tan.y, -tan.x);
                float anglePente = Vector2.Angle(tan, Vector3.up);
                if(anglePente > settings.AngleMinPente && anglePente < settings.AngleMaxPente /*|| anglePente > 150*/)
                {
                    penteVelocity = tan;
                    rigidBody.gravityScale = 0f;
                }
            }
            
            MoveHorizontalOnGround();

            //Détection du jump
            if (jump && !jumpButtonHasPressed && canJump)
            {
                rigidBody.gravityScale = gravity; 
                SetModeAir();
                startHang = true;
                hasHanged = false;
                jumpElapsedTime = 0f;
                hangElapsedTime = 0f;
                jumpButtonHasPressed = true;
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, settings.MinJumpForce);
                IsGrounded = false;
                jumpingWingsPS.Play();
				jumpDustGroundPS.Play();
				jumpDustAirPS.Play();
                StartCoroutine(StartJumpParticule());

				OnPlayerJump?.Invoke();

				if (SoundManager.Instance)
					SoundManager.Instance.Play(sounds.Character_Jump,this);
            }
            else if (!jump) jumpButtonHasPressed = false;

            if (!_isGrounded)
            {
                elapsedTimerBeforeSetModeAir += Time.deltaTime;

                if (elapsedTimerBeforeSetModeAir > settings.CoyoteTime)
                {
                    rigidBody.gravityScale = gravity;
                    SetModeAir();
                    hasHanged = true;
                    canJump = false;
                }
            }

            // Code pour que le Idle Long se lance toute les 5sec si le perso est déjà en Idle
            if (Math.Abs(rigidBody.velocity.x) < 0.1f)
            {
                float animDuration = 1.8f * 45;

                if (isPlaying && animCounter < animDuration) animCounter++;
                else if (animCounter >= animDuration)
                {
                    animCounter = 0;
                    isPlaying = false;
                }

                else idleElapsedTime += Time.deltaTime;
                if (idleElapsedTime >= timeBetweenIdleAndIdleLong)
                {
                    idleElapsedTime = 0;
                    animator.SetTrigger(settings.IdleLong);
                    SoundManager.Instance.Stop(sounds.Character_Idle, this);
                    SoundManager.Instance.Play(sounds.Character_IdleLong, this);
                    vCamIdle.SetActive(true);
                    isPlaying = true;
                }
            }
            else 
            {
                idleElapsedTime = 0;
                vCamIdle.SetActive(false);
            }

			// Updating Animator
			UpdateOrientation();
			animator.SetFloat(settings.HorizontalSpeedParam, Mathf.Abs(rigidBody.velocity.x));
        }

		private void UpdateOrientation()
		{
			if (previousDirection > 0)
				transform.localScale = scaleRight;
			else if (previousDirection < 0)
				transform.localScale = scaleLeft;
		}

		private void CheckIsGrounded()
		{
			Vector3 origin = rigidBody.position + Vector2.down * settings.IsGroundedRaycastDistance;

			//LineCast horizontal aux pieds
			hitInfos = Physics2D.Linecast(groundLinecastStartPos.position, groundLinecastEndPos.position, settings.GroundLayerMask);
			if (drawGroundLinecast) Debug.DrawLine(groundLinecastStartPos.position, groundLinecastEndPos.position, Color.red);

			hitInfosNormal = Physics2D.Raycast(origin, Vector2.down, settings.JumpTolerance, settings.GroundLayerMask);
			if (drawGroundRaycast) Debug.DrawRay(origin, Vector2.down - new Vector2(0, settings.JumpTolerance), Color.blue);

			bool isTraversable = false;
			RaycastHit2D traversableHitInfos = Physics2D.Raycast(traversableRaycastOrigin.position, Vector2.down, settings.TraversableRaycastDistance, settings.GroundLayerMask);
			if (drawTraversableRaycast) Debug.DrawRay(traversableRaycastOrigin.position, Vector2.down * settings.CanGroundOnTraversableDistance, Color.magenta);
			if (traversableHitInfos.collider)
			{
				if (traversableHitInfos.collider.GetComponent<PlatformEffector2D>() &&
					traversableHitInfos.collider.GetComponent<PlatformEffector2D>().useOneWay &&
					//(traversableHitInfos.distance < settings.CanGroundOnTraversableDistance ||
					/*Mathf.Abs(*/rigidBody.velocity.y/*)*/ > settings.ToPassTraversableVelocity)/*)*/
					isTraversable = true;
			}

			IsGrounded = (hitInfos.collider != null || hitInfosNormal.collider != null) && !isTraversable;
		}

		private void MoveHorizontalOnGround()
        {
            float ratio;
            float horizontalMove;

            if (isLocked) return;

            if (horizontalAxis != 0f)
            {
                ratio = settings.RunAccelerationCurve.Evaluate(horizontalMoveElapsedTime);
                horizontalMove = Mathf.Lerp(0f, settings.RunSpeed, ratio);
                walkingPS.Play();

				if (SoundManager.Instance)
                {
                    SoundManager.Instance.Stop(sounds.Character_Idle, this);
					SoundManager.Instance.Play(sounds.Character_Run,this);
                }
            }
            else
            {
                ratio = settings.RunDecelerationCurve.Evaluate(horizontalMoveElapsedTime);
                horizontalMove = Mathf.Lerp(0f, topSpeed, ratio);
                if (SoundManager.Instance)
                {
                    SoundManager.Instance.Stop(sounds.Character_Run, this);
                    SoundManager.Instance.Play(sounds.Character_Idle, this);
                }
            }

                rigidBody.velocity = penteVelocity.normalized * horizontalMove * previousDirection; 
        }

        private void DoActionSpawn()
        {
            //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Spawn"))
            SoundManager.Instance.Play(sounds.Character_Spawn, this);
            SetModeNormal();
        }

        private void DoActionInAir()
        {
            CheckIsOnWall();
            animator.SetBool(settings.IsOnWallParam, IsOnWall); 

            CheckIsGrounded(); 
            if (_isGrounded)
            {
				onWallPS.Stop();
				wasOnWall = false;
				wallJumpElaspedTime = 0; 
                SetModeNormal();

				if (SoundManager.Instance)
					SoundManager.Instance.Play(sounds.Character_Fall,this);

                return;
            }

            if (_isOnWall)
            {
				animator.SetBool(settings.IsOnWallParam, true);
				//transform.localScale = facingRightWall > 0 ? scaleRight : scaleLeft;

				if (jump && !jumpButtonHasPressed)
				{
                    jumpButtonHasPressed = true;
                    wasOnWall = true;
                    horizontalMoveElapsedTime = 0f;
					wallJumpElaspedTime = 0f;
                    previousDirection = -facingRightWall;
                    rigidBody.velocity = new Vector2(settings.WallJumpHorizontalForce * previousDirection, settings.WallJumpVerticalForce);
                    onWallPS.Stop();
					onWallJumpPS.transform.localScale = facingRightWall == 1 ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
                    wallJumpPS.Play();
					animator.SetTrigger(settings.JumpOnWall);
					animator.SetBool(settings.IsOnWallParam, false);

					if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
					{
						animator.Play("Fall");
						animator.Play("Jump");
					}

					OnPlayerJump?.Invoke();

					transform.localScale = facingRightWall < 0 ? scaleRight : scaleLeft;
				}
			}

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

            // Est en train de hang
            if (hasHanged)
                hangElapsedTime += Time.fixedDeltaTime;

            // fin du hang
            if (hangElapsedTime >= settings.JumpHangTime)
                rigidBody.gravityScale = gravity;

            // Gère le fait qu'on ait bien relaché le bouton de jump avant de pouvoir planer
            if (!jump) jumpButtonHasPressed = false;

            //Passe en mode planage
            if (jump && !jumpButtonHasPressed && !wasOnWall) SetModePlane();

            //Chute du Player
            if (_isOnWall && rigidBody.velocity.y <= -settings.FallOnWallVerticalSpeed)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, -settings.FallOnWallVerticalSpeed);
                if(!onWallPS.isPlaying) onWallPS.Play();
            }
            else if (rigidBody.velocity.y <= -settings.FallVerticalSpeed)
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, -settings.FallVerticalSpeed);

			if (!_isOnWall)
				UpdateOrientation();

            animator.SetFloat(settings.HorizontalSpeedParam, Mathf.Abs(rigidBody.velocity.x));

            animator.SetFloat(settings.VerticalVelocityParam, rigidBody.velocity.y);

            if (wallJumpElaspedTime >= settings.WallJumpTime)
            {
                wasOnWall = false;
                wallJumpElaspedTime = 0;
			}
            if (wasOnWall)
            {
                wallJumpElaspedTime += Time.deltaTime;
                return; 
            }
            else MoveHorizontalInAir(); 
        }

        private void DoActionPlane()
        {
            CheckIsOnWall();
            if (_isOnWall || !jump)
            {
				if (SoundManager.Instance)
                {
					SoundManager.Instance.Stop(sounds.Character_Planer,this);
                }
                SetModeAir();
                return;
            }

            CheckIsGrounded();
            if (_isGrounded)
            {
				if (SoundManager.Instance)
				{
					SoundManager.Instance.Stop(sounds.Character_Planer,this);
					SoundManager.Instance.Play(sounds.Character_Fall,this);
				}
                //planePS.Stop(); 
				SetModeNormal();
                return; 
            }

            if (!jumpButtonHasPressed) jumpButtonHasPressed = true; 

            MoveHorizontalPlane(); 

            //Planage vertical
            if (rigidBody.velocity.y <= settings.PlaneVerticalSpeed)
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, - settings.PlaneVerticalSpeed);

            transform.localScale = previousDirection >= 0 ? scaleRight : scaleLeft;
            animator.SetFloat(settings.HorizontalSpeedParam, Mathf.Abs(rigidBody.velocity.x));
            animator.SetFloat(settings.VerticalVelocityParam, rigidBody.velocity.y);

			if (SoundManager.Instance)
				SoundManager.Instance.Play(sounds.Character_Planer,this); 
        }

        private void CheckIsOnWall()
        {
            //LineCast verticaux pour tester la collision au mur
            RaycastHit2D hitInfosLeft = Physics2D.Linecast(wallLinecastLeftStartPos.position, wallLinecastLeftEndPos.position, settings.GroundLayerMask); 
            RaycastHit2D hitInfosRight = Physics2D.Linecast(wallLinecastRightStartPos.position, wallLinecastRightEndPos.position, settings.GroundLayerMask);
            Debug.DrawLine(wallLinecastRightStartPos.position, wallLinecastRightEndPos.position, Color.white);
            Debug.DrawLine(wallLinecastLeftStartPos.position, wallLinecastLeftEndPos.position, Color.black);

            if (hitInfosLeft.collider != null)
            {
				if (hitInfosLeft.collider.GetComponent<PlatformEffector2D>() &&
					hitInfosLeft.collider.GetComponent<PlatformEffector2D>().useOneWay)
					return;

				IsOnWall = true;
                facingRightWall = transform.localScale == scaleLeft ? -1 : 1;
            }
            else if (hitInfosRight.collider != null)
            {
				if (hitInfosRight.collider.GetComponent<PlatformEffector2D>() &&
					hitInfosRight.collider.GetComponent<PlatformEffector2D>().useOneWay)
					return;

				IsOnWall = true;
                facingRightWall = transform.localScale == scaleLeft ? 1 : -1;
            }
            else IsOnWall = false;

            if(IsOnWall)
            {
                Collider2D collider = hitInfosLeft.collider != null ? hitInfosLeft.collider : hitInfosRight.collider;
                if(collider.CompareTag(platformDestructibleTag)) collider.GetComponent<DestructiblePlatform>().SetModeNormal();
                SoundManager.Instance.Play(sounds.Character_WallCatch, this);
            }
            else onWallPS.Stop();

        }

        private void MoveHorizontalInAir()
        {
            SoundManager.Instance.Stop(sounds.Character_Run, this);
            if (isLocked) return;

            float horizontalMove;
            if(IsOnWall && horizontalAxis == facingRightWall ) return; 
            if (horizontalAxis != 0f) // On maintiens une direction lors de la chute
            {
                horizontalMove = Mathf.Lerp(rigidBody.velocity.x, settings.FallHorizontalSpeed * horizontalAxis, horizontalMoveElapsedTime);
            }
            else
            {
                float ratio = settings.InAirDecelerationCurve.Evaluate(horizontalMoveElapsedTime); 
                horizontalMove = Mathf.Lerp(0f, rigidBody.velocity.x,ratio);
            }
            rigidBody.velocity = new Vector2(horizontalMove, rigidBody.velocity.y);

        }

        private void MoveHorizontalPlane()
        {
            float ratio;
            float horizontalMove;

            if (isLocked) return;

            if (horizontalAxis != 0f)
            {
                ratio = settings.PlaneAccelerationCurve.Evaluate(horizontalMoveElapsedTime);
                horizontalMove = Mathf.Lerp(Mathf.Abs(rigidBody.velocity.x),  settings.PlaneHorizontalSpeed  , ratio);
                //wasOnWall = false;
            }
            else
            {
                ratio = settings.PlaneDecelerationCurve.Evaluate(horizontalMoveElapsedTime);
                horizontalMove = Mathf.Lerp(0f, rigidBody.velocity.x, ratio);
                wasOnWall = false;
            }

            rigidBody.velocity = new Vector2(previousDirection * horizontalMove, rigidBody.velocity.y);
        }


		private void SetModeVoid()
		{
			DoAction = DoActionVoid;
		}

        private void DoActionVoid() {}

        public IEnumerator Lock (float duration)
        {
            isLocked = true;

            while (lockTimer <= duration)
            {
                lockTimer += Time.deltaTime;

                rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
                if (rigidBody.velocity.y > 0) rigidBody.velocity = new Vector2(0, 0);

                yield return null;
            }
            lockTimer = 0;
            isLocked = false;
        }

        private IEnumerator StartJumpParticule()
        {
            while (jumpPSTimer <= jumpPSDuration)
            {
                jumpPSTimer += Time.deltaTime;
                yield return null; 
            }

            jumpingWingsPS.Stop();
            jumpPSTimer = 0; 
        }

        private void OnDestroy()
        {
            OnPlayerJump     = null;
            OnPlayerEndJump  = null;
            OnPlayerPlane    = null;
            OnPlayerEndPlane = null;
        }

        #endregion

        #region LifeMethods
        private void InitLife()
        {
            _life = settings.StartLife;
        }

        private bool CheckRestingLife()
        {
            return Life > 0;
        }

        public void AddLife(int EarnedLife = 1)
        {
            Life += EarnedLife;
        }

        public bool LooseLife(int LoseLife = 1)
        {
			GetComponent<Collider2D>().enabled = false; // Patch sur la mort du player si il traverse plusieurs kilZone en mourrant 

			animator.SetTrigger(settings.Die);

            if (SoundManager.Instance)
                SoundManager.Instance.Stop(sounds.Character_Planer, this);

			rigidBody.velocity = new Vector2(0f, rigidBody.velocity.y);

			SetModeVoid();
			Life -= LoseLife;

			if (Hud.Instance)
				Hud.Instance.Life = _life;

			return CheckRestingLife();
        }

        public void Die()
        {
            
            OnDie?.Invoke();
			SetModeNormal();
        }

		public void SetStartPosition(Vector2 position)
		{
			startPosition = position;
			SetPosition(startPosition);
		}

        public void SetPosition(Vector2 position)
        {
            if (isActiveAndEnabled) StartCoroutine(ReplacePlayer(position));
        }

        private IEnumerator ReplacePlayer(Vector2 position)
        {
            while(vCamBody == null) yield return null; 

            lastLookAheadTime = vCamBody.m_LookaheadTime;
            lastLookAheadSmoothing = vCamBody.m_LookaheadSmoothing;

            vCamBody.m_LookaheadTime = 0;
            vCamBody.m_LookaheadSmoothing = 0;
            transform.position = position;
            //while (transform.position.x != position.x && transform.position.y != position.y)
            //{
            //    Debug.Log("on replace player");
            //    if (!rigidBody.IsSleeping())rigidBody.Sleep();
            //    transform.position = position;
            //    yield return null;
            //}
            //rigidBody.WakeUp();

            while (vCamBody.m_LookaheadTime != lastLookAheadTime)
            {
                vCamBody.m_LookaheadTime = Mathf.MoveTowards(vCamBody.m_LookaheadTime, lastLookAheadTime, 0.1f);
                yield return null;
            }
            vCamBody.m_LookaheadSmoothing = lastLookAheadSmoothing;
            StopCoroutine("ReplacePlayer");
        }
        #endregion
    }
}