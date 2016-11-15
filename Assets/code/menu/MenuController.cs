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
        /// Reference to persitent data container.
        /// </summary>
        private LevelLoadController _levelLoad;

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
        private static void LoadLevel(int whichLevel)
        {
            if (whichLevel <= SceneManager.sceneCount)
            {
                SceneManager.LoadScene(whichLevel);
            }
        }

        /// <summary>
        /// Starts a new game.
        /// </summary>
        public void NewGame()
        {
            LoadLevel(1);
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
    }
}
