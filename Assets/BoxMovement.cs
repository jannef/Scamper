using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxMovement : MonoBehaviour {
	public boxmovementclass _movementClass;

	void Awake() {
		_movementClass = new BasicUpAndDownBox(this);
	}

	void OnCollisionEnter2D(Collision2D col) {
		var colYLoc = _movementClass._transform.position.y - col.transform.position.y;

		if (colYLoc > 0) {
			_movementClass.OnBelowColl (col.collider.gameObject.tag, col.collider.gameObject.GetComponent<BoxMovement> ()._movementClass._speed);
		} else {
			_movementClass.OnTopColl (col.collider.gameObject.tag, col.collider.gameObject.GetComponent<BoxMovement> ()._movementClass._speed);
		}
	}

	void Update() {
		_movementClass.MoveBlock ();
	}

	private void OnMouseDown() {
		_movementClass.OnBoxClicked ();
	}

}
