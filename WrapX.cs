using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class WrapX : MonoBehaviour {
	#region Public Properties
	public float minX = -3;
	public float maxX = 3;

	#endregion
	//--------------------------------------------------------------------------------
	#region Private Properties

	#endregion
	//--------------------------------------------------------------------------------
	#region MonoBehaviour Events
	void Start() {
	
	}
	
	void Update() {
		if (transform.position.x < minX) {
			SetX(transform.position.x + maxX - minX);
		} else if (transform.position.x > maxX) {
			SetX(transform.position.x - maxX + minX);
		}
	}

	#endregion
	//--------------------------------------------------------------------------------
	#region Public Methods
	
	#endregion
	//--------------------------------------------------------------------------------
	#region Private Methods
	void SetX(float x) {
		transform.position = new Vector3(x, transform.position.y, transform.position.z);
	}
	#endregion
}
