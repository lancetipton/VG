using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Alter : MonoBehaviour {

	public ParticleSystem skybeams;
	public ParticleSystem alterFx;

	void OnTriggerEnter2D(Collider2D coll) {
		if(coll.gameObject.tag == "Goat"){
			Invoke("Winround", 3f);
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
		if(coll.gameObject.tag == "Goat"){
			CancelInvoke("Winround");
		}
	}

	void Winround(){
		skybeams.Play();
		alterFx.Stop();
		WinRound.instance.ShowWinner();	
		Invoke("RestartGame", 4f);
	}


	void RestartGame(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
