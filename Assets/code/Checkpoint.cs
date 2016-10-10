using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    public enum CameraTransitionInterpolations
    {
        Linear,
        EaseOut,
        EaseIn,
        Exponential,
        Smoothstep,
        Smootherstep
    }

    public class Checkpoint : MonoBehaviour
    {
        public CameraTransitionInterpolations TransitionInterpolation = CameraTransitionInterpolations.Linear;

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

        void Update()
        {
            if (_activeTransition)
            {
                _transitionTimer = Mathf.Min(_transitionTimer + SceneManager.Instance.DeltaTime, CameraTransitionTime);
                var lerpRatio = _transitionTimer / CameraTransitionTime;

                switch (TransitionInterpolation)
                {
                    case CameraTransitionInterpolations.EaseIn:
                        lerpRatio = Mathf.Cos(lerpRatio * Mathf.PI * 0.5f);
                        break;
                    case CameraTransitionInterpolations.EaseOut:
                        lerpRatio = Mathf.Sin(lerpRatio * Mathf.PI * 0.5f);
                        break;
                    case CameraTransitionInterpolations.Exponential:
                        lerpRatio = Mathf.Pow(lerpRatio, 2f);
                        break;
                    case CameraTransitionInterpolations.Smoothstep:
                        lerpRatio = Mathf.Pow(lerpRatio, 2f) * (3f - (2f * lerpRatio));
                        break;
                    case CameraTransitionInterpolations.Smootherstep:
                        lerpRatio = Mathf.Pow(lerpRatio, 3f) * (lerpRatio * (6f * lerpRatio - 15f) + 10f);
                        break;
                    default:
                        break;

                }

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
