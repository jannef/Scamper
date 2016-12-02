using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace fi.tamk.game.theone.menu
{
    /// <summary>
    /// Class for handling UI elements.
    /// </summary>
    /// <auth>Janne Forsell</auth>
    public class MenuController : MonoBehaviour
    {
        /// <summary>
        /// Index number of a certain level.
        /// </summary>
        private int _levelIndex;

        /// <summary>
        /// Finds LevelLoadController for persistent data. Start is the right place so multiples of that type are already gone.
        /// </summary>
        private void Awake()
        {

            for (int i = 0; i <= LockLevel.levels; i++)
            {
                _levelIndex = i;
                CheckLockedLevels();
            }
        }

        /// <summary>
        /// Loads level based on unity scene index.
        /// </summary>
        /// <param name="whichLevel">Which scene to load.</param>
        public void LoadLevel(int whichLevel)
        {
            LevelLoadController.Instance.ToScene(whichLevel);
        }

        /// <summary>
        /// Quits the application.
        /// </summary>
        public void QuitGame()
        {
            Application.Quit();
        }

        /// <summary>
        /// Goes to main menu.
        /// </summary>
        public void MainMenu()
        {
            LoadLevel(0);
        }

        public void LevelSelect()
        {
            LoadLevel(1);
        }

        public void InfoScreen()
        {
            LoadLevel(6);
        }

        public void VictoryScreen()
        {
            LoadLevel(7);
        }

        public void CheckLockedLevels()
        {
            for (int j = 1; j < LockLevel.levels; j++)
            {
                _levelIndex = (j + 1);
                bool locked = true;

                if (LevelLoadController.Instance.GetLevelsLocked().TryGetValue(_levelIndex, out locked))
                {
                    GameObject.Find("LockedDay" + (j + 1)).SetActive(false);
                    GameObject.Find("Day" + (j + 1) + "Button").SetActive(true);
                    Debug.Log("Unlocked level " + (j + 1));
                }
            }
        }
    }
}
