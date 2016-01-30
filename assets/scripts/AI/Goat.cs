using UnityEngine;
using System.Collections;

public class Goat: MonoBehaviour {

	bool shouldChangeState = false;
	float stateStartTime;
	float timeInState {
		get {
			return Time.time - stateStartTime;
		}
	}

	enum State {
		Idle,
		Left,
		Right,
	}
	State state;

	bool hasLeft;
	bool hasRight;

	void Start() {
		enterState(State.Idle);
	}

	// Update is called once per frame
	void Update() {
		// See if we have ground to the left or right of us.
		var bounds = GetComponent<Collider2D>().bounds;
		var extents = bounds.extents;
		var left = bounds.min - extents;
		var right = new Vector3(bounds.max.x + extents.x, bounds.min.y - extents.y);
		hasLeft = checkBounds(left, extents);
		hasRight = checkBounds(right, extents);
		// Now behave.
		continueState();
	}

	private bool checkBounds(Vector3 position, Vector3 extents) {
		var hits = Physics2D.OverlapAreaAll(position - extents, position + extents);
		var any = false;
		foreach (var hit in hits) {
			if (hit.gameObject == gameObject) continue;
			any = true;
		}
		if (any) {
			Debug.DrawLine(position - extents, position + extents, Color.red, 0, false);
		}
		return any;
	}

	void continueState() {
		// Check based on deltaTime to be fair, and checking once per update
		// gives exponential fall off, which is nice.
		shouldChangeState = Random.value < Time.deltaTime;
		switch (state) {
			case State.Left:
				go(-1);
				break;
			case State.Right:
				go(1);
				break;
			case State.Idle:
				// Do nothing for now.
				if (shouldChangeState) {
					enterState(Random.value < 0.5 ? State.Left : State.Right);
				}
				break;
		}
	}

	void enterState(State state) {
		stateStartTime = Time.time;
		this.state = state;
	}

	void go(float dir) {
		//
	}

}
