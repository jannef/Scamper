﻿using UnityEngine;
using System.Collections.Generic;
using fi.tamk.game.theone.shader;

namespace fi.tamk.game.theone.phys
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PGameBlock : MonoBehaviour
    {
        public enum OnBoxClickAction { Impulse, ReverseGravity }

        /**
         * List of bodies and collision data
         */
        protected Dictionary<GameObject, Collision2D> _touchList;

        #region States
        /**
         *  Object is only usable once, before unlocking from some event.
         */
        public bool LockAfterUse = false;

        /**
         * Action taken when clicked.
         */
        public OnBoxClickAction OnClickAction = OnBoxClickAction.Impulse;

        /**
         * Force on click, if OnBoxClickAction.Impulsion
         */
        public Vector2 ForceOnClick = new Vector2(0, 7.8f);

        /**
         * Should inertia be dampened when colliding with ground.
         * 
         * Buggy as fuck.
         */
        public bool DampenInertia = false;
        private DesaturationShaderController _shader = null;

        /**
         * Player temporarily can't interact with this object.
         */
        public bool LockedFromPlayer = false;
        #endregion

        #region ResetPositionValues
        protected Vector3 _startLocation;
        protected Transform _transform;
        protected Rigidbody2D _rb;
        protected float _originalGravity;
        protected float _originalRotation;
        #endregion

        #region InteractableChecking
        virtual public bool IsResting()
        {
            foreach (var t in _touchList)
            {
                var vecY = t.Key.transform.position.y - transform.position.y;

                if (vecY != 0 && SceneManager.Instance.GameObjectMap[t.Key].GravityUp() == GravityUp() && !IsPositive(vecY) == GravityUp())
                {
                    return true;
                }
            }

            // didn't find solid ground below
            return false;
        }

        protected static bool IsPositive(float number)
        {
            if (number > 0)
            {
                return true;
            }

            return false;
        }

        public bool IsTopmost()
        {
            foreach (var t in _touchList)
            {
                // positive y is touch is above
                var vec = t.Value.contacts[0].point - new Vector2(_transform.position.x, _transform.position.y);

                if (!SceneManager.Instance.GameObjectMap[t.Key].CompareTag("Movable") || Mathf.Abs(vec.x) > Mathf.Abs(vec.y))
                {
                    continue;
                }
                else
                {
                    Debug.Log(GravityUp() + " " + vec.y);
                    if ((GravityUp() && vec.y > 0) || (!GravityUp() && vec.y <= 0))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        #endregion

        #region MonoBehaviourMethods
        void Start()
        {
            SceneManager.Instance.GameObjectMap.Add(gameObject, this);
            _shader = GetComponent<DesaturationShaderController>();
            _shader.Fade = 1;

            _touchList = new Dictionary<GameObject, Collision2D>();
            _transform = transform;
            _startLocation = _transform.position;
            _rb = GetComponent<Rigidbody2D>();
            _originalGravity = _rb.gravityScale;
            _originalRotation = _rb.rotation;

            OnStart();
        }

        void OnMouseDown()
        {
            if (!LockedFromPlayer && !SceneManager.Instance.Pause && IsResting() && IsTopmost())
            {
                if (LockAfterUse) LockedFromPlayer = true;
                switch (OnClickAction)
                {
                    case OnBoxClickAction.ReverseGravity:
                        _rb.gravityScale *= -1f;
                        break;
                    case OnBoxClickAction.Impulse:
                    default:
                        _rb.AddForce(ForceOnClick, ForceMode2D.Impulse);
                        break;
                }
            }
        }
        #endregion

        /**
         * Adds colliding object and the collision _touchList
         */
        void OnCollisionEnter2D(Collision2D col)
        {
            _touchList.Add(col.collider.gameObject, col);

            if (IsResting())
            {
                if (DampenInertia) _rb.velocity = Vector2.zero;
            }
        }

        /**
         * Returns true if local gravity is towards top.
         */
        private bool GravityUp()
        {
            if (_rb.gravityScale >= 0) return true;
            return false;
        }

        /**
         * Removes exiting collider from _touchList
         */
        void OnCollisionExit2D(Collision2D col)
        {
            _touchList.Remove(col.collider.gameObject);
        }

        /**
         * Called from Start(). For inheriting types to use so they don't accidentally left out
         * important stuff in PGameBlock.Start().
         */
        virtual protected void OnStart()
        {

        }

        /**
         * Returns block to its initial position, speed and rotation etc. Inherited types should
         * reset all behaviour to start values.
         */
        virtual public void ResetBlock()
        {
            _rb.gravityScale = _originalGravity;
            _rb.velocity = Vector2.zero;
            _transform.position = _startLocation;
            _rb.rotation = _originalRotation;
        }

        /**
         * Sets gravity scale of related rigidbody2d.
         */
        public void SetGravity(float newGravity)
        {
            _rb.gravityScale = newGravity;
        }

        public void SetFade(float ratio)
        {
            if (ratio < 0f || ratio > 1.0f) return;
            if (_shader != null)
            {
                _shader.Fade = ratio;
            }
        }
    }
}
