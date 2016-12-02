using UnityEngine;
using System.Collections;
using System.Text;
using fi.tamk.game.theone.menu;
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
        /// List of possible GUI states.
        /// </summary>
        private enum GuiState { Start, Normal, Paused }

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
        /// Paused sprite for recordImage;
        /// </summary>
        [SerializeField] private Sprite notReconrdingSprite;

        /// <summary>
        /// Original sprite of recordImage.
        /// </summary>
        private Sprite recordSprite;

        /// <summary>
        /// Reference to level reset button.
        /// </summary>
        [SerializeField] private Image resetImage;

        /// <summary>
        /// Reference to the image used to display death mask.
        /// </summary>
        [SerializeField] private Image deathImage;

        /// <summary>
        /// Pause button image.
        /// </summary>
        [SerializeField] private Image pauseImage;

        /// <summary>
        /// Play buttin sprite to replace pause image.
        /// </summary>
        [SerializeField] private Sprite playImageSprite;

        /// <summary>
        /// Original sprite of pauseImage;
        /// </summary>
        private Sprite pauseSprite;

        /// <summary>
        /// Mask color for death image.
        /// </summary>
        [SerializeField] private Color[] deathMask;

        /// <summary>
        /// Time it takes to transition.
        /// </summary>
        [SerializeField] private float deathFlickerDuration = 0.43f;

        /// <summary>
        /// The pause menu;
        /// </summary>
        [SerializeField] private GameObject PauseMenu;

        /// <summary>
        /// State of the GUI.
        /// </summary>
        private GuiState _guiState = GuiState.Start;

        /// <summary>
        /// Activates given gui state.
        /// </summary>
        /// <param name="whichState"></param>
        private void ActivateState(GuiState whichState)
        {
            _guiState = whichState;

            switch (whichState)
            {
                case GuiState.Normal:
                    SceneManager.Instance.Pause = false;
                    pauseImage.sprite = pauseSprite;
                    recordImage.sprite = recordSprite;
                    PauseMenu.SetActive(false);
                    break;
                case GuiState.Paused:
                    SceneManager.Instance.Pause = true;
                    pauseImage.sprite = playImageSprite;
                    recordImage.sprite = notReconrdingSprite;
                    PauseMenu.SetActive(true);
                    break;
                case GuiState.Start:
                default:
                    break;
            }
        }

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
            pauseSprite = pauseImage.sprite;
            recordSprite = recordImage.sprite;

            RatNumber = 1;
            SceneManager.Instance.LevelResetEvent += HandlePlayerDeath;

            ActivateState(GuiState.Normal);
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
            deathImage.raycastTarget = true;

            Color colBegin = cols[iteration % cols.Length];
            Color colEnd = cols[(iteration + 1) % cols.Length];

            var timer = 0f;
            while (timer <= duration)
            {
                timer += SceneManager.Instance.DeltaTime;
                deathImage.color = Color.Lerp(colBegin, colEnd, Mathf.Min(timer / duration, 1f));

                yield return new WaitForEndOfFrame();
            }

            if (iteration < (cols.Length - 2))
            {
                StartCoroutine(DeathFlicker(duration, cols, ++iteration));
            }
            else
            {
                deathImage.raycastTarget = false;
            }
        }

        /// <summary>
        /// Resets the level without a deaths. Button behavior.
        /// </summary>
        public void ResetButton()
        {
            if (_guiState != GuiState.Normal) ActivateState(GuiState.Normal);
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

        public void PausePlayButton()
        {
            ActivateState(_guiState == GuiState.Normal ? GuiState.Paused : GuiState.Normal);
        }

        public void BackToLevelSelect()
        {
            LevelLoadController.Instance.ToLevelSelect();
        }
    }
}
