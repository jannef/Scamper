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
        /// Loads level based on unity scene index.
        /// </summary>
        /// <param name="whichLevel">Which scene to load.</param>
        public void LoadLevel(int whichLevel)
        {
            LevelLoadController.Instance.ToLevel(whichLevel);
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
            LevelLoadController.Instance.ToScene(0);
        }

        public void LevelSelect()
        {
            LevelLoadController.Instance.ToScene(1);
        }

        public void InfoScreen()
        {
            LevelLoadController.Instance.ToScene(6);
        }

        public void VictoryScreen()
        {
            LevelLoadController.Instance.ToScene(7);
        }
    }
}
