using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using fi.tamk.game.theone.phys;

namespace fi.tamk.game.theone.ui
{
    /// <summary>
    /// IngameGUI management.
    /// </summary>
    public class IngameGUIController : MonoBehaviour
    {
        /// <summary>
        /// Reference to time indicator.
        /// </summary>
        [SerializeField] private Text timeUiText;

        /// <summary>
        /// Reference to death count indicator.
        /// </summary>
        [SerializeField] private Text livesUiText;

        /// <summary>
        /// Reference to not paused-indicator.
        /// </summary>
        [SerializeField] private Image recordImage;

        /// <summary>
        /// Reference to monetization energy indicator.
        /// </summary>
        [SerializeField] private Image powerImage;

        /// <summary>
        /// Reference to level reset button.
        /// </summary>
        [SerializeField] private Image resetImage;

        /// <summary>
        /// Reference to the image used to display death mask.
        /// </summary>
        [SerializeField] private Image deathImage;

        /// <summary>
        /// Mask color for death image.
        /// </summary>
        [SerializeField] private Color[] deathMask;

        /// <summary>
        /// Time it takes to transition.
        /// </summary>
        [SerializeField] private float deathFlickerDuration = 0.43f;

        /// <summary>
        /// Retuns number of deaths since level loaded.
        /// </summary>
        private int RatNumber
        {
            get { return _ratBackup; }

            set
            {
                _ratBackup = value;
                livesUiText.text = string.Format("RAT #{0}", _ratBackup);
            }
        }

        /// <summary>
        /// Backing field for RatNumber.
        /// </summary>
        private int _ratBackup = 0;

        /// <summary>
        /// Sets deaths and binds to death event.
        /// </summary>
        private void Awake()
        {
            RatNumber = 1;
            SceneManager.Instance.LevelResetEvent += HandlePlayerDeath;
        }

        /// <summary>
        /// Updates the clock.
        /// </summary>
        private void Update()
        {
            var time = SceneManager.Instance.TimeSinceLevelLoaded;

            var hours = (int)(time/3600);
            var minutes = (int)((time%3600)/60);
            var seconds = (int) (time%60);
            var hundos = (int) ((time*100)%100);

            timeUiText.text = string.Format("{0:00}:{1:00}:{2:00}:{3:00}", hours, minutes, seconds, hundos);
        }

        /// <summary>
        /// Recursive handler for death flicker.
        /// </summary>
        /// <param name="duration">How long does one color transition last in seconds.</param>
        /// <param name="cols">Colors to flicker trough.</param>
        /// <param name="iteration">Do not use. Uses this internally.</param>
        /// <returns></returns>
        private IEnumerator DeathFlicker(float duration, Color[] cols, int iteration = 0)
        {
            Color colBegin = cols[iteration % cols.Length];
            Color colEnd = cols[(iteration + 1) % cols.Length];

            var timer = 0f;
            while (timer <= duration)
            {
                timer += SceneManager.Instance.DeltaTime;
                deathImage.color = Color.Lerp(colBegin, colEnd, Mathf.Min(timer / duration, 1f));

                yield return new WaitForEndOfFrame();
            }

            if (iteration < (cols.Length - 2)) StartCoroutine(DeathFlicker(duration, cols, ++iteration));
        }

        /// <summary>
        /// Resets the level without a deaths. Button behavior.
        /// </summary>
        public void ResetButton()
        {
            SceneManager.Instance.PlayerDeathReset();
            RatNumber--;
        }

        /// <summary>
        /// Increases deats by one.
        /// </summary>
        private void HandlePlayerDeath()
        {
            RatNumber++;
            StartCoroutine(DeathFlicker(deathFlickerDuration, deathMask));
        }
    }
}
