using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    public class LevelEndCheckpoin : MonoBehaviour {
        [SerializeField] private int ToWhichLevel = 0;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject == SceneManager.Instance.PlayerGameObject)
            {
                SceneManager.Instance.PersistentData.ToScene(ToWhichLevel);
            }
        }
    }
}
