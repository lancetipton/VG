using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public static SoundManager instance;
	public AudioSource MusicPlayer;
	public AudioSource BGPlayer;
	public AudioSource ThunderPlayer;
	public AudioSource[] FxPlayer = new AudioSource[4];


	public AudioClip[] Music = new AudioClip[2];
	public AudioClip[] Hit = new AudioClip[4];
	public AudioClip[] Foot = new AudioClip[7];
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

		if(fx == "spawn"){
			PlayFX(playerNum, 0);
		}
		else if(fx == "swing"){
			PlayFX(playerNum, 1);
		}
		else if(fx == "hit"){
			int num = Random.Range(0, 3);
			FxPlayer[playerNum].PlayOneShot(Hit[num]);
		}
		else if(fx == "jump"){
			if(playerNum == 3){
				PlayFX(playerNum, 3);
			}
			else{
				PlayFX(playerNum, 4);	
			}
		}
		else if(fx == "pick-up"){
			PlayFX(playerNum, 5);
		}
		else if(fx == "grunt"){
			int num = Random.Range(6, 8);
			PlayFX(playerNum, num);
		}
		else if(fx == "foot1"){
			int num = Random.Range(0, 6);
			FxPlayer[playerNum].PlayOneShot(Foot[num]);
		}
		else if(fx == "foot2"){
			int num = Random.Range(0, 6);
			FxPlayer[playerNum].PlayOneShot(Foot[num]);
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
