using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PortfolioProject
{
    public class PauseMenu : MonoBehaviour
    {
        public static bool GamePaused = false;

        public GameObject ui;
        public GameObject pauseMenuUI;
        public GameObject settingsMenuUI;

        public void Pause()
        {
            ui.SetActive(false);
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GamePaused = true;
        }

        public void Resume()
        {
            ui.SetActive(true);
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GamePaused = false;
        }

        public void Settings()
        {
            pauseMenuUI.SetActive(false);
            settingsMenuUI.SetActive(true);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void Back()
        {
            pauseMenuUI.SetActive(true);
            settingsMenuUI.SetActive(false);
        }
    }
}
