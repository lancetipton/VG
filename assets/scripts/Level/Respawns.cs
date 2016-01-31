using UnityEngine;
using System.Collections;

public class Respawns : MonoBehaviour {

	public static Respawns instance;

	
	public GameObject[] ResData = new GameObject[6];

	void Awake(){
		instance = this;
	}

	public GameObject RandomSpawn(){
		int num = Random.Range(0,ResData.Length-1);
		return ResData[num];
	}

}
