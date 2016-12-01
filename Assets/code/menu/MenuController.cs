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
        /// Reference to persistent data container.
        /// </summary>
        private LevelLoadController _levelLoad;

        private int _levelIndex;

        /// <summary>
        /// Finds LevelLoadController for persistent data. Start is the right place so multiples of that type are already gone.
        /// </summary>
        private void Start()
        {
            _levelLoad = FindObjectOfType<LevelLoadController>();
        }

        /// <summary>
        /// Loads level based on unity scene index.
        /// </summary>
        /// <param name="whichLevel">Which scene to load.</param>
        public void LoadLevel(int whichLevel)
        {
            _levelLoad.ToScene(whichLevel);
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

        public void CheckLockedLevels()
        {
            for (int i = 1; i < LockLevel.levels; i++)
            {
                _levelIndex = (i + 1);
                if ((PlayerPrefs.GetInt("level" + _levelIndex.ToString())) == 1)
                {
                    GameObject.Find("LockedDay" + (i + 1)).SetActive(false);
                    GameObject.Find("Day" + (i + 1)).SetActive(true);
                    Debug.Log("Unlocked");
                }
            }
        }
    }
}
