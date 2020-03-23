///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:36
///-----------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using Com.IsartDigital.Platformer.LevelObjects;
using Com.IsartDigital.Platformer.Managers;
using Pixelplacement;
using UnityEngine;
using UnityEngine.UI;


namespace Com.IsartDigital.Platformer.Screens
{
	[RequireComponent(typeof(Button), typeof(Animator))]
	public class Hud : AScreen
	{
		private static Hud _instance;
		public static Hud Instance => _instance;

		public delegate void HudEventHandler(Hud hud);
		public event HudEventHandler OnButtonPausePressed;
		public event HudEventHandler OnFinalAnimFinished;

        private LevelManager lvlManager = null; 

		[Header("Score")]
		[SerializeField] private Text scoreText = null;
		[SerializeField] private GameObject scoreObject = null;
		[SerializeField] private GameObject bigScoreObject = null;
        [SerializeField] private AnimationCurve bigScoreEnterAnim = null; 
        [SerializeField] private AnimationCurve bigScoreEnterAnimWin = null; 

		[Header("Life")]
		[SerializeField] private Text lifeText = null;
		[SerializeField] private Image lifeImage = null;
		[SerializeField] private Transform lifeCadre = null;
		[SerializeField] private Transform lifeBorder = null;
		[SerializeField] private Transform imgCadre = null;

		[Header("sprite for life")]
		[SerializeField] private Sprite lifeSprite1 = null;
		[SerializeField] private Sprite lifeSprite2 = null;
		[SerializeField] private Sprite lifeSprite3 = null;

		[Header("controller")]
		[SerializeField] private Joystick joystick = null;
		[SerializeField] private Joystick jumpButton = null;

        [Header("Slot")]


        [SerializeField] private List<RectTransform> slotsPos = new List<RectTransform>(); 
        private List<Vector2> keyStartPo = new List<Vector2>(); 

        private Button btnPause;
        public int SlotNumber = 0; 
		private float _score = 0f;

        private bool isFirstTimeBg = true; 
        private bool isFirstTime = true; 
		public float Score
		{
			get => _score;
			set
			{
                if(isFirstTime)
                {
                    isFirstTime = false;
                    return;
                }
                _score = value;
                if(!animator.GetCurrentAnimatorStateInfo(1).IsName(enter))
                {
                    animator.SetTrigger(enter);
                   
                }
                else UpdateText(scoreText, _score);
                _timer = 0;
				
			}
		}

		//private bool[] _bigScore = new bool[] { false, false, false, false };
        private List<Transform> keyTab = new List<Transform>();
		public bool[] BigScore
		{
			//get => _bigScore;
			set
			{
                if(isFirstTimeBg)
                {
                    isFirstTimeBg = false;
                    return;
                }
			//	_bigScore = (bool[])value.Clone();
                scoreObject.SetActive(true);
				bigScoreObject.SetActive(true);
				_timer = 0;
                Tween.LocalPosition(bigScoreObject.transform.GetChild(SlotNumber).transform, new Vector2(slotsPos[SlotNumber].localPosition.x, slotsPos[SlotNumber].localPosition.y), 1, 0,bigScoreEnterAnim);
                keyTab.Add(bigScoreObject.transform.GetChild(SlotNumber).transform); 
				//UpdateBigScore();
			}
		}

		private float _life = 0f;
		public float Life
		{
			get => _life;
			set
			{
				_life = value;
				switch (_life)
				{
					case 1:
						lifeImage.sprite = lifeSprite1;
						break;
					case 2:
						lifeImage.sprite = lifeSprite2;
						break;
					case 3:
						lifeImage.sprite = lifeSprite3;
						break;
				}
				UpdateText(lifeText, _life);
			}
		}

		private float _timer = 0f;
		private bool paused = false;
        private bool test = false; 

        public bool Paused { set { paused = value; } }


		private void Awake()
		{
           
			if (_instance != null)
			{
				Destroy(gameObject);
			}
			else _instance = this;
            lvlManager = FindObjectOfType<LevelManager>();
            btnPause = GetComponentInChildren<Button>();
			btnPause.onClick.AddListener(Hud_OnButtonPauseClicked);
            SaveKeyStartPos(); 

#if UNITY_ANDROID || UNITY_EDITOR
			Player.OnPlayerMove += UpdateMoveController;
			Player.OnPlayerJump += GrowJumpButton;
			Player.OnPlayerEndJump += StopGrowJumpButton;
			Player.OnPlayerPlane += PulseJumpButton;
			Player.OnPlayerEndPlane += StopPulsingJumpButton;
			joystick.gameObject.SetActive(true);
			jumpButton.gameObject.SetActive(true);
#endif
		}
        public void RegisterSelfAnimator()
		{
			animator = GetComponent<Animator>();
		}

		private void GrowJumpButton()
		{
			animator.SetBool("IsHold", true);
		}

		private void StopGrowJumpButton()
		{
			animator.SetBool("IsHold", false);
		}

		private void PulseJumpButton()
		{
			animator.SetBool("IsPlane", true);
		}

		private void StopPulsingJumpButton()
		{
			animator.SetBool("IsPlane", false);
		}

		private void UpdateText(Text changingText, float value)
		{
			changingText.text = value.ToString();

            if(changingText == lifeText)
            {
                Tween.LocalRotation(lifeImage.transform, Quaternion.AngleAxis(20f, Vector3.forward),0.2f, 0, Tween.EaseOut); 
                Tween.LocalRotation(lifeImage.transform, Quaternion.AngleAxis(-20f, Vector3.forward), 0.2f, 0.2f, Tween.EaseOut); 
                Tween.LocalRotation(lifeImage.transform, Quaternion.identity, 0.2f, .4f, Tween.EaseOut); 

                Tween.LocalScale(lifeCadre, new Vector2(1.7f, 1.7f), 0.2f, .6f, Tween.EaseIn);
                Tween.LocalScale(lifeCadre, new Vector2(1, 1), 0.2f, 0.8f, Tween.EaseIn);
                Tween.LocalScale(lifeBorder, new Vector2(1.7f, 1.7f), 0.2f, .6f, Tween.EaseIn);
                Tween.LocalScale(lifeBorder, new Vector2(1, 1), 0.2f, .8f, Tween.EaseIn);
            }
		}

        private void callBack()
        {
            UpdateText(scoreText, _score);
        }

		/*private void UpdateBigScore()
		{
			for (int i = _bigScore.Length - 1; i >= 0; i--)
				bigScoreObject.transform.GetChild(i).gameObject.SetActive(_bigScore[i]);
		}*/

		private void Hud_OnButtonPauseClicked()
		{
			paused = true;

			OnButtonPausePressed?.Invoke(this);
		}

		internal void UIManager_OnResume()
		{
			paused = false;
		}

		private void UpdateMoveController(float horizontalAxis)
		{
			joystick.UpdateHandleHorizontalPosition(horizontalAxis);
		}

		private void OnApplicationPause(bool pause)
		{
			if(pause) OnButtonPausePressed?.Invoke(this);
		}

		private void OnDestroy()
		{
			btnPause.onClick.RemoveListener(Hud_OnButtonPauseClicked);
			Player.OnPlayerMove -= UpdateMoveController;
			Player.OnPlayerJump -= PulseJumpButton;
			Player.OnPlayerEndJump -= StopPulsingJumpButton;
			Player.OnPlayerPlane -= GrowJumpButton;
			Player.OnPlayerEndPlane -= StopGrowJumpButton;

			_instance = null;
		}

		public override void UnsubscribeEvents()
		{
			OnButtonPausePressed = null;
		}

        private void SaveKeyStartPos()
        {
            for(int i = 0; i < bigScoreObject.transform.childCount; i++)
            {
                keyStartPo.Add(bigScoreObject.transform.GetChild(i).localPosition);
            }
        }

        public void ResetKeyPos()
        {
            for(int i = keyStartPo.Count - 1; i >= 0; i--)
            {
                bigScoreObject.transform.GetChild(i).localPosition = keyStartPo[i];
            }
            isFirstTime = true;
            isFirstTimeBg = true;
        }
	}
}