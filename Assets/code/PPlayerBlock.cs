using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    public class PPlayerBlock : PGameBlock
    {
        [SerializeField] protected Vector2 PlayerSpeed = new Vector2(2.2f, 0);
        [SerializeField] protected AnimationController PlayerAnimation = null;
        [SerializeField] protected Transform FloorCollisionTolerance = null;
        private bool _moving = false;
        private Checkpoint _activeCheckpoint = null;

        #region MonoBehaviourMethods
        /**
         * Kills player when he collides with a block.
         * 
         * This also hides default behaviour so no touchlist is generated, this is intented for now.
         */
        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.gameObject.CompareTag("Movable") ||
                (col.collider.gameObject.CompareTag("MovableWalkable") && !CollisionBelow(col)))
            {
                SceneManager.Instance.PlayerDeathReset();
            }
        }

        private bool CollisionBelow(Collision2D col)
        {
            if (FloorCollisionTolerance == null) return false;

            var footHeight = FloorCollisionTolerance.position.y;
            var colHeight = col.contacts[0].point.y;

            return colHeight < footHeight;
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
            var speedToSet = Mathf.Abs(PlayerSpeed.x);

            if (_moving)
            {
                MyTransform.Translate(PlayerSpeed*SceneManager.Instance.DeltaTime, Space.World);
            }
            else
            {
                speedToSet = 0f;
            }

            if (PlayerAnimation != null) PlayerAnimation.Speed = speedToSet;

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