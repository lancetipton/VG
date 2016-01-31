using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	public Transform player;
	public bool active = true;
	public float offset = 3f;
	
	Vector3 our_scale;

	void Start(){
		our_scale = transform.localScale;
	}

	void LateUpdate(){
		if(active){
			if(player.localScale.x < 0){
				transform.localScale = new Vector3(-our_scale.x,transform.localScale.y, transform.localScale.z);
			}
			else{
				transform.localScale = our_scale;
			}
			Vector3 pos = new Vector3(player.position.x, player.position.y +offset, player.position.z);
			Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
			transform.position = new Vector3 (screenPos.x, screenPos.y + offset, screenPos.z);
		}
	}

}
