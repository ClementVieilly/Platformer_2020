///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:36
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using Com.IsartDigital.Platformer.LevelObjects;
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

		[Header("Score")]
		[SerializeField] private Text scoreText = null;
		[SerializeField] private GameObject scoreObject = null;
		[SerializeField] private GameObject bigScoreObject = null;

		[Header("Life")]
		[SerializeField] private Text lifeText = null;
		[SerializeField] private Image lifeImage = null;

		[Header("sprite for life")]
		[SerializeField] private Sprite lifeSprite1 = null;
		[SerializeField] private Sprite lifeSprite2 = null;
		[SerializeField] private Sprite lifeSprite3 = null;

		[Header("controller")]
		[SerializeField] private Joystick joystick = null;
		[SerializeField] private Joystick jumpButton = null;

        [Header("Slot")]
        [SerializeField] private List<RectTransform> slotsPos = new List<RectTransform>(); 

        private Button btnPause;
        public int SlotNumber = 0; 
		private float _score = 0f;


        private bool isFirstTime = true; 
		public float Score
		{
			get => _score;
			set
			{
				_score = value;
                if(!animator.GetCurrentAnimatorStateInfo(1).IsName(enter)) animator.SetTrigger(enter); 
				_timer = 0;
				UpdateText(scoreText, _score);
			}
		}

		private bool[] _bigScore = new bool[] { false, false, false, false };
		public bool[] BigScore
		{
			get => _bigScore;
			set
			{
                if(isFirstTime)
                {
                    isFirstTime = false;
                    return;
                }
				_bigScore = (bool[])value.Clone();
                scoreObject.SetActive(true);
				bigScoreObject.SetActive(true);
				_timer = 0;
                Tween.LocalPosition(bigScoreObject.transform.GetChild(SlotNumber).transform, new Vector2(slotsPos[SlotNumber].localPosition.x, slotsPos[SlotNumber].localPosition.y), 0.7f, 0,Tween.EaseOut);
                Tween.LocalScale(bigScoreObject.transform.GetChild(SlotNumber).transform, new Vector2(1.2f, 1.2f), 0.2f,0.7f,Tween.EaseIn);
                Tween.LocalScale(bigScoreObject.transform.GetChild(SlotNumber).transform, new Vector2(1, 1), 0.2f,.9f,Tween.EaseOut);
                UpdateText(scoreText, _score);
				UpdateBigScore();
                
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
		public bool Paused { set { paused = value; } }


		private void Awake()
		{
			if (_instance != null)
			{
				Destroy(gameObject);
			}
			else _instance = this;

			btnPause = GetComponentInChildren<Button>();
			btnPause.onClick.AddListener(Hud_OnButtonPauseClicked);

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

		private void Update()
		{
			showHud();
		}

		private void showHud()
		{
			if (!bigScoreObject.activeSelf) return;

			if (paused) return;

			_timer += Time.deltaTime;
			if (_timer > 5)
			{
				bigScoreObject.SetActive(false);
				_timer = 0;
			}
		}

		private void UpdateText(Text changingText, float value)
		{
			changingText.text = value.ToString();
		}

		private void UpdateBigScore()
		{
			for (int i = _bigScore.Length - 1; i >= 0; i--)
				bigScoreObject.transform.GetChild(i).gameObject.SetActive(_bigScore[i]);
		}

		private void Hud_OnButtonPauseClicked()
		{
			paused = true;
			_timer = 0;

			OnButtonPausePressed?.Invoke(this);
		}

		internal void UIManager_OnResume()
		{
			scoreObject.SetActive(true);
			bigScoreObject.SetActive(true);

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
	}
}