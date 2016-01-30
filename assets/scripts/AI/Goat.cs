﻿using UnityEngine;

public class Goat: MonoBehaviour {

	public float walkSpeed = 0.7f;
	public float minStateTime = 0.5f;

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
		// Put the center of gravity down a bit.
		var body = GetComponent<Rigidbody2D>();
		var bounds = GetComponent<Collider2D>().bounds;
		body.centerOfMass = new Vector2(0, -bounds.extents.y / 2);
	}

	// Update is called once per frame
	void Update() {
		// See if we have ground to the left or right of us.
		var bounds = GetComponent<Collider2D>().bounds;
		// Use our own size, but be depth shy (y / 2).
		var extents = new Vector3(bounds.extents.x, bounds.extents.y / 2);
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
		float chance = Time.deltaTime;
		float minStateTime = this.minStateTime;
		if (state == State.Idle) {
			// Stay put for longer than we move.
			chance /= 2;
			minStateTime *= 2;
		}
		shouldChangeState =
			timeInState > minStateTime && Random.value < chance;
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
					if (!hasLeft) {
						enterState(State.Right);
					} else if (!hasRight) {
						enterState(State.Left);
					} else {
						// Both are safe so pick randomly.
						enterState(Random.value < 0.5 ? State.Left : State.Right);
					}
				}
				setVelocity(0);
                break;
		}
	}

	void enterState(State state) {
		shouldChangeState = false;
		stateStartTime = Time.time;
		this.state = state;
	}

	void go(float dir) {
		// Check for cases to switch to idle.
		// Is Mathf.DeltaAngle always positive to begin with?
		var angle = GetComponent<Rigidbody2D>().rotation;
		var fallen = Mathf.Abs(Mathf.DeltaAngle(angle, 0)) > 30;
		var risky = dir < 0 && !hasLeft || dir > 0 && !hasRight;
		if (fallen || risky || shouldChangeState) {
			enterState(State.Idle);
		}
		setVelocity(dir * walkSpeed);
	}

	void setVelocity(float velocity) {
		var body = GetComponent<Rigidbody2D>();
		body.velocity = new Vector2(velocity, body.velocity.y);
	}

}
