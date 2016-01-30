using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerStatusDisplay : MonoBehaviour {
	#region Public Properties
	
	public CharDamage charDamage;
	
	#endregion
	//--------------------------------------------------------------------------------
	#region Private Properties
	
	Text playerNameText;
	Text damageText;
	int lastDamageShown = -1;
	
	#endregion
	//--------------------------------------------------------------------------------
	#region MonoBehaviour Events
	void Start() {
		playerNameText = transform.FindChild("PlayerNameText").GetComponent<Text>();
		damageText = transform.FindChild("DamageText").GetComponent<Text>();
		playerNameText.text = charDamage.gameObject.name;
	}
	
	void Update() {
		int dam = charDamage.damage;
		if (dam != lastDamageShown) {
			damageText.text = charDamage.damage.ToString();
			lastDamageShown = dam;
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
