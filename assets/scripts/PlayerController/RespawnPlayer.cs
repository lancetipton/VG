using UnityEngine;
using System.Collections;

public class RespawnPlayer : MonoBehaviour {

	GameObject new_pos;

	void Start(){
		transform.position = Respawns.instance.RandomSpawn().transform.position;
	}

	public void RestPos(){
		new_pos = Respawns.instance.RandomSpawn();
		Invoke("SetPos", 3f);
		gameObject.SetActive(false);
	}

	void SetPos(){
		transform.position = new_pos.transform.position;
		gameObject.SetActive(true);
	}

}
