using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using fi.tamk.game.theone.phys;

namespace fi.tamk.game.theone.ui
{
    public class IngameGUIController : MonoBehaviour
    {
        [SerializeField] private Text timeUiText;
        [SerializeField] private Text livesUiText;
        [SerializeField] private Image recordImage;
        [SerializeField] private Image powerImage;
        [SerializeField] private Image resetImage;

        private int RatNumber
        {
            get { return _ratBackup; }

            set
            {
                _ratBackup = value;
                livesUiText.text = string.Format("RAT #{0}", _ratBackup);
            }
        }

        private int _ratBackup = 0;

        private void Awake()
        {
            RatNumber = 1;
            SceneManager.Instance.LevelResetEvent += IncreaseDeaths;
        }

        private void Update()
        {
            var time = SceneManager.Instance.TimeSinceLevelLoaded;

            var hours = (int)(time/3600);
            var minutes = (int)((time%3600)/60);
            var seconds = (int) (time%60);
            var hundos = (int) ((time*100)%100);

            timeUiText.text = string.Format("{0:00}:{1:00}:{2:00}:{3:00}", hours, minutes, seconds, hundos);
        }

        public void ResetButton()
        {
            SceneManager.Instance.PlayerDeathReset();
            RatNumber--;
        }

        private void IncreaseDeaths()
        {
            RatNumber++;
        }
    }
}
