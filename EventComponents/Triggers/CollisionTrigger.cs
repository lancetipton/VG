using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class CollisionTrigger : MonoBehaviour {
	#region Public Properties
	
	public GameObjectEvent onCollision;
	public GameObjectEvent onTrigger;
	
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
	
	// Sent when an incoming collider makes contact with this object's collider (2D physics only).
	protected void OnCollisionEnter2D(Collision2D collisionInfo) {
		if (onCollision != null) onCollision.Invoke(collisionInfo.gameObject);
	}
	
	// Sent when another object enters a trigger collider attached to this object (2D physics only).
	protected void OnTriggerEnter2D(Collider2D other) {
		if (onTrigger != null) onTrigger.Invoke(other.gameObject);
	}
	
	#endregion
	//--------------------------------------------------------------------------------
	#region Public Methods
	
	#endregion
	//--------------------------------------------------------------------------------
	#region Private Methods

	#endregion
}
