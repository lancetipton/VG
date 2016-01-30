using UnityEngine;
using System.Collections;

public class RespawnPlayer : MonoBehaviour {


	public void RestPos(){
		print("Reset pos");
		GameObject new_pos = Respawns.instance.RandomSpawn();
		transform.position = new_pos.transform.position;
	}

}
