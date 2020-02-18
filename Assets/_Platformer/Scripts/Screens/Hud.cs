///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:36
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.LevelObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Com.IsartDigital.Platformer.Screens
{

    [RequireComponent(typeof(Button))]

    public class Hud : AScreen {

        private static Hud _instance;
        public static Hud Instance => _instance;

        public delegate void HudEventHandler(Hud hud);
        public HudEventHandler OnButtonPausePressed;

        [Header("Score")]
        [SerializeField] private Text scoreText;
        [SerializeField] private GameObject scoreObject;

        [Header("Life")]
        [SerializeField] private Text lifeText;
        [SerializeField] private Image lifeImage;

        [Header("sprite for life")]
        [SerializeField] private Sprite lifeSprite1;
        [SerializeField] private Sprite lifeSprite2;
        [SerializeField] private Sprite lifeSprite3;

        [Header("controller")]
        [SerializeField] private Slider moveSlider;
        [SerializeField] private Button jumpButton;

        private Button btnPause;

        private float _score = 0;
        public float Score 
        { 
            get => _score;
            set 
            { 
                _score = value;
                scoreObject.SetActive(true);
                _timer = 0;
                UpdateText(scoreText, _score);
            } 
        }

        private float _life = 0;
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

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else _instance = this;

            btnPause = GetComponentInChildren<Button>();
            btnPause.onClick.AddListener(Hud_OnButtonPauseClicked);
            Player.OnPlayerMove += UpdateMoveController;
            Player.OnPlayerJump += UpdateJumpController;
            Player.OnPlayerEndJump += UpdateJumpController2;
        }

        private float _timer = 0f;
        private void Update()
        {
            showHud();
        }

        private void showHud()
        {
            if (!scoreObject.activeSelf) return;

            _timer += Time.deltaTime;
            if (_timer > 3)
            {
                scoreObject.SetActive(false);
                _timer = 0;
            }
        }

        private void UpdateText(Text changingText, float value)
        {
            changingText.text = value.ToString();
        }

        //envoie d'un event lors d'un clic sur le bouton pause
        private void Hud_OnButtonPauseClicked()
        {
            OnButtonPausePressed?.Invoke(this);
        }

        private void UpdateMoveController(float horizontalAxis)
        {
            moveSlider.value = Mathf.Lerp(moveSlider.value, horizontalAxis,0.1f);
        }

        private void UpdateJumpController()
        {
            Debug.Log("update jump button");
            jumpButton.image.color = Color.green;
        }
        private void UpdateJumpController2()
        {
            Debug.Log("update jump button");
            jumpButton.image.color = Color.white;
        }

        private void OnDestroy()
        {
            btnPause.onClick.RemoveListener(Hud_OnButtonPauseClicked);
            Player.OnPlayerMove -= UpdateMoveController;
            Player.OnPlayerJump -= UpdateJumpController;
            Player.OnPlayerEndJump -= UpdateJumpController2;
            _instance = null;
        }

        public override void UnsubscribeEvents()
        {
            OnButtonPausePressed = null;
        }
    }

}