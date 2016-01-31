using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public static SoundManager instance;
	public AudioSource MusicPlayer;
	public AudioSource[] FxPlayer = new AudioSource[4];

	public AudioClip[] Music = new AudioClip[2];
	public AudioClip[] soundFx = new AudioClip[30];


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


	public void FindFX(int playerNum, string fx){
		if(fx == "jump"){

		}
		else if(fx == "hard"){

		}
		else if(fx == "soft"){

		}
		else if(fx == "hard"){

		}
		else if(fx == "pick-up"){

		}
		else if(fx == "grunt"){

		}
	}


	void PlayFX(int playerNum, int fxNum){
		FxPlayer[playerNum].PlayOneShot(soundFx[fxNum]);
	}

	public void PlayMusic(int type){
		MusicPlayer.clip = Music[type];
		MusicPlayer.Play();
	}

}
