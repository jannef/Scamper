using UnityEngine;
using System.Collections;
using fi.tamk.game.theone.phys;

namespace fi.tamk.game.theone.phys
{
    public class RemoveActivatorButton : MonoBehaviour
    {
        /// <summary>
        /// Blocks this remote activator will affect.
        /// </summary>
        [SerializeField] private PGameBlock[] ActivatedBlocks;

        /// <summary>
        /// Alternate sprite to use when in pushed state.
        /// </summary>
        [SerializeField] private Sprite PushedStateSprite;

        /// <summary>
        /// Collisions with acceptable activators based on tag currently active.
        /// </summary>
        private int _collisions = 0;

        private SpriteRenderer _spriteRenderer = null;
        private Sprite _originalSprite = null;

        /// <summary>
        /// Get reference to sprite renderer and current sprite.
        /// </summary>
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer != null) _originalSprite = _spriteRenderer.sprite;

            SceneManager.Instance.LevelResetEvent += OnLevelReset;
        }

        /// <summary>
        /// If this button was just touched, sends the remote activation signal.
        /// </summary>
        /// <param name="col">the collision</param>
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (!CheckTags(col)) return;
            if (_collisions == 0)
            {
                foreach (var block in ActivatedBlocks)
                {
                    if (block == null) continue;
                    block.OnRemoteActivation();
                }
            }

            _collisions++;
            ChangeToSprite(PushedStateSprite);
        }

        /// <summary>
        /// If this button is no longer touched, sends the remote reset signal.
        /// </summary>
        /// <param name="col">the collision</param>
        private void OnCollisionExit2D(Collision2D col)
        {
            if (!CheckTags(col)) return;
            _collisions--;

            if (_collisions != 0) return;
            ChangeToSprite(_originalSprite);

            foreach (var block in ActivatedBlocks)
            {
                if (block == null) continue;
                block.OnRemoteActivationActionReset();
            }
        }

        /// <summary>
        /// Checks if the colliding object is of acceptable tag.
        /// </summary>
        /// <param name="col"></param>
        /// <returns>the collision from which to look the tag from</returns>
        private bool CheckTags(Collision2D col)
        {
            return col.collider.gameObject.CompareTag("Movable") ||
                   col.collider.gameObject.CompareTag("MovableWalkable");
        }

        /// <summary>
        /// Changes the attached sprite renderers sprite to given sprite, if both are defined.
        /// </summary>
        /// <param name="toWhich">new sprite</param>
        private void ChangeToSprite(Sprite toWhich)
        {
            if (_spriteRenderer != null && toWhich != null) _spriteRenderer.sprite = toWhich;
        }

        /// <summary>
        /// Changes to original sprite and resets the button.
        /// </summary>
        private void OnLevelReset()
        {
            ChangeToSprite(_originalSprite);
            _collisions = 0;
        }
    }
}
