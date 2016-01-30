/*
This script attached to the character's striking surface
(fist, weapon, etc.).  It applies damage to whatever's hit.
*/
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Striker : MonoBehaviour {
	#region Public Properties

	#endregion
	//--------------------------------------------------------------------------------
	#region Private Properties

	#endregion
	//--------------------------------------------------------------------------------
	#region MonoBehaviour Events
	void Start() {
	
	}
	
	void Update() {
	
	}
	
	protected void OnTriggerEnter2D(Collider2D other) {
		Vector2 force = transform.TransformDirection(Vector3.right);
		
		Rigidbody2D targetRbody = other.GetComponent<Rigidbody2D>();
		if (targetRbody != null) targetRbody.AddForceAtPosition(force, 
				transform.position, ForceMode2D.Impulse);
		
		CharDamage dam = other.GetComponent<CharDamage>();
		if (dam != null) dam.ApplyDamage(10);
	}
	

	#endregion
	//--------------------------------------------------------------------------------
	#region Public Methods
	
	#endregion
	//--------------------------------------------------------------------------------
	#region Private Methods

	#endregion
}
