using UnityEngine;

namespace fi.tamk.game.theone
{
    public class AcceleratingUpDownbox : BasicUpDownBox
    {
        public float MaxVelocity;
        public float AccelerationUp;
        public float AccelerationDown;

        public AcceleratingUpDownbox(BoxMovement master, float aDown = 1f, float aUp = -0.8f) : base(master)
        {
            AccelerationDown = aDown;
            AccelerationUp = aUp;
            _speed = 0.1f;
        }

        public override void MoveBlock()
        {
            _transform.Translate(0, _speed * SceneManager.Instance.DeltaTime, 0);

            if (_speed > 0)
            {
                _speed += SceneManager.Instance.DeltaTime * AccelerationUp;
            } else if (_speed < 0)
            {
                _speed -= SceneManager.Instance.DeltaTime * AccelerationDown;
            }
        }
    }
}
