using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class GamepadDetector : MonoBehaviour {
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
		for (int i=0; i<4; i++) {
			if (Mathf.Abs(Input.GetAxis("Player " + (i+1) + " Horizontal")) > 0.2F) {
				Debug.Log("Player " + (i+1) + " (" + Input.GetJoystickNames()[i] + ") dpad pressed");
			}
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
