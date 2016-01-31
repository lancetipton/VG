using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RespawnPlayer : MonoBehaviour {
	
	public GameObject follow;
	FollowPlayer followScript;
	public ParticleSystem spawnPart;
	GameObject new_pos;

	private static List<GameObject> usedSpawns = new List<GameObject>();

	void Start(){
		spawnPart.Play();
		followScript = follow.GetComponent<FollowPlayer>();
		followScript.gameObject.SetActive(true);
		// Don't initially spawn two players in the same place.
		// This loop won't be infinite if we have more spawn points than players.
		// This check also presumes sequential rather than parallel execution.
		while (true) {
			var spawn = Respawns.instance.RandomSpawn();
			if (!usedSpawns.Contains(spawn)) {
				transform.position = spawn.transform.position;
				usedSpawns.Add(spawn);
				break;
			}
		}
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
