using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class CharController : MonoBehaviour {
	#region Public Properties
	public int playerNum = 1;
	public float runSpeed = 4;
	public float acceleration = 20;
	public float jumpSpeed = 5;
	public float gravity = 15;
	public Vector2 influence = new Vector2(5, 0.5f);
	public AudioClip[] sounds;
	public UnityEngine.UI.Text debugText;
	public LayerMask groundLayers;
	public Striker striker;
	
	#endregion
	//--------------------------------------------------------------------------------
	#region Private Properties
	Animator animator;
	AudioSource audioSource;
	GoatGrabber grabber;
	Vector3 defaultScale;
	bool grounded;
	float stateStartTime;
	float lastGroundedTime;
	
	float timeInState {
		get { return Time.time - stateStartTime; }
	}
	
	float timeUngrounded {
		get { return Time.time - lastGroundedTime; }
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
		StrongHitting,
		KnockedBack
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
	float lastVertInput;
	
	#endregion
	//--------------------------------------------------------------------------------
	#region MonoBehaviour Events
	void Start() {
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		rbody = GetComponent<Rigidbody2D>();
		grabber = GetComponentInChildren<GoatGrabber>();
		defaultScale = transform.localScale;
		rbody.isKinematic = false;
	}
	
	void Update() {
		// Gather inputs
		string prefix = "Player " + playerNum + " ";
		horzInput = Input.GetAxisRaw(prefix + "Horizontal");
		jumpJustPressed = Input.GetButtonDown(prefix + "Jump");
		jumpHeld = Input.GetButton(prefix + "Jump");
		weakHitPressed = Input.GetButtonDown(prefix + "WeakHit");
		strongHitPressed = Input.GetButtonDown(prefix + "StrongHit");
		float vertInput = Input.GetAxisRaw(prefix + "Vertical");
		if (vertInput > 0.5f) {
			jumpHeld = true;
			if (lastVertInput <= 0.5f) jumpJustPressed = true;
		}
		lastVertInput = vertInput;
		
		// Update state
		ContinueState();
		
		// Debugging
		//debugText.text = "State: " + state + " (" + timeInState + ")"
		//	+ "\ngrounded: " + grounded
		//	+ "\nvelocity: " + rbody.velocity;
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
		Rigidbody2D targetRbody = target.GetComponent<Rigidbody2D>();
		if (targetRbody != null) targetRbody.AddForceAtPosition(force, 
			striker.transform.position, ForceMode2D.Impulse);
		CharDamage dam = target.GetComponent<CharDamage>();
		if (dam != null) dam.ApplyDamage(10);
	}
	
	public void TakeHit(Vector2 knockbackForce) {
		EnterState(State.KnockedBack);
		CharDamage dam = GetComponent<CharDamage>();
		rbody.AddForce(knockbackForce * dam.knockbackFactor, ForceMode2D.Impulse);
		
		grabber.DropGoat(new Vector2(-knockbackForce.x/2, knockbackForce.y));
	}
	
	public void Reset() {
		EnterState(State.Idle);
		lastVertInput = 0;
		rbody.velocity = velocity = Vector2.zero;
		GetComponent<CharDamage>().Reset();
	}
	
	public void GoatGrabbed() {
	}
	
	public void GoatDropped() {
	}
	
	/// <summary>
	/// This method can be invoked from an animation event at the end of an animation,
	/// so we know it's time to transition to the next state.  Applies mainly to attacks.
	/// </summary>
	public void AnimationEnded() {
		switch (state) {
		case State.Landing:
		case State.WeakHitting:
		case State.StrongHitting:
			EnterState(State.Idle);
			break;
		}
	}
	
	public void ActivateStriker() {
		striker.gameObject.SetActive(true);
	}
	
	public void DeactivateStriker() {
		striker.gameObject.SetActive(false);
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
			striker.SetStrongMode(false);
			break;
		case State.StrongHitting:
			animator.Play(kStrongHitAnim);
			striker.SetStrongMode(true);
			break;
		case State.KnockedBack:
			grounded = false;
			airJumpsDone = 1;	// don't allow further air jumping at this point
			break;
		}
		
		this.state = state;
		stateStartTime = Time.time;
	}
	
	void ContinueState() {
		switch (state) {
			
		case State.Idle:
			RunOrJump();
			if (timeUngrounded > 0.25f) EnterState(State.JumpingDown);
			break;
			
		case State.RunningLeft:
		case State.RunningRight:
			if (!RunOrJump()) EnterState(State.Idle);
			if (timeUngrounded > 0.25f) EnterState(State.JumpingDown);
			break;
			
		case State.JumpingUp:
			if (velocity.y < 0) EnterState(State.JumpingDown);
			if (jumpJustPressed && airJumpsDone < 1) {
				EnterState(State.JumpingUp);
				airJumpsDone++;
			}
			CheckAttacks();
			break;
			
		case State.JumpingDown:
			if (grounded) EnterState(State.Landing);
			if (jumpJustPressed && airJumpsDone < 1) {
				EnterState(State.JumpingUp);
				airJumpsDone++;
			}
			CheckAttacks();
			break;
			
		case State.Landing:
			//if (timeInState > 0.2f) EnterState(State.Idle);
			if (timeInState > 0.2f) RunOrJump();
			break;
			
		case State.WeakHitting:
			if (timeInState > 1f) EnterState(State.Idle);
			break;
			
		case State.StrongHitting:
			if (timeInState > 1f) EnterState(State.Idle);
			break;
			
		case State.KnockedBack:
			velocity = rbody.velocity;
			if (timeInState > 0.5f) EnterState(State.JumpingDown);
			break;
			
		}
	}
	
	bool RunOrJump() {
		if (jumpJustPressed && grounded) SetOrKeepState(State.JumpingUp);
		else if (CheckAttacks()) {
			return true;
		} else if (horzInput < 0) SetOrKeepState(State.RunningLeft);
		else if (horzInput > 0) SetOrKeepState(State.RunningRight);
		else return false;
		return true;
	}
	
	bool CheckAttacks() {
		if (weakHitPressed) {
			if (!grabber.carrying) {
				SetOrKeepState(State.WeakHitting);
				return true;
			}
		} else if (strongHitPressed) {
			if (grabber.carrying) {
				// Throw in the direction of the Dpad.
				Vector3 throwDir;
				if (horzInput == 0) throwDir = Vector3.up;
				else throwDir = new Vector3(Mathf.Sign(horzInput), 0.5f, 0);
				grabber.DropGoat((Vector2)throwDir * 5 + velocity * 2);
			} else {
				SetOrKeepState(State.StrongHitting);
				return true;
			}
		}
		return false;
	}
	
	void Face(int direction) {
		transform.localScale = new Vector3(defaultScale.x * direction, defaultScale.y, defaultScale.z);
	}
	
	void UpdatePhysics() {
		if (state == State.KnockedBack) return;	// no control while knocked back (stunned)
		
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
			if (grabber.carrying) targetSpeed *= grabber.speedFactor;
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
		
		if (grounded) lastGroundedTime = Time.time;
	}
	#endregion
}
