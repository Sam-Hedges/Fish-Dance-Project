using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PortfolioProject
{
    public class PauseMenu : MonoBehaviour
    {
        // starts with the pause menu closed
        public static bool GamePaused = false;

        // game objects
        public GameObject ui;
        public GameObject pauseMenuUI;
        public GameObject settingsMenuUI;

        // this function removes the pause button, pulls up the menu and pauses the game 
        public void Pause()
        {
            ui.SetActive(false);
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GamePaused = true;
        }

        // this function pulls up the pause button, removes the menu and unpauses the game
        public void Resume()
        {
            ui.SetActive(true);
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GamePaused = false;
        }

        // this function removes the pause menu and pulls up the settings menu
        public void Settings()
        {
            pauseMenuUI.SetActive(false);
            settingsMenuUI.SetActive(true);
        }

        // this function quits the game
        public void QuitGame()
        {
            Application.Quit();
        }

        // this function pulls up the pause menu and removes the settings menu
        public void Back()
        {
            pauseMenuUI.SetActive(true);
            settingsMenuUI.SetActive(false);
        }
    }
}
