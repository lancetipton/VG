using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WinRound : MonoBehaviour {

	public static WinRound instance;
	public Text winner;
	public GameObject back;
	void Awake(){
		instance = this;
	}

	void Start(){
		winner.text = "";
		back.SetActive(false);
	}

	public void ShowWinner(){
		back.SetActive(true);
		winner.text = "PLAYER " + Goat.instance.lastCarry + " WINS!";
	}

}
