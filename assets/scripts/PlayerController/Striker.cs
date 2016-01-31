/*
This script attached to the character's striking surface
(fist, weapon, etc.).  It applies damage to whatever's hit.
*/
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Striker : MonoBehaviour {
	#region Public Properties
	
	public int damageValue = 10;
	public float knockbackValue = 2;
	public GameObject hitEffect;
	
	#endregion
	//--------------------------------------------------------------------------------
	#region Private Properties

	#endregion
	//--------------------------------------------------------------------------------
	#region MonoBehaviour Events
	void Start() {
		if (hitEffect != null) {
			hitEffect.transform.SetParent(transform.parent);
			hitEffect.SetActive(false);
		}
	}
	
	void Update() {
	
	}
	
	protected void OnTriggerEnter2D(Collider2D other) {
		Debug.Log(gameObject.name + " hits " + other.name);
		
		// Calculate the force to apply
		Vector2 force = transform.TransformVector(Vector3.right);
		force.y = 0.5f;	// add a bit of upward force
		force *= knockbackValue;
		
		CharDamage dam = other.GetComponent<CharDamage>();
		if (dam != null) dam.ApplyDamage(damageValue);
		
		CharController charCtrl = other.GetComponent<CharController>();
		if (charCtrl != null) {
			charCtrl.TakeHit(force);
		} else {		
			Rigidbody2D targetRbody = other.GetComponentInParent<Rigidbody2D>();
			if (targetRbody != null) targetRbody.AddForce(force, ForceMode2D.Impulse);
		}
		
		if (hitEffect != null) {
			hitEffect.transform.position = transform.position;
			hitEffect.SetActive(true);
		}
		
	}
	

	#endregion
	//--------------------------------------------------------------------------------
	#region Public Methods
	
	#endregion
	//--------------------------------------------------------------------------------
	#region Private Methods

	#endregion
}
