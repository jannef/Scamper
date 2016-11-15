using UnityEngine;
using System.Collections;
using fi.tamk.game.theone.utils;

namespace fi.tamk.game.theone.phys
{
    /// <summary>
    /// Check point instance. Saves player progress and moves camera when reached.
    /// </summary>
    public class Checkpoint : MonoBehaviour
    {
        /// <summary>
        /// Automatic camera movement interpolation type. Default value in pretty good.
        /// </summary>
        public InterpolationType CameraInterpolation = InterpolationType.Smootherstep;

        /// <summary>
        /// Where the camera should move once this checkpoint is reached.
        /// </summary>
        public Transform CameraPosition;

        /// <summary>
        /// Where the player should spawn on death after this checkpoint is reached.
        /// </summary>
        public Transform Spawn;

        /// <summary>
        /// How long should a camera transition take.
        /// </summary>
        public float CameraTransitionTime = 2f;

        /// <summary>
        /// How large should a new camera viewport be.
        /// </summary>
        public float NewWieportSize = 5f;

        /// <summary>
        /// Is a camera transition in effect.
        /// </summary>
        private bool _activeTransition = false;

        /// <summary>
        /// Has this checkpoint already fired once. Prevents from firing again on player death reset.
        /// </summary>
        private bool _hasFired = false;

        /// <summary>
        /// How much of the transition has elapsed.
        /// </summary>
        private float _transitionTimer = 0f;

        /// <summary>
        /// How large camera was previously.
        /// </summary>
        private float _oldSize;

        /// <summary>
        /// Cached transform of main camera.
        /// </summary>
        private Transform _cameraTransform;

        /// <summary>
        /// Chached transform of player rat.
        /// </summary>
        private Transform _playerTransform;

        /// <summary>
        /// Reference to main camera.
        /// </summary>
        private Camera _camera;

        /// <summary>
        /// Whete the camera was previously.
        /// </summary>
        private Vector3 _cameraBeginPosition;

        /// <summary>
        /// Where the player was previously.
        /// </summary>
        private Vector3 _playerBeginPosition;

        /// <summary>
        /// Where the player should end up.
        /// </summary>
        private Vector3 _playerTargetPosition;

        /// <summary>
        /// Reference to the logical player game object.
        /// </summary>
        private PPlayerBlock _player;

        /// <summary>
        /// Finds references.
        /// </summary>
        void Awake()
        {
            _cameraTransform = Camera.main.gameObject.transform;
            _camera = Camera.main;
        }

        /// <summary>
        /// Activates the checkpoint.
        /// </summary>
        /// <param name="other">NOt used.</param>
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

        /// <summary>
        /// Moves camera and player if transition is in progress.
        /// </summary>
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
