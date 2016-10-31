﻿using UnityEngine;
using System.Collections;
using fi.tamk.game.theone.utils;

namespace fi.tamk.game.theone.phys
{

    public class Checkpoint : MonoBehaviour
    {
        public InterpolationType CameraInterpolation = InterpolationType.Smootherstep;

        /**
         * Position to move camera into.
         * 
         * Checkpoint prefab has one automatically set, but if doing stuff manually,
         * make sure to set one in game editor.
         */
        public Transform CameraPosition;

        /**
         * Position to spawn player into, if he dies before reachign another checkpoint.
         * 
         * Checkpoint prefab has one automatically set, but if doing stuff manually,
         * make sure to set one in game editor.
         */
        public Transform Spawn;

        /**
         * How long should the transition of panning camera and player take.
         * 
         * Player is moved to same location he would respawn into while camera in panned.
         */
        public float CameraTransitionTime = 2f;

        /**
         * How large should the camera wievport be for this checkpoints "level".
         */
        public float NewWieportSize = 5f;

        #region TransitioHelperVariables
        private bool _activeTransition = false;
        private bool _hasFired = false;
        private float _transitionTimer = 0f;
        private float _oldSize;

        private Transform _cameraTransform;
        private Transform _playerTransform;
        private Camera _camera;

        private Vector3 _cameraBeginPosition;
        private Vector3 _playerBeginPosition;
        private Vector3 _playerTargetPosition;

        private PPlayerBlock _player;
        #endregion

        void Awake()
        {
            _cameraTransform = Camera.main.gameObject.transform;
            _camera = Camera.main;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!_hasFired && other.gameObject.CompareTag("Player"))
            {
                _oldSize =_camera.orthographicSize;
                _activeTransition = true;
                _hasFired = true;
                _cameraBeginPosition = _cameraTransform.position;
                _playerTransform = other.gameObject.transform;
                _playerBeginPosition = _playerTransform.position;
                _playerTargetPosition = new Vector3(Spawn.transform.position.x, _playerBeginPosition.y, _playerBeginPosition.z);

                _player = ((PPlayerBlock)SceneManager.Instance.GameObjectMap[other.gameObject]);
                _player.Checkpoint(this);
            }
        }

        void LateUpdate()
        {
            if (_activeTransition)
            {
                _transitionTimer = Mathf.Min(_transitionTimer + SceneManager.Instance.DeltaTime, CameraTransitionTime);
                var lerpRatio = _transitionTimer / CameraTransitionTime;

                lerpRatio = Interpolations.Interpolation(lerpRatio, CameraInterpolation);

                _cameraTransform.position = Vector3.Lerp(_cameraBeginPosition, CameraPosition.position, lerpRatio);
                _playerTransform.position = Vector3.Lerp(_playerBeginPosition, _playerTargetPosition, lerpRatio);
                _camera.orthographicSize = Mathf.Lerp(_oldSize, NewWieportSize/2f, lerpRatio);

                if (_transitionTimer >= CameraTransitionTime)
                {
                    _activeTransition = false;
                    _player.CheckpointRelease();
                }
            }
        }
    }
}
