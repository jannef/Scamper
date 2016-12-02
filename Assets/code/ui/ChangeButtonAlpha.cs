using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace fi.tamk.game.theone.ui
{
    public class ChangeButtonAlpha : MonoBehaviour
    {
        [SerializeField]
        private static Button _button;

        [SerializeField]
        private static float _alphaAmount;

        private Color _buttonColor = _button.image.color;
        
        private void changeAlpha()
        {
            _buttonColor.a = _alphaAmount;
            _button.image.color = _buttonColor;
        }

    }
}
