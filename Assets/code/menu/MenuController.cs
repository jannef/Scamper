﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace fi.tamk.game.theone.menu
{
    public class MenuController : MonoBehaviour
    {
        private LevelLoadController _levelLoad;

        /// <summary>
        /// Finds LevelLoadController for persistent data. Start is the right place so multiples of that type are already gone.
        /// </summary>
        private void Start()
        {
            _levelLoad = FindObjectOfType<LevelLoadController>();
        }

        private static void LoadLevel(int whichLevel)
        {
            if (whichLevel <= SceneManager.sceneCount)
            {
                SceneManager.LoadScene(whichLevel);
            }
        }

        public void NewGame()
        {
            LoadLevel(1);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void MainMenu()
        {
            LoadLevel(0);
        }
    }
}