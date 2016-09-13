using UnityEngine;
using System.Collections;

public class BasicUpAndDownBox : boxmovementclass {

	public BasicUpAndDownBox (MonoBehaviour master) : base(master) {
		
	}

	public override void MoveBlock() {
		_transform.Translate (0, _speed * Time.deltaTime, 0);
	}

	public override void OnTopColl(string tag, float speed) {
		_speed *= -1;
	}

	public override void OnBelowColl(string tag, float speed) {
		if (tag.Equals ("Movable") && speed != 0) {
			_speed *= -1;
		} else {
			_speed = 0;
		}
	}

	public override void OnBoxClicked() {
		if (_speed == 0) {
			_speed = 2;
		}
	}
}
