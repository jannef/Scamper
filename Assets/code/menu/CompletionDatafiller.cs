using UnityEngine;
using System.Collections;
using fi.tamk.game.theone.ui;
using UnityEngine.UI;

namespace fi.tamk.game.theone.menu
{
    public class CompletionDatafiller : MonoBehaviour
    {
        [SerializeField] private Text Rats;
        [SerializeField] private Text Time;

        private void Awake()
        {
            Rats.text = LevelLoadController.Instance.RatsDeadPerLevel.ToString();
            Time.text = IngameGUIController.SecondsToString(LevelLoadController.Instance.LevelCompletionTime);

            LevelLoadController.Instance.RatsDeadPerLevel = 0;
        }
    }
}
