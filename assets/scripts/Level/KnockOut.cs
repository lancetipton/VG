using UnityEngine;
using System.Collections;

public class KnockOut : MonoBehaviour {

  void OnTriggerEnter2D(Collider2D coll) {

    if(coll.gameObject.tag == "Players"){
       coll.gameObject.GetComponent<RespawnPlayer>().RestPos();

    }
  }

}
