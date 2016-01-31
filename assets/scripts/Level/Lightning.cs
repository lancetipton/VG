using UnityEngine;
using System.Collections;

public class Lightning : MonoBehaviour {

	public static Lightning instance;
	public GameObject Flash;
	public ParticleSystem[] Strikes = new ParticleSystem[5];

	int count = 0;

	float strikeTimer;

	void Awake(){
		instance = this;
		strikeTimer = Time.time + Random.Range(1, 5);
		Flash.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
	    if(strikeTimer <= Time.time){
	      PlayStrike();
	    }
	}

	void PlayStrike(){
		PlayLighting();
		PlayFlash();
		strikeTimer = Time.time + Random.Range(1, 5);
	}

	public void PlayLighting(){
		int number = Random.Range(0, 4);
		float pos = Random.Range(-23f, 34);
		Strikes[number].transform.position = new Vector3(transform.position.x + pos, transform.position.y, transform.position.z);
		Strikes[number].Play();
		SoundManager.instance.PlayThunder();
	}

	void PlayFlash(){
		if(count < 3){
			count += 1;
			FlashOn();
		}
		else{
			Flash.SetActive(false);
			count = 0;
		}
	}

	void FlashOn(){
		Flash.SetActive(true);
		Invoke("FlashOff", 0.1f);
	}

	void FlashOff(){
		Flash.SetActive(false);
		Invoke("PlayFlash", 0.1f);
	}
}
