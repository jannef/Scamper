﻿using UnityEngine;
using System.Collections;
using fi.tamk.game.theone.phys;
using UnityEngine.Assertions.Comparers;

namespace fi.tamk.game.theone.phys
{
    /// <summary>
    /// Remotely activates blocks when assosiated game object is in contact with other
    /// blocks.
    /// </summary>
    /// <auth>Janne Forsell</auth>
    public class RemoveActivatorButton : RemoteBase
    {

        public AudioClip magnetSound;
        private const float magnetVolume = 1.0f;

        /// <summary>
        /// Alternate sprite to use when in pushed state.
        /// </summary>
        [SerializeField] private Sprite PushedStateSprite;

        /// <summary>
        /// How long does the button take consider deactivatred after losign contact.
        /// </summary>
        [SerializeField] private float ContactInterruptionTolerance = 0.2f;

        private float _timeSinceContactInterruption = 0f;
        private bool _isPressed = false;

        /// <summary>
        /// Collisions with acceptable activators based on tag currently active.
        /// </summary>
        [SerializeField] private int _collisions = 0;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        private Sprite _originalSprite = null;

        /// <summary>
        /// Get reference to sprite renderer and current sprite.
        /// </summary>
        private void Awake()
        {
            if (_spriteRenderer != null) _originalSprite = _spriteRenderer.sprite;

            SceneManager.Instance.LevelResetEvent += OnLevelReset;
        }

        /// <summary>
        /// Handles deactivating the button after a grace period defined by ContactInterruptionTolerance.
        /// </summary>
        private void Update()
        {
            if (_collisions == 0 && _isPressed)
            {
                _timeSinceContactInterruption += SceneManager.Instance.DeltaTime;
                if (_timeSinceContactInterruption > ContactInterruptionTolerance)
                {
                    EndPressedState();
                }
            }
            else
            {
                _timeSinceContactInterruption = 0f;
            }
        }

        /// <summary>
        /// If this button was just touched, sends the remote activation signal.
        /// </summary>
        /// <param name="col">the collision</param>
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (!CheckTags(col)) return;

            if (_collisions == 0 && !_isPressed)
            {
                SceneManager.Instance.PlayDistanceBasedSound(magnetSound, magnetVolume, transform.position);
                //magnetSource.PlayOneShot(magnetSound, magnetVolume);
                ActivateBlocks();
                _isPressed = true;
            }

            _collisions++;
            ChangeToSprite(PushedStateSprite);
        }

        /// <summary>
        /// Returns button to unpressed state. Swaps graphics, sends out deactivation signals.
        /// </summary>
        private void EndPressedState()
        {
            _isPressed = false;
            ChangeToSprite(_originalSprite);

            DeactivateBlocks();
        }

        /// <summary>
        /// If this button is no longer touched, sends the remote reset signal.
        /// </summary>
        /// <param name="col">the collision</param>
        private void OnCollisionExit2D(Collision2D col)
        {
            if (!CheckTags(col)) return;
            _collisions--;
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
            _timeSinceContactInterruption = Mathf.Infinity;
        }
    }
}
