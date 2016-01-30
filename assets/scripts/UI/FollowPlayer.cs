using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	public Transform player;
	public bool active = false;
	public float offset = 3f;
	// Use this for initialization
	
  void Update () {

  }

  public void TurnOn(){

  	Vector3 pos = new Vector3(player.position.x, player.position.y +offset, player.position.z);
  	Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
    transform.position = new Vector3 (screenPos.x, screenPos.y + offset, screenPos.z);

  	Invoke("TurnOff", 3f);
  }

  void TurnOff(){

  }

}
