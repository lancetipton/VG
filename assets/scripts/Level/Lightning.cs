using UnityEngine;
using System.Collections;

public class Lightning : MonoBehaviour {

	public static Lightning instance;

	public ParticleSystem[] Strikes = new ParticleSystem[5];

	float strikeTimer;
	// -23 / 34
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
		int number = Random.Range(0, 4);
		float pos = Random.Range(-23f, 34);
		Strikes[number].transform.position = new Vector3(transform.position.x + pos, transform.position.y, transform.position.z);
		Strikes[number].Play();
	}

}
