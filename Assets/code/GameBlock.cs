using UnityEngine;
using System;
using System.Collections.Generic;

namespace fi.tamk.game.theone
{
    public class GameBlock : MonoBehaviour
    {
        [HideInInspector] public List<GameObject> touchList;

        public void Awake()
        {
            touchList = new List<GameObject>();
            SceneManager.Instance.ColliderMap.Add(gameObject, this);
        }

        public virtual bool IsStationary()
        {
            return true;
        }

        public void OnDestroy()
        {
            try {
                // Scene manager might be killed before the objects, so must check to avoid
                // number of exceptions being thrown at the end of game.
                // Another solution is to first deload the game by transitioning into empty
                // scene and then killing all singletons etc...
                if (SceneManager.Instance.ColliderMap != null)
                    SceneManager.Instance.ColliderMap.Remove(gameObject);
            } catch (Exception e)
            {              
                Debug.Log("Exception when trying to remove stuff from collider map:");
                Debug.Log(e.StackTrace);
            }
        }

        void OnCollisionExit2D(Collision2D col)
        {
            touchList.Remove(col.collider.gameObject);
            CollisionEnd(col);
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            touchList.Add(col.collider.gameObject);
            CollisionBegin(col);
        }

        protected virtual void CollisionBegin(Collision2D col)
        {

        }

        protected virtual void CollisionEnd(Collision2D col)
        {

        }
    }
}
