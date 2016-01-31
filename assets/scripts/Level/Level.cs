using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {

	public static Level instance;
	// Use this for initialization
	
	void Awake(){
		instance = this;
	}

	void Start () {
		SoundManager.instance.PlayMusic(1);
		SoundManager.instance.PlayBG();
	}
	



}
