using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone
{
    public class GameBlock : MonoBehaviour
    {
        public void Awake()
        {
            SceneManager.Instance.ColliderMap.Add(GetComponent<Collider2D>(), this);
        }

        public virtual bool IsStationary()
        {
            return true;
        }
    }
}
