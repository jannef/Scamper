﻿using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using fi.tamk.game.theone.shader;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    /// <summary>
    /// Base class taht contains functionality of the logical gameObjects that interact with eachother
    /// in the game world. This class offers collision detection related methods and resetting to original
    /// position.
    /// </summary>
    /// <auth>Janne Forsell</auth>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PGameBlock : RemoteBehaviour
    {	

        /// <summary>
        /// Types of behaviours for objects on interaction.
        /// </summary>
        public enum OnBoxClickAction { Impulse, ReverseGravity, None }

        public enum OnRemoteActivationAction { Unlock, Impulse, ReverseGravity, None }

        public event SceneEvent BlockClickedEvent;

        public AudioClip telekinesisSound;

        private AudioSource source;

        private const float telekinesisVolume = 0.7f;
        
        /// <summary>
        /// Stores data of collisions that are ongoing. This info is kept by
        /// Box2d, but user cannot access with implementation used by unity.
        /// </summary>
        protected Dictionary<GameObject, Collision2D> TouchList;

        /// <summary>
        /// Should the object be locked from the player after one use.
        /// </summary>
        public bool LockAfterUse = false;

        /// <summary>
        /// If remote activation is active.
        /// </summary>
        private bool _remotelyActivated = false;

        /// <summary>
        /// Which action should this object take when clicked by the player.
        /// </summary>
        public OnBoxClickAction OnClickAction = OnBoxClickAction.Impulse;

        /// <summary>
        /// Which action should this object perform when remotely activating.
        /// </summary>
        public OnRemoteActivationAction OnRemoteAction = OnRemoteActivationAction.None;

        /// <summary>
        /// Amounth of force to be applied to this on click.
        /// </summary>
        public Vector2 ForceOnClick = new Vector2(0, 7.8f);

        /// <summary>
        /// Should dampen the inertia when colliding with solid ground...
        /// 
        /// TODO: Buggy as fuck, please avoid setting to true.
        /// </summary>
        public bool DampenInertia = false;

        /// <summary>
        /// This object is locked from player interaction while this is true;
        /// </summary>
        public bool LockedFromPlayer = false;

        /// <summary>
        /// Starting position of this object for resetting purposes.
        /// </summary>
        protected Vector3 StartLocation;

        /// <summary>
        /// Reference to this' gameObject's Transfrom. Cached for efficiency, depracated
        /// and not wort using in later versions of unity.
        /// </summary>
        protected Transform MyTransform;

        /// <summary>
        /// Cached reference to rigidbody attached to gameObjects owning this.
        /// </summary>
        protected Rigidbody2D Rb;

        /// <summary>
        /// Original Gravity for resetting purposes.
        /// </summary>
        protected float OriginalGravity;

        /// <summary>
        /// Orifinal rotation for resetting purposes.
        /// </summary>
        protected float OriginalRotation;

        /// <summary>
        /// Stores original LockedFromPlayer status for resets.
        /// </summary>
        protected bool _originalLockStatus;

        /// <summary>
        /// True if remote deactivation is in progress.
        /// </summary>
        protected bool _remoteDeactivationInProgress = false;

        /// <summary>
        /// Finds out if this object can be considered resting on something that is considered always to be in rest or on top of something
        /// that can find solid ground below calling this same method recursively.
        /// 
        /// We are not using simple rigidbody2d.velocity.magnitude~0 test, becasue that way we cannot have mobile platforms supporting
        /// interactable PGameBlocks on top of them.
        /// </summary>
        /// <returns>True if this object is resting on a surface.</returns>
        public virtual bool IsResting()
        {
            // Have you already taken our lord Linqus Cristus as your personal saviour?
            return (
                from t in TouchList
                let vecY = t.Key.transform.position.y - transform.position.y
                where Math.Abs(vecY) > 0.005f && SceneManager.Instance.GameObjectMap[t.Key].GravityDown() == GravityDown() && !(vecY > 0) == GravityDown()
                select t).Any();
        }

        /// <summary>
        /// Looks up the TouchList to figure out if another object is resting on top of this one.
        /// </summary>
        /// <returns>True if no object was found that is resting on this one.</returns>
        public bool IsTopmost()
        {
            foreach (var t in TouchList)
            {
                // positive y is touch is above
                var vec = t.Value.contacts[0].point - new Vector2(MyTransform.position.x, MyTransform.position.y);

                if (!SceneManager.Instance.GameObjectMap[t.Key].CompareTag("Movable") ||
                    Mathf.Abs(vec.x) > Mathf.Abs(vec.y)) continue;

                if ((GravityDown() && vec.y > 0) || (!GravityDown() && vec.y <= 0))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Overloaded MonoBehavior method performed on instantiation.
        /// </summary>
        private void Awake()
        {
            SceneManager.Instance.GameObjectMap.Add(gameObject, this);

            source = GetComponent<AudioSource>();

            TouchList = new Dictionary<GameObject, Collision2D>();
            MyTransform = transform;
            StartLocation = MyTransform.position;
            Rb = GetComponent<Rigidbody2D>();
            OriginalGravity = Rb.gravityScale;
            OriginalRotation = Rb.rotation;
            _originalLockStatus = LockedFromPlayer;

            SceneManager.Instance.LevelResetEvent += ResetBlock;

            OnAwake();
        }

        /// <summary>
        /// Box2d event system method executed when this gameobject is clicked with mouse or
        /// tapped on touch screen.
        /// </summary>
        private void OnMouseDown()
        {
            if (LockedFromPlayer || _remotelyActivated || SceneManager.Instance.Pause || !IsResting() || !IsTopmost()) return;
            if (LockAfterUse) LockedFromPlayer = true;
            if (BlockClickedEvent != null) BlockClickedEvent();

            SceneManager.Instance.BoxClicked();

            switch (OnClickAction)
            {
                case OnBoxClickAction.ReverseGravity:
                    Rb.gravityScale *= -1f;
                    SceneManager.Instance.PlayDistanceBasedSound(telekinesisSound, telekinesisVolume, transform.position);
                    //source.PlayOneShot(telekinesisSound, telekinesisVolume);
                    break;
                case OnBoxClickAction.Impulse:
                    Rb.AddForce(ForceOnClick, ForceMode2D.Impulse);
                    SceneManager.Instance.PlayDistanceBasedSound(telekinesisSound, telekinesisVolume, transform.position);
                    //source.PlayOneShot(telekinesisSound, telekinesisVolume);
                    break;
                case OnBoxClickAction.None:
                    break;
            }
        }

        /// <summary>
        /// Handles remote activation of this block by switch, button or such.
        /// </summary>
        public override void OnRemoteActivation()
        {
            if (_remotelyActivated) return;
            _remotelyActivated = true;

            switch(OnRemoteAction)
            {
                case OnRemoteActivationAction.Unlock:
                    LockedFromPlayer = false;
                    break;
                case OnRemoteActivationAction.Impulse:
                    if (LockedFromPlayer || SceneManager.Instance.Pause || !IsResting() || !IsTopmost())
                    {
                        _remotelyActivated = false;
                        return;
                    }
                    Rb.AddForce(ForceOnClick, ForceMode2D.Impulse);
                    break;
                case OnRemoteActivationAction.ReverseGravity:
                    Rb.gravityScale *= -1f;
                    break;
                case OnRemoteActivationAction.None:
                    break;
            }
        }

        /// <summary>
        /// Resets state of the block after remote activation is finished.
        /// </summary>
        public override void OnRemoteActivationActionReset()
        {
            switch (OnRemoteAction)
            {
                case OnRemoteActivationAction.Unlock:
                    LockedFromPlayer = true;
                    break;
                case OnRemoteActivationAction.ReverseGravity:
                    Rb.gravityScale *= -1f;
                    break;
                case OnRemoteActivationAction.Impulse:
                    break;
                case OnRemoteActivationAction.None:
                    break;
            }

            _remotelyActivated = false;
        }

        /// <summary>
        /// Box2d event that is triggered when a collision with another Box2d rigidbody
        /// begins.
        /// 
        /// Collisiondata is stored in TouchList since we want to avoid catching
        /// OnCollisionStay2d events every physics update - we are fine by just storing
        /// the data and polling on demand.
        /// 
        /// There is a potential bug if two colliders adhert to eachother midair and then
        /// continue to move together. Nasty stuff will happen, demonstrated by turning
        /// dampen inertia on and getting two boxes to stick to eachother.
        /// </summary>
        /// <param name="col">Collision data passed from Box2d ohysics engine</param>
        private void OnCollisionEnter2D(Collision2D col)
        {
            // Player collisions can be ignored. Derived player class implements those.
            if (col.collider.gameObject.CompareTag("Player")) return;
            TouchList.Add(col.collider.gameObject, col);

            if (!IsResting()) return;
            if (DampenInertia) Rb.velocity = Vector2.zero;
        }

        /// <summary>
        /// Figures out where gravity is pulling this object in x-axis.
        /// Everythign will probably break if you change global gravity direction
        /// from project settings.
        /// </summary>
        /// <returns>True if local gravity points down.</returns>
        private bool GravityDown()
        {
            return Rb.gravityScale >= 0;
        }

        /// <summary>
        /// Removes stored collision involving the collider that is now triggering
        /// Box2d's OnCollisionExit event.
        /// </summary>
        /// <param name="col"></param>
        private void OnCollisionExit2D(Collision2D col)
        {
            TouchList.Remove(col.collider.gameObject);
        }

        /// <summary>
        /// Called at Awake(), for inherited classes to perform additional initialization
        /// with reduced risk of not calling parents initialization function. This must be
        /// done so, because we cannot enforce initialization by cosntructor in Unity.
        /// 
        /// User can always fuck things up by hiding Start() using 'new' keyword, but that's
        /// something that can't be helped.
        /// </summary>
        protected virtual void OnAwake()
        {
			
        }

        /// <summary>
        /// Resets this gameObject to it's original status to achieve level reset.
        /// </summary>
        public virtual void ResetBlock()
        {
            StopAllCoroutines();

            // Only reset gravity here if it was reversed by clicking!
            if (OnClickAction == OnBoxClickAction.ReverseGravity )Rb.gravityScale = OriginalGravity;

            _remotelyActivated = false;
            _remoteDeactivationInProgress = false;
            
            Rb.velocity = Vector2.zero;
            MyTransform.position = StartLocation;
            Rb.rotation = OriginalRotation;

            LockedFromPlayer = _originalLockStatus;
        }

        /// <summary>
        /// Setter for this' gameObject's rigidbody-component's gravity scale.
        /// </summary>
        /// <param name="newGravity">Gravityscale to set. Local gravity is 'float * vector2'.</param>
        public void SetGravity(float newGravity)
        {
            Rb.gravityScale = newGravity;
        }
    }
}
