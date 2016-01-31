using UnityEngine;
using System.Collections;

public class KnockOut : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Players") {
			coll.gameObject.GetComponentInChildren<GoatGrabber>().DropGoat(new Vector2());
			coll.gameObject.GetComponent<RespawnPlayer>().RestPos();
		} else if (coll.gameObject.tag == "Goat") {
			coll.gameObject.GetComponent<Goat>().Respawn();
		}
	}

}
