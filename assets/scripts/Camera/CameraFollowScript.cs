using UnityEngine;
using System.Collections;

public class CameraFollowScript : MonoBehaviour {

  public static CameraFollowScript instance;

  public GameObject target;

  public float offsetX;
  public float offsetY;


  private float posX;
  private float posY;
  private float posZ;

  Vector3 startPos;


  void Awake(){
    instance = this;
  }


  void Update () {

      posX = target.transform.position.x + offsetX;
      posY = target.transform.position.y + offsetY;
      posZ = transform.position.z;

      transform.position = new Vector3 (posX, posY, posZ);

  }




}
