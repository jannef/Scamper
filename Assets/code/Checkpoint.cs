﻿using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    public class Checkpoint : MonoBehaviour
    {
        public Transform CameraPosition;
        public Transform Spawn;
        public float CameraTransitionTime = 2f;

        private bool _activeTransition = false;
        private bool _hasFired = false;
        private float _transitionTimer = 0f;

        private Transform _cameraTransform;
        private Transform _playerTransform;

        private Vector3 _cameraBeginPosition;
        private Vector3 _playerBeginPosition;
        private Vector3 _playerTargetPosition;

        void Awake()
        {
            _cameraTransform = Camera.main.gameObject.transform;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!_hasFired && other.gameObject.CompareTag("Player"))
            {
                _activeTransition = true;
                _hasFired = true;
                _cameraBeginPosition = _cameraTransform.position;
                _playerTransform = other.gameObject.transform;
                _playerBeginPosition = _playerTransform.position;
                _playerTargetPosition = new Vector3(Spawn.transform.position.x, _playerBeginPosition.y, _playerBeginPosition.z);

                ((PPlayerBlock)SceneManager.Instance.GameObjectMap[other.gameObject]).Checkpoint(this);
            }
        }

        void Update()
        {
            if (_activeTransition)
            {
                _transitionTimer = Mathf.Min(_transitionTimer + SceneManager.Instance.DeltaTime, CameraTransitionTime);
                var lerpRatio = _transitionTimer / CameraTransitionTime;

                _cameraTransform.position = Vector3.Lerp(_cameraBeginPosition, CameraPosition.position, lerpRatio);
                _playerTransform.position = Vector3.Lerp(_playerBeginPosition, _playerTargetPosition, lerpRatio);

                if (_transitionTimer >= CameraTransitionTime) _activeTransition = false;
            }
        }
    }
}
