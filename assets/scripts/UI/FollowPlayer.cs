using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	public Transform player;
	public bool active = false;
	// Use this for initialization
	void Start () {
		
	}
	
  void Update () {
  	 Vector3 screenPos = GetComponent<Camera>().WorldToScreenPoint(player.position);
     transform.position = new Vector3 (screenPos.x, screenPos.y, screenPos.z);
  }
}
