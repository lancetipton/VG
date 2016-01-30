﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraBoundsScript: MonoBehaviour {

	public GameObject[] targets;

	public float offsetX;
	public float offsetY;

	private Vector3 velocity = new Vector3();

	void Update() {
		// Figure out our bounds and such.
		var first = targets[0];
		var bounds = new Bounds(getCenter(first), Vector3.zero);
		// This duplicates the first, but that shouldn't be a big deal.
		foreach (var target in targets) {
			bounds.Encapsulate(getCenter(target));
		}
		var camera = GetComponent<Camera>();
		// Make sure we are showing enough in both dimensions.
		var scale = 1.5f;
		var minSize = 4f;
		var aspect = camera.aspect;
		var sizeY = Mathf.Max(minSize, scale * bounds.extents.y);
		var sizeX = aspect * sizeY;
		if (sizeX < scale * bounds.extents.x) {
			sizeY = scale * bounds.extents.x / aspect;
        } else if (sizeX < minSize) {
			sizeY = minSize / aspect;
		}
		// Set the values.
		var current = new Vector3(
			transform.position.x,
			transform.position.y,
			camera.orthographicSize
		);
		var goal = new Vector3(
			bounds.center.x + offsetX,
			bounds.center.y + offsetY,
			sizeY
		);
		var update = Vector3.SmoothDamp(current, goal, ref velocity, 0.25f);
		camera.orthographicSize = update.z;
		transform.position = new Vector3(
			update.x, update.y, transform.position.z
		);
	}

	private static Vector3 getCenter(GameObject obj) {
		return obj.GetComponent<Renderer>().bounds.center;
    }

}