///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:38
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.LevelObjects.InteractiveObstacles;
using Com.IsartDigital.Platformer.Managers;
using Com.IsartDigital.Platformer.ScriptableObjects;
using System;
using System.Collections;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class Player : ALevelObject
    {
        [SerializeField] private PlayerController controller = null;
        [SerializeField] private PlayerSettings settings = null;
        [SerializeField] private SoundsSettings sounds = null;

        //Pos des Linecast !
        [SerializeField] private Transform wallLinecastRightStartPos = null; 
        [SerializeField] private Transform wallLinecastRightEndPos = null;
        [SerializeField] private Transform wallLinecastLeftStartPos = null;
        [SerializeField] private Transform wallLinecastLeftEndPos = null;
        [SerializeField] private Transform cornerLinecastRightStartPos = null;
        [SerializeField] private Transform cornerLinecastRightEndPos = null;
        [SerializeField] private Transform cornerLinecastLeftStartPos = null;
        [SerializeField] private Transform cornerLinecastLeftEndPos = null;
        [SerializeField] private Transform groundLinecastStartPos = null;
        [SerializeField] private Transform groundLinecastEndPos = null;

        [Header("Particle Systems")]
        [SerializeField] private ParticleSystem walkingPS;
        [SerializeField] private ParticleSystem jumpingPS;
        [SerializeField] private ParticleSystem landingPS;
        [SerializeField] private ParticleSystem wallJumpPSRight;
        [SerializeField] private ParticleSystem wallJumpPSLeft;
        [SerializeField] private ParticleSystem planePS;


        [SerializeField] private GameObject stateTag = null;

        private string platformTraversableTag = "PlatformTraversable"; 

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

        private bool isSlinding = false;
        private Vector2 penteVelocity;

        private bool isOnCorner = false;
        private bool wasInCorner = false;

        private float elapsedTimerBeforeSetModeAir = 0f;
        private bool canJump = false;

        private Vector2 startPosition;
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

        private bool _jump = false;
        private bool jump 
        {
            get { return _jump; }
            set
            {
                _jump = value;
                if (_jump) OnPlayerJump?.Invoke();
                else OnPlayerEndJump?.Invoke();
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
		private bool isFacingWallJump = false;

        //Animations
        private Vector3 scaleLeft = new Vector3(0.5f, 0.5f, 1f);
        private Vector3 scaleRight = new Vector3(-0.5f, 0.5f, 1f);
        private float idleElapsedTime = 0;
        private float animCounter = 0;
        private bool isPlaying = false;
        private float timeBetweenIdleAndIdleLong = 5f;

        private Rigidbody2D rigidBody = null;
        private Animator animator = null;

        private Action DoAction = null;

        // Properties for Pause
        private Action PreviousDoAction = null;
        private Vector2 pausePos;

        //Event for HUD controller update
        public delegate void PlayerMoveEventHandler(float horizontalAxis);
        public static event PlayerMoveEventHandler OnPlayerMove;
        public static Action OnPlayerJump;
        public static Action OnPlayerEndJump;

        override public void Init()
        {
            Life = settings.StartLife;
            _lastCheckpointPos = transform.position;
            startPosition = transform.position;
        }

        public void Reset()
        {
            InitLife();
            transform.position = startPosition;
            _lastCheckpointPos = transform.position;

            rigidBody.simulated = true;
            gameObject.SetActive(true);
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
            OnPlayerMove?.Invoke(horizontalAxis);
            jump = controller.Jump;
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
        }

        private void SetModePlane()
        {
            SoundManager.Instance.Play(sounds.PlaneFlap01);
            stateTag.name = "Plane"; 
            DoAction = DoActionPlane;
            animator.SetBool(settings.IsPlaningParam, true);
        }

        public void SetModePause()
        {
            PreviousDoAction = DoAction;
            rigidBody.Sleep();

            rigidBody.simulated = false;

            DoAction = DoActionVoid; 
        }

        public void SetModeResume()
        {
            rigidBody.WakeUp();

            rigidBody.simulated = true;

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
                if(anglePente > settings.AngleMinPente && anglePente < settings.AngleMaxPente || anglePente > 150)
                {
                    penteVelocity = tan;
                    rigidBody.gravityScale = 0f;
                    isSlinding = false;
                }
                else
                {
                   // isSlinding = true;
                   // rigidBody.gravityScale = gravity;
                }

                
            }
            
            MoveHorizontalOnGround();

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
                IsGrounded = false;
                if (transform.parent != null) transform.SetParent(null);
                jumpingPS.Play();
                SoundManager.Instance.Play(sounds.Jump);

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
            // Code pour que le Idle Long se lance toute les 5sec si le perso est déjà en Idle
            if(Math.Abs(rigidBody.velocity.x) < 0.1f)
            {
                float animDuration = 1.8f * 45;

                if(isPlaying && animCounter < animDuration) animCounter++;
                else if(animCounter >= animDuration)
                {
                    animCounter = 0;
                    isPlaying = false;
                }

                else idleElapsedTime += Time.deltaTime;
                if(idleElapsedTime >= timeBetweenIdleAndIdleLong)
                {
                    idleElapsedTime = 0;
                    animator.SetTrigger(settings.IdleLong);
                    isPlaying = true;
                }
            }
            else idleElapsedTime = 0;

            // Updating Animator
            transform.localScale = previousDirection >= 0 ? scaleRight : scaleLeft;
            animator.SetFloat(settings.HorizontalSpeedParam, Mathf.Abs(rigidBody.velocity.x));

            if(!_isGrounded)
                animator.SetFloat(settings.VerticalVelocityParam, rigidBody.velocity.y);
        
    }

        private void CheckIsGrounded()
        {
            Vector3 origin = rigidBody.position + Vector2.down * settings.IsGroundedRaycastDistance;

            /*Vector2 lineCastStart = new Vector2(rigidBody.position.x - settings.IsGroundedLineCastDistance, rigidBody.position.y - settings.JumpTolerance); 
            Vector2 lineCastEnd = new Vector2(rigidBody.position.x + settings.IsGroundedLineCastDistance, rigidBody.position.y - settings.JumpTolerance); */

            //LineCast horizontal aux pieds
            hitInfos = Physics2D.Linecast(groundLinecastStartPos.position, groundLinecastEndPos.position, settings.GroundLayerMask);
            Debug.DrawLine(groundLinecastStartPos.position, groundLinecastEndPos.position, Color.red);
            IsGrounded = hitInfos.collider != null;

            if (IsGrounded)
            {
                if (hitInfos.collider.GetComponent<MobilePlatform>() != null) transform.SetParent(hitInfos.transform);
            }
            else
            {
                if(transform.parent != null) transform.SetParent(null);
            }

            if(IsGrounded)
            {
                //RayCast vertical pour recup sa normal pour calculer les pentes
                hitInfosNormal = Physics2D.Raycast(origin, Vector2.down, settings.JumpTolerance, settings.GroundLayerMask);
                Debug.DrawRay(origin, Vector2.down - new Vector2(0, settings.JumpTolerance), Color.blue);

            }

        }

        private void MoveHorizontalOnGround()
        {
            float ratio;
            float horizontalMove;

            if(horizontalAxis != 0f)
            {
                ratio = settings.RunAccelerationCurve.Evaluate(horizontalMoveElapsedTime);
                horizontalMove = Mathf.Lerp(0f, settings.RunSpeed, ratio);

                walkingPS.Play();
                SoundManager.Instance.Play(sounds.FootstepsWood);
            }
            else
            {
                ratio = settings.RunDecelerationCurve.Evaluate(horizontalMoveElapsedTime);
                horizontalMove = Mathf.Lerp(0f, topSpeed, ratio);
            }
            if(!isSlinding)
            {
               //rigidBody.velocity = new Vector2(penteVelocity.normalized.x * horizontalMove * previousDirection,rigidBody.velocity.y); 
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
            CheckIsOnWall();
            CheckIsGrounded(); 
            if (_isGrounded)
            {
                wasOnWall = false;
                wallJumpElaspedTime = 0; 
                SetModeNormal();
                return;
            }

            //Gère le cas ou le joueur est sur un coin de plateforme et lui donne un impulsion pour qu'il soit sur la plateforme
            if(isOnCorner && !wasInCorner)
            {
                wasInCorner = true;
                if(wasInCorner)
                {
                   // StartCoroutine(TestCoroutine());
                    isOnCorner = false;
                }
            }

            if(_isOnWall)
            {
                if(jump && !jumpButtonHasPressed)
                {
                    jumpButtonHasPressed = true;
                    wasOnWall = true;
                    horizontalMoveElapsedTime = 0f;
                    isFacingWallJump = false;
                    topSpeed = settings.WallJumpHorizontalForce;
                    previousDirection = -facingRightWall;
                    rigidBody.velocity = new Vector2(settings.WallJumpHorizontalForce * previousDirection, settings.WallJumpVerticalForce);
                   // rigidBody.velocity = Vector2.Lerp(rigidBody.velocity, new Vector2(settings.WallJumpHorizontalForce * previousDirection, rigidBody.velocity.y), wallJumpElaspedTime / 0.5f);
                   // Debug.Log(Vector2.Lerp(rigidBody.velocity, new Vector2(settings.WallJumpHorizontalForce * previousDirection, rigidBody.velocity.y), wallJumpElaspedTime));
                    ParticleSystem wjParticle = facingRightWall == 1 ? wallJumpPSRight : wallJumpPSLeft;
                    wjParticle.Play();
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
            if(_isOnWall && rigidBody.velocity.y <= -settings.FallOnWallVerticalSpeed)
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, -settings.FallOnWallVerticalSpeed);
            else if(rigidBody.velocity.y <= - settings.FallVerticalSpeed)
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, - settings.FallVerticalSpeed);

            transform.localScale = previousDirection >= 0 ? scaleRight : scaleLeft;
            animator.SetFloat(settings.HorizontalSpeedParam, Mathf.Abs(rigidBody.velocity.x));

            animator.SetFloat(settings.VerticalVelocityParam, rigidBody.velocity.y);

            if(wallJumpElaspedTime >= 1.5f)
            {
                wasOnWall = false;
                wallJumpElaspedTime = 0; 
            }
            if(wasOnWall)
            {
                wallJumpElaspedTime += Time.deltaTime;
                return; 
            }
            else MoveHorizontalInAir(); 
        }


        private void DoActionPlane()
        {
            CheckIsOnWall();
            if(_isOnWall || !jump)
            {
                SoundManager.Instance.Stop(sounds.PlaneWind);
                SetModeAir();
                return;
            }

            CheckIsGrounded();
            if(_isGrounded)
            {
                SoundManager.Instance.Stop(sounds.PlaneWind);
                SetModeNormal();
                return; 
            }

            if(!jumpButtonHasPressed) jumpButtonHasPressed = true; 

            MoveHorizontalPlane(); 

            //Planage vertical
            if(rigidBody.velocity.y <= settings.PlaneVerticalSpeed)
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, - settings.PlaneVerticalSpeed);

            transform.localScale = previousDirection >= 0 ? scaleRight : scaleLeft;
            animator.SetFloat(settings.HorizontalSpeedParam, Mathf.Abs(rigidBody.velocity.x));
            animator.SetFloat(settings.VerticalVelocityParam, rigidBody.velocity.y);

            planePS.Play();
            SoundManager.Instance.Play(sounds.PlaneWind); 
        }

        private void CheckIsOnWall()
        {
            //LineCast verticaux pour tester la collision au mur
            RaycastHit2D hitInfosLeft = Physics2D.Linecast(wallLinecastLeftStartPos.position, wallLinecastLeftEndPos.position, settings.GroundLayerMask); 
            RaycastHit2D hitInfosRight = Physics2D.Linecast(wallLinecastRightStartPos.position, wallLinecastRightEndPos.position, settings.GroundLayerMask);
            Debug.DrawLine(wallLinecastRightStartPos.position, wallLinecastRightEndPos.position, Color.white);
            Debug.DrawLine(wallLinecastLeftStartPos.position, wallLinecastLeftEndPos.position, Color.black);

            //LineCast verticaux pour tester la collision au corner
            RaycastHit2D hitInfosCornerRight = Physics2D.Linecast(cornerLinecastRightStartPos.position, cornerLinecastRightEndPos.position, settings.GroundLayerMask); 
            RaycastHit2D hitInfosCornerLeft = Physics2D.Linecast(cornerLinecastLeftStartPos.position, cornerLinecastLeftEndPos.position, settings.GroundLayerMask);
            Debug.DrawLine(cornerLinecastRightStartPos.position, cornerLinecastRightEndPos.position, Color.yellow);
            Debug.DrawLine(cornerLinecastLeftStartPos.position, cornerLinecastLeftEndPos.position, Color.red);

            if(hitInfosLeft.collider != null)
            {
                IsOnWall = true;
                facingRightWall = transform.localScale == scaleLeft ? -1 : 1;
            }
            else if(hitInfosRight.collider != null)
            {
                IsOnWall = true;
                facingRightWall = transform.localScale == scaleLeft ? 1 : -1;
            }
            else IsOnWall = false;

            if(hitInfosRight.collider && !hitInfosCornerRight.collider) isOnCorner = true;
            else if(hitInfosLeft.collider && !hitInfosCornerLeft.collider) isOnCorner = true;
            else isOnCorner = false;
        }

        private void MoveHorizontalInAir()
        {
            if (horizontalAxis != 0f) // On maintiens une direction lors de la chute
            {
                rigidBody.velocity = Vector2.Lerp(rigidBody.velocity, new Vector2(22f * previousDirection, rigidBody.velocity.y), horizontalMoveElapsedTime); 
            }
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

        //Coroutine qui replace le player qd on arrive a un corner
        private IEnumerator TestCoroutine()
        {
            while(isOnCorner)
            {
                //rigidBody.position = Vector2.MoveTowards(rigidBody.position, target, 1f); Tp le player a une pos 
                rigidBody.velocity += new Vector2(settings.ImpulsionInCorner.x * previousDirection, settings.ImpulsionInCorner.y);
                wasInCorner = false;
                yield return null;
            }
            StopAllCoroutines(); 
        }

        private void DoActionVoid()
        {

        }

        #endregion

        #region LifeMethods
        private void InitLife()
        {
            _life = settings.StartLife;
        }

        private bool CheckRestingLife()
        {
            if(Life == 0)
            {
                animator.SetTrigger(settings.Die); 
                //Die();
            }
            return Life > 0;
        }

        public void AddLife(int EarnedLife = 1)
        {
            Life += EarnedLife;
        }

        public bool LooseLife(int LoseLife = 1)
        {
            Life -= LoseLife;
            return Life > 0;
        }

        public void Die()
        {
            gameObject.SetActive(false);
            OnDie?.Invoke();
            
        }

        public void setPosition(Vector2 position)
        {
            transform.position = position;
        }
        #endregion
    }
}