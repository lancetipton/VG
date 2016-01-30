﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class CharController : MonoBehaviour {
	#region Public Properties
	public float runSpeed = 4;
	public float acceleration = 20;
	public float jumpSpeed = 5;
	public float gravity = 15;
	public Vector2 influence = new Vector2(5, 5);
	public AudioClip[] sounds;
	public UnityEngine.UI.Text debugText;
	public LayerMask groundLayers;
	public GameObject hitCollider;
	
	#endregion
	//--------------------------------------------------------------------------------
	#region Private Properties
	Animator animator;
	AudioSource audioSource;
	Vector3 defaultScale;
	bool grounded;
	float stateStartTime;
	
	float timeInState {
		get { return Time.time - stateStartTime; }
	}

	const string kIdleAnim = "Idle";
	const string kRunAnim = "Run";
	const string kJumpStartAnim = "JumpStart";
	const string kJumpFallAnim = "JumpFall";
	const string kJumpLandAnim = "JumpLand";
	const string kWeakHitAnim = "WeakHit";
	const string kStrongHitAnim = "StrongHit";
	
	enum State {
		Idle,
		RunningRight,
		RunningLeft,
		JumpingUp,
		JumpingDown,
		Landing,
		WeakHitting,
		StrongHitting
	}
	State state;
	Vector2 velocity;
	float horzInput;
	bool jumpJustPressed;
	bool jumpHeld;
	bool weakHitPressed;
	bool strongHitPressed;
	
	int airJumpsDone = 0;
	Rigidbody2D rbody;
	
	#endregion
	//--------------------------------------------------------------------------------
	#region MonoBehaviour Events
	void Start() {
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		rbody = GetComponent<Rigidbody2D>();
		defaultScale = transform.localScale;
		rbody.isKinematic = false;
	}
	
	void Update() {
		// Gather inputs
		horzInput = Input.GetAxisRaw("Horizontal");
		jumpJustPressed = Input.GetButtonDown("Jump");
		jumpHeld = Input.GetButton("Jump");
		weakHitPressed = Input.GetButtonDown("WeakHit");
		strongHitPressed = Input.GetButtonDown("StrongHit");
		
		// Update state
		ContinueState();
		
		// Debugging
		debugText.text = "State: " + state + " (" + timeInState + ")"
			+ "\ngrounded: " + grounded
			+ "\nvelocity: " + rbody.velocity;
	}
	
	void LateUpdate() {
		UpdatePhysics();
	}
	
	#endregion
	//--------------------------------------------------------------------------------
	#region Public Methods

	public void PlaySound(string name) {
		if (!audioSource.enabled) return;
		foreach (AudioClip clip in sounds) {
			if (clip.name == name) {
				audioSource.clip = clip;
				audioSource.Play();
				return;
			}
		}
		Debug.LogWarning(gameObject + ": AudioClip not found: " + name);
	}
	
	public void HitObject(GameObject target) {
		Vector2 force = transform.TransformDirection(Vector3.right);
		target.GetComponent<Rigidbody2D>().AddForceAtPosition(force, 
			hitCollider.transform.position, ForceMode2D.Impulse);
	}
	
	#endregion
	//--------------------------------------------------------------------------------
	#region Private Methods
	void SetOrKeepState(State state) {
		if (this.state == state) return;
		EnterState(state);
	}
	
	void ExitState() {
	}
	
	void EnterState(State state) {
		ExitState();
		switch (state) {
		case State.Idle:
			animator.Play(kIdleAnim);
			break;
		case State.RunningLeft:
			animator.Play(kRunAnim);
			Face(-1);
			break;
		case State.RunningRight:
			animator.Play(kRunAnim);
			Face(1);
			break;
		case State.JumpingUp:
			animator.Play(kJumpStartAnim);
			rbody.AddRelativeForce(new Vector2(0f, jumpSpeed), ForceMode2D.Impulse);
			break;
		case State.JumpingDown:
			animator.Play(kJumpFallAnim);
			break;
		case State.Landing:
			animator.Play(kJumpLandAnim);
			airJumpsDone = 0;
			break;
		case State.WeakHitting:
			animator.Play(kWeakHitAnim);
			break;
		case State.StrongHitting:
			animator.Play(kStrongHitAnim);
			break;
		}
		
		this.state = state;
		stateStartTime = Time.time;
	}
	
	void ContinueState() {
		switch (state) {
			
		case State.Idle:
			RunOrJump();
			break;
			
		case State.RunningLeft:
		case State.RunningRight:
			if (!RunOrJump()) EnterState(State.Idle);
			break;
			
		case State.JumpingUp:
			if (velocity.y < 0) EnterState(State.JumpingDown);
			if (jumpJustPressed && airJumpsDone < 1) {
				EnterState(State.JumpingUp);
				airJumpsDone++;
			}
			break;
			
		case State.JumpingDown:
			if (grounded) EnterState(State.Landing);
			if (jumpJustPressed && airJumpsDone < 1) {
				EnterState(State.JumpingUp);
				airJumpsDone++;
			}
			break;
			
		case State.Landing:
			if (timeInState > 0.2f) EnterState(State.Idle);
			else if (timeInState > 0.1f) RunOrJump();
			break;
			
		case State.WeakHitting:
			hitCollider.SetActive(timeInState > 0.05f && timeInState < 0.15f);
			if (timeInState > 0.2f) EnterState(State.Idle);
			break;
			
		case State.StrongHitting:
			hitCollider.SetActive(timeInState > 0.15f && timeInState < 0.33f);
			if (timeInState > 0.4f) EnterState(State.Idle);
			break;
			
		}
	}
	
	bool RunOrJump() {
		if (jumpJustPressed && grounded) SetOrKeepState(State.JumpingUp);
		else if (weakHitPressed && grounded) SetOrKeepState(State.WeakHitting);
		else if (strongHitPressed && grounded) SetOrKeepState(State.StrongHitting);
		else if (horzInput < 0) SetOrKeepState(State.RunningLeft);
		else if (horzInput > 0) SetOrKeepState(State.RunningRight);
		else return false;
		return true;
	}
	
	
	void Face(int direction) {
		transform.localScale = new Vector3(defaultScale.x * direction, defaultScale.y, defaultScale.z);
	}
	
	void UpdatePhysics() {
		if (grounded) {
			float targetSpeed = 0;
			switch (state) {
			case State.RunningLeft:
				targetSpeed = -runSpeed;
				break;
			case State.RunningRight:
				targetSpeed = runSpeed;
				break;
			}
			velocity.x = Mathf.MoveTowards(velocity.x, targetSpeed, acceleration * Time.deltaTime);
			velocity.y = rbody.velocity.y;
			rbody.velocity = velocity;
		} else {
			// vertical influence affects gravity scale
			if (jumpHeld) rbody.gravityScale = 1 - influence.y;
			else rbody.gravityScale = 1;
			
			// horizontal influence is an acceleration towards the target speed
			// (just like when running, but the acceleration should be much lower)
			float targetSpeed = horzInput * runSpeed;
			velocity.x = Mathf.MoveTowards(velocity.x, targetSpeed, influence.x * Time.deltaTime);
			velocity.y = rbody.velocity.y;
			rbody.velocity = velocity;			
		}
		
		// Figure out whether we're grounded
		
		// Front edge
		float depth = 0.3f;
		Vector3 foot = transform.TransformPoint(new Vector3(0.15f, 0.1f, 0));
		bool hit = Physics2D.Raycast(foot, Vector2.down, depth, groundLayers);
		Debug.DrawLine(foot, foot + Vector3.down * depth, hit ? Color.red : Color.green, 0, false);
		grounded = hit;
		
		// Rear edge
		foot = transform.TransformPoint(new Vector3(-0.12f, 0.1f, 0));
		hit = Physics2D.Raycast(foot, Vector2.down, depth, groundLayers);
		Debug.DrawLine(foot, foot + Vector3.down * depth, hit ? Color.red : Color.green, 0, false);
		grounded = grounded || hit;
	}
	#endregion
}