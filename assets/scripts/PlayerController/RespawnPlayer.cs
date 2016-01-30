﻿using UnityEngine;
using System.Collections;

public class RespawnPlayer : MonoBehaviour {

	public ParticleSystem spawnPart;
	GameObject new_pos;

	void Start(){
		spawnPart.Play();
		transform.position = Respawns.instance.RandomSpawn().transform.position;
	}

	public void RestPos(){
		
		new_pos = Respawns.instance.RandomSpawn();
		Invoke("SetPos", 3f);
		gameObject.SetActive(false);
	}

	void SetPos(){
		transform.position = new_pos.transform.position;
		CharController charCtrl = GetComponent<CharController>();
		if (charCtrl != null) charCtrl.Reset();
		gameObject.SetActive(true);
		spawnPart.Play();
	}

}
