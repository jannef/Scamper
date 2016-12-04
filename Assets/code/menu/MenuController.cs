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

        public AudioClip UIClickSound;

        private AudioSource UIClickSource;

        private const float UIClickVolume = 1.0f;
        
        protected void Awake()
        {
            UIClickSource = GetComponent<AudioSource>();
        }

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
            UIClickSource.PlayOneShot(UIClickSound, UIClickVolume);
            Application.Quit();
        }

        /// <summary>
        /// Goes to main menu.
        /// </summary>
        public void MainMenu()
        {
            StartCoroutine(DelayedLoad(UIClickSound, UIClickVolume, UIClickSound.length, 0));
        }

        public void LevelSelect()
        {
            StartCoroutine(DelayedLoad(UIClickSound, UIClickVolume, UIClickSound.length, 1));
        }

        public void InfoScreen()
        {
            StartCoroutine(DelayedLoad(UIClickSound, UIClickVolume, UIClickSound.length, 6));
        }

        public void VictoryScreen()
        {
            LevelLoadController.Instance.ToScene(7);
        }

        private IEnumerator DelayedLoad(AudioClip clip, float clipVolume, float clipLength, int whichScene)
        {
            UIClickSource.PlayOneShot(clip, clipVolume);
            yield return new WaitForSeconds(clipLength);
            LevelLoadController.Instance.ToScene(whichScene);
        }
    }
}
