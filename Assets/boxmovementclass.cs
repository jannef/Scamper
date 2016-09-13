using UnityEngine;
using System.Collections;

public class boxmovementclass {
	public Transform _transform;
	public float _speed = 2.0f;
	protected MonoBehaviour _master;

	public boxmovementclass (MonoBehaviour master) {
		_master = master;
		_transform = _master.transform;
	}

	void Awake() {
		_transform = _master.transform;
	}



	public virtual void OnBelowColl (string tag, float speed) {
	}

	public virtual void OnTopColl(string tag, float speed) {
	}

	void Update() {
		MoveBlock ();
	}

	public virtual void MoveBlock() {

	}

	private void OnMouseDown() {
		OnBoxClicked ();
	}

	public virtual void OnBoxClicked() {
	}
}
