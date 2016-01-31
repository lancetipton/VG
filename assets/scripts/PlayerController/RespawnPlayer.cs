using UnityEngine;
using System.Collections;

public class RespawnPlayer : MonoBehaviour {
	
	public GameObject follow;
	FollowPlayer followScript;
	public ParticleSystem spawnPart;
	GameObject new_pos;

	void Start(){
		spawnPart.Play();
		followScript = follow.GetComponent<FollowPlayer>();
		followScript.gameObject.SetActive(true);
		transform.position = Respawns.instance.RandomSpawn().transform.position;
		Invoke("TurnOffInd", 3f);
	}

	public void RestPos(){
		new_pos = Respawns.instance.RandomSpawn();
		Invoke("SetPos", 3f);
		gameObject.SetActive(false);
	}

	void SetPos(){
		transform.position = new_pos.transform.position;
		followScript.gameObject.SetActive(true);
		followScript.active = true;
		CharController charCtrl = GetComponent<CharController>();
		if (charCtrl != null) charCtrl.Reset();
		gameObject.SetActive(true);
		spawnPart.Play();
		Invoke("TurnOffInd", 3f);
	}

	public void TurnOffInd(){
		followScript.active = false;
		followScript.gameObject.SetActive(false);
	}

}
