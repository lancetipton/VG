using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class GoatGrabber : MonoBehaviour {
	#region Public Properties
	
	public float speedFactor = 0.8f;
	
	public UnityEvent onGoatGrabbed;
	public UnityEvent onGoatDropped;
	
	public bool carrying {
		get { return goat != null; }
	}
	
	#endregion
	//--------------------------------------------------------------------------------
	#region Private Properties
	
	Rigidbody2D goat;
	public float cantGrabUntil;	// Time.time at which we are allowed to grab again
	
	#endregion
	//--------------------------------------------------------------------------------
	#region MonoBehaviour Events
	void Start() {
	}
	
	void LateUpdate() {
		if (goat != null) goat.MovePosition(transform.position);
	}
	
	protected void OnTriggerEnter2D(Collider2D other) {
		if (goat != null || Time.time < cantGrabUntil) return;
		
		Rigidbody2D otherBody = other.GetComponent<Rigidbody2D>();
		if (other.CompareTag("Goat") && !otherBody.isKinematic) {
			GrabGoat(otherBody);
		}
	}

	#endregion
	//--------------------------------------------------------------------------------
	#region Public Methods
	
	public void DropGoat(Vector2 direction) {
		if (goat == null) return;
		goat.position = transform.position + (Vector3)direction.normalized;
		goat.GetComponent<Collider2D>().enabled = true;
		goat.isKinematic = false;
		goat.velocity = new Vector3(direction.x, direction.y, 0);
		Debug.Log("Dropped goat with velocity " + goat.velocity);
		goat = null;
		cantGrabUntil = Time.time + 0.5f;
		onGoatDropped.Invoke();
	}
	
	#endregion
	//--------------------------------------------------------------------------------
	#region Private Methods
	
	void GrabGoat(Rigidbody2D goat) {
		this.goat = goat;
		Debug.Log("Got the goat!");
		goat.isKinematic = true;
		goat.MovePosition(transform.position);
		goat.GetComponent<Collider2D>().enabled = false;
		onGoatGrabbed.Invoke();
		CharController player = GetComponentInParent<CharController>();
		Goat.instance.lastCarry = player.playerNum;
	}
	
	#endregion
}
