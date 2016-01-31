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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
