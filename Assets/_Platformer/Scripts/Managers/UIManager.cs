///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 22/01/2020 15:05
///-----------------------------------------------------------------

using System;
using Com.IsartDigital.Platformer.Screens;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Managers
{
    public class UIManager : MonoBehaviour
    {

        [SerializeField] private GameObject hudPrefab;
        [SerializeField] private GameObject pausePrefab;

        private Hud currentHud;//correspond au hud actuel utilisé (PC ou mobile)
        private PauseMenu currentPauseMenu;//correspond au menu pause actuel utilisé 

        private void Awake()
        {
            currentHud = CreateHud().GetComponent<Hud>();
            currentHud.OnButtonPausePressed += Hud_OnPauseButtonPressed;
        }

        private GameObject CreatePauseMenu() //Crée une instance de Menu Pause et renvoie sa référence
        {
            return Instantiate(pausePrefab);
        }

        private GameObject CreateHud() //Crée une instance de Hud et renvoie une référence vers elle
        {
            return Instantiate(hudPrefab);
        }

        private void Hud_OnPauseButtonPressed(Hud hud) //Fonction callback de l'event de click sur le bouton pause du Hud
        {
            currentPauseMenu = CreatePauseMenu().GetComponent<PauseMenu>();
            currentPauseMenu.OnResumeClicked += PauseMenu_OnResumeClicked;
            currentPauseMenu.OnRetryClicked += PauseMenu_OnRetryClicked;
            currentPauseMenu.OnHomeClicked += PauseMenu_OnHomeClicked;
        }

        private void PauseMenu_OnResumeClicked(PauseMenu pauseMenu)
        {
            currentPauseMenu.OnResumeClicked -= PauseMenu_OnResumeClicked;
            Destroy(pauseMenu.gameObject);
        }
        private void PauseMenu_OnRetryClicked(PauseMenu pauseMenu)
        {
            currentPauseMenu.OnResumeClicked -= PauseMenu_OnRetryClicked;
            Destroy(pauseMenu.gameObject);
        }
        private void PauseMenu_OnHomeClicked(PauseMenu pauseMenu)
        {
            currentPauseMenu.OnResumeClicked -= PauseMenu_OnHomeClicked;
            currentHud.OnButtonPausePressed -= Hud_OnPauseButtonPressed;

            Destroy(currentHud.gameObject);
            Destroy(pauseMenu.gameObject);

        }



    }
}