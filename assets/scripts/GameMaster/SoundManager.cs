using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public static SoundManager instance;
	public AudioSource MusicPlayer;
	public AudioSource BGPlayer;
	public AudioSource ThunderPlayer;
	public AudioSource[] FxPlayer = new AudioSource[4];


	public AudioClip[] Music = new AudioClip[2];
	public AudioClip[] soundFx = new AudioClip[30];
	public AudioClip[] Thund = new AudioClip[4];


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

	public void FindFX(){

	}


	public void FindPlayerFX(int playerNum, string fx){

		if(fx == "jump"){

		}
		else if(fx == "swing"){

		}
		else if(fx == "hit"){

		}
		else if(fx == "pick-up"){

		}
		else if(fx == "grunt"){

		}
		else if(fx == "spawn"){
			PlayFX(playerNum, 0);
		}
	}


	void PlayFX(int playerNum, int fxNum){
		FxPlayer[playerNum].PlayOneShot(soundFx[fxNum]);
	}

	public void PlayMusic(int type){
		MusicPlayer.clip = Music[type];
		MusicPlayer.volume = 0.2f;
		MusicPlayer.Play();
	}

	public void PlayBG(){
		BGPlayer.loop = true;
		BGPlayer.volume = 0.2f;
		BGPlayer.Play();
	}

	public void PlayThunder(){
		int num = Random.Range(0, Thund.Length - 1);
		ThunderPlayer.PlayOneShot(Thund[num]);
	}

}
