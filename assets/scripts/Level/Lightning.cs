using UnityEngine;
using System.Collections;

public class Lightning : MonoBehaviour {

	public static Lightning instance;

	public ParticleSystem[] Strikes = new ParticleSystem[5];

	float strikeTimer;
	
	void Awake(){
		instance = this;
		strikeTimer = Time.time + Random.Range(1, 5);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(strikeTimer <= Time.time){
	      PlayStrike();
	    }
	}

	void PlayStrike(){
		PlayLighting();
		strikeTimer = Time.time + Random.Range(1, 5);
	}

	public void PlayLighting(){
		print("play");
		int number = Random.Range(0, 4);
		Strikes[0].Play();
	}

}
