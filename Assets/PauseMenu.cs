using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PortfolioProject
{
    public class PauseMenu : MonoBehaviour
    {
        public static bool GamePaused = false;

        public GameObject pauseMenuUI;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))  //change this to a UI buttoon press
            {
                //enables the resume function when game is paused and when it isnt paused, allows the pause function
                if (GamePaused)
                {
                    Resumetemp();
                }
                else
                {
                    Pausetemp();
                }
            }
        }

        void Resumetemp()
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GamePaused = false;
        }

        void Pausetemp()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GamePaused = true;
        }

        public void Resume()
        {
            Debug.Log("Resume button");
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GamePaused = false;
        }

        public void Settings()
        {
            Debug.Log("Settings");
        }

        public void QuitGame()
        {
            Debug.Log("Quitting game...");
            Application.Quit();
        }
    }
}
