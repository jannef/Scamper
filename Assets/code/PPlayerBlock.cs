﻿using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    public class PPlayerBlock : PGameBlock
    {
        [SerializeField] protected Vector2 PlayerSpeed = new Vector2(2.2f, 0);

        #region States
        private bool _moving = false;
        private Checkpoint _activeCheckpoint = null;
        #endregion

        #region MonoBehaviourMethods
        /**
         * Kills player when he collides with a block.
         * 
         * This also hides default behaviour so no touchlist is generated, this is intented for now.
         */
        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.gameObject.CompareTag("Movable"))
            {
                SceneManager.Instance.PlayerDeathReset();
            }
        }

        /**
         * Hides default behaviour, because player has no reason to manage touchlist.
         */
        void OnCollisionExit2D(Collision2D col)
        {

        }

        /**
         * Moves player in the world.
         */
        void Update()
        {
            if (_moving)
            {
                MyTransform.Translate(PlayerSpeed * SceneManager.Instance.DeltaTime, Space.World);
            }
        }

        /**
         * Overrides default behaviour that is used for "NPC" block to simply enable movement.
         */
        void OnMouseDown()
        {
            if (!LockedFromPlayer
                && !_moving && _activeCheckpoint == null
                && !SceneManager.Instance.Pause)
            {
                _moving = true;
            }
        }
        #endregion

        /**
         * Sets state of the block to default values.
         */
        public override void ResetBlock()
        {
            base.ResetBlock();
            _moving = false;
        }

        public void Checkpoint(Checkpoint c)
        {
            StartLocation = c.Spawn.position;
            _moving = false;
            _activeCheckpoint = c;
        }

        public void CheckpointRelease()
        {
            _activeCheckpoint = null;
        }
    }
}