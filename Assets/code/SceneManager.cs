﻿using UnityEngine;
using System.Collections.Generic;

namespace fi.tamk.game.theone
{
    public class SceneManager : Singleton<SceneManager>
    {
        /**
         *  Flag for pausing the game.
         *
         *  Game should probably not be paused in the middle of an update cycle... some objects might
         *  get to move 0-2 times extra depending on when the flow is interrupted/resumed compared to
         *  other objects.
         */
        public bool Pause = false;
        public float GlobalGravity = -0.2f;

        public Dictionary<GameObject, GameBlock> ColliderMap;

        /**
         *  Contains Time.deltaTime or 0 depending if the game is paused.
         */
        public float DeltaTime
        {
            get
            {
                if (Pause) return 0f;
                return Time.deltaTime;
            }
        }

        /**
         *  Is private to keep this solely a singleton class.
         */
        private SceneManager()
        {
            ColliderMap = new Dictionary<GameObject, GameBlock>();
        }

        // TODO: Here for testing purposes only!! REMOVE/MOVE AT SOME POINT!!
        void Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                Pause = !Pause;
            }
        }

        public void PlayerDeathReset()
        {
            foreach(KeyValuePair<GameObject, GameBlock> t in ColliderMap)
            {
                if (t.Key.CompareTag("Movable")) t.Value.Reset();
            }
        }
    }
}
