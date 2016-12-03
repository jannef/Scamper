using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.menu
{
    public class StampRandomizer : MonoBehaviour
    {
        public const float PositionOffset = 15f;
        public const float AngleOffset = 15.5f;

        private void Awake()
        {
            var tf = transform as RectTransform;
            if (tf == null) return;

            tf.anchoredPosition += new Vector2(Random.Range(-PositionOffset, PositionOffset), Random.Range(-PositionOffset, PositionOffset));
            tf.rotation = Quaternion.Euler(0, 0, Random.Range(-AngleOffset, AngleOffset));
        }
    }
}
