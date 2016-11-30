using System;
using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    /// <summary>
    /// Remote leaver activator.
    /// </summary>
    /// <auth>Janne Forsell</auth>
    public class RemoteActivatorLever : RemoteBase
    {
        /// <summary>
        /// Angle of the leaver at the moment.
        /// </summary>
        public float LeverAngle
        {
            get {return _hinge.jointAngle; }
        }

        /// <summary>
        /// Reference to hinge joint.
        /// </summary>
        [SerializeField] private HingeJoint2D _hinge = null;

        /// <summary>
        /// Lower limit of the activation angle.
        /// </summary>
        [SerializeField] private float LowerActivationLimit = 0;

        /// <summary>
        /// Upper limti of the activation angle.
        /// </summary>
        [SerializeField] private float UpperActivationLimit = 0;

        /// <summary>
        /// Reference to sprite renderer of the lever.
        /// </summary>
        [SerializeField] private SpriteRenderer SpriteRenderer;

        /// <summary>
        /// Sprite to switch to when the lever is pushed.
        /// </summary>
        [SerializeField] private Sprite PushedStateSprite;

        /// <summary>
        /// Has this lever been activated already.
        /// </summary>
        private bool _hasActivated = false;

        /// <summary>
        /// Reference to original sprite for resetting.
        /// </summary>
        private Sprite _originalSprite = null;

        /// <summary>
        /// Get reference to original sprite and subscribe to reset event.
        /// </summary>
        private void Awake()
        {
            _originalSprite = SpriteRenderer.sprite;
            SceneManager.Instance.LevelResetEvent += ResetLever;
        }

        /// <summary>
        /// If this hasn't activated, listen to angle to trigger activation.
        /// </summary>
        private void Update()
        {
            if (_hasActivated || LeverAngle > UpperActivationLimit || LeverAngle < LowerActivationLimit) return;
            _hasActivated = true;
            ChangeToSprite(PushedStateSprite);

            ActivateBlocks();
        }

        /// <summary>
        /// Change sprite to given sprite.
        /// </summary>
        /// <param name="toWhich">Spriteto change to.</param>
        private void ChangeToSprite(Sprite toWhich)
        {
            if (SpriteRenderer != null && toWhich != null) SpriteRenderer.sprite = toWhich;
        }


        /// <summary>
        /// Reset this lever.
        /// </summary>
        private void ResetLever()
        {
            ChangeToSprite(_originalSprite);
            if (_hasActivated) DeactivateBlocks();
            _hasActivated = false;
        }
    }
}
