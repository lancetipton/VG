using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Alter : MonoBehaviour {

	public ParticleSystem skybeams;
	public ParticleSystem alterFx;
	CameraBoundsScript camBounds;
	
	public static Alter instance;
	
	void Awake() {
		instance = this;
	}
	
	void Start() {
		camBounds = Camera.main.GetComponent<CameraBoundsScript>();
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if(coll.gameObject.tag == "Goat"){
			camBounds.FocusOnAltar(alterFx.transform.position);
			Invoke("Winround", 3f);
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
		if(coll.gameObject.tag == "Goat"){
			Debug.Log("Trigger exit - canceling");
			CancelWin();
		}
	}
	
	public void CancelWin() {
		camBounds.CancelFocusOnAltar();
		CancelInvoke("Winround");		
	}
	
	void Winround(){
		camBounds.CancelFocusOnAltar();
		skybeams.Play();
		alterFx.Stop();
		WinRound.instance.ShowWinner();	
		Invoke("RestartGame", 4f);
	}


	void RestartGame(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
