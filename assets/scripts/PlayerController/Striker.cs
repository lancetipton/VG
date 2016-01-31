/*
This script attached to the character's striking surface
(fist, weapon, etc.).  It applies damage to whatever's hit.
*/
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Striker : MonoBehaviour {
	#region Public Properties
	
	public int weakDamageValue = 5;
	public float weakKnockbackValue = 1;
	
	public int strongDamageValue = 15;
	public float strongKnockbackValue = 2;
	
	public GameObject hitEffect;
	
	#endregion
	//--------------------------------------------------------------------------------
	#region Private Properties
	
	bool strongMode = false;
	
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
		//Debug.Log(gameObject.name + " hits " + other.name);
		
		// Calculate the force to apply
		Vector2 force = transform.TransformVector(Vector3.right);
		force.y = 0.5f;	// add a bit of upward force
		force *= (strongMode ? strongKnockbackValue : weakKnockbackValue);
		
		CharDamage dam = other.GetComponent<CharDamage>();
		if (dam != null) dam.ApplyDamage(strongMode ? strongDamageValue : weakDamageValue);
		
		CharController charCtrl = other.GetComponent<CharController>();
		if (charCtrl != null) {
			charCtrl.TakeHit(force);
		} else {		
			Rigidbody2D targetRbody = other.GetComponentInParent<Rigidbody2D>();
			if (targetRbody != null) targetRbody.AddForce(force, ForceMode2D.Impulse);
		}
		SoundManager.instance.FindPlayerFX(GetComponentInParent<CharController>().playerNum, "hit");
			
		if (hitEffect != null) {
			hitEffect.transform.position = transform.position;
			hitEffect.SetActive(true);
		}
		
	}
	

	#endregion
	//--------------------------------------------------------------------------------
	#region Public Methods
	
	public void SetStrongMode(bool strongMode) {
		this.strongMode = strongMode;
	}
	
	#endregion
	//--------------------------------------------------------------------------------
	#region Private Methods

	#endregion
}
