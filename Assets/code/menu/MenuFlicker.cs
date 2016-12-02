using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine.UI;

namespace fi.tamk.game.theone.menu
{
    public class MenuFlicker : MonoBehaviour
    {
        [SerializeField] private Sprite AlternateSprite;
        [SerializeField] private float[] Durations;

        private Image _renderer;
        private Sprite _originalImage;

        private void Awake()
        {
            _renderer = GetComponent<Image>();
            _originalImage = _renderer.sprite;

            StartCoroutine(Flicker());
        }

        private IEnumerator Flicker(int count = 0)
        {
            _renderer.sprite = (count%2 == 1) ? AlternateSprite : _originalImage;
            yield return new WaitForSecondsRealtime(Durations[count%Durations.Length]);
            StartCoroutine(Flicker(count + 1));
        }
    }
}
