///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:36
///-----------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Com.IsartDigital.Platformer.Screens {

    [RequireComponent(typeof(Button))]

    public class Hud : AScreen {

        private static Hud _instance;
        public static Hud Instance => _instance;

        public delegate void HudEventHandler(Hud hud);
        public HudEventHandler OnButtonPausePressed;

        [SerializeField] private Text scoreText;
        [SerializeField] private Text lifeText;

        private Button btnPause;

        private float _score = 0;
        public float Score 
        { 
            get => _score;
            set 
            { 
                _score = value;
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
                UpdateText(lifeText, _life);
            }
        }

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }

            _instance = this;

            btnPause = GetComponentInChildren<Button>();
            btnPause.onClick.AddListener(Hud_OnButtonPauseClicked);
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

        private void OnDestroy()
        {
            btnPause.onClick.RemoveListener(Hud_OnButtonPauseClicked);
            _instance = null;
        }
    }

}