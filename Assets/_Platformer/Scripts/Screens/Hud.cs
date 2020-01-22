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

        public delegate void HudEventHandler(Hud hud);
        public HudEventHandler OnButtonPausePressed;

        private Button btnPause;

        private void Awake()
        {
            btnPause = GetComponentInChildren<Button>();
            btnPause.onClick.AddListener(Hud_OnButtonPauseClicked);
        }

        //envoie d'un event lors d'un clic sur le bouton pause
        private void Hud_OnButtonPauseClicked()
        {
            OnButtonPausePressed?.Invoke(this);
        }

        private void OnDestroy()
        {
            btnPause.onClick.RemoveListener(Hud_OnButtonPauseClicked);
        }
    }

}