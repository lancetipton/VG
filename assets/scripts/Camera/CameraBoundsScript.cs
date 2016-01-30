using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraBoundsScript: MonoBehaviour {

	public GameObject[] targets;

	public float offsetX;
	public float offsetY;

	private float posX;
	private float posY;
	private float posZ;

	Vector3 startPos;

	void Update() {
		var first = targets[0];
		var bounds = new Bounds(first.transform.position, Vector3.zero);
		foreach (var target in targets) {
			bounds.Encapsulate(target.transform.position);
		}
		var camera = GetComponent<Camera>();
		var aspect = camera.aspect;
		camera.orthographicSize = bounds.extents.y;
		transform.position = new Vector3(bounds.center.x, bounds.center.y, transform.position.z);
	}

}
