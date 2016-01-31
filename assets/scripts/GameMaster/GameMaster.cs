using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	public static GameMaster instance;

	void Awake(){

		if(instance == null)
		{
		  DontDestroyOnLoad(gameObject);
		  instance = this;
		}
		else if(instance != this) {
		  Destroy(gameObject);
		}

	}


}
