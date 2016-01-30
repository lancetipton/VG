using UnityEngine;
using System.Collections;

public class HurtPlayer : MonoBehaviour {

	public bool knockLeft;
	public bool knockBackPlayer;

	void OnCollisionEnter2D(Collision2D coll) {
		if(coll.gameObject.tag == "Players"){
			print("here");
			CharDamage Damage = coll.gameObject.GetComponent<CharDamage>();
			int amount = Random.Range(4, 10);
			if(knockBackPlayer){
				Vector2 knockBack;

				if(knockLeft){
					knockBack = new Vector2(Random.Range(-10, -4), Random.Range(4, 10));
				}
				else{
					knockBack = new Vector2(Random.Range(4, 10), Random.Range(4, 10));
				}
				Damage.ApplyDamagePlusKnockback(amount, knockBack);
			}
			else{
				Damage.ApplyDamage(amount);
			}

		}
	}


}
