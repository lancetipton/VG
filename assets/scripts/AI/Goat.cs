using UnityEngine;
using System.Collections;

public class Goat: MonoBehaviour {

	// Update is called once per frame
	void Update() {
		//Collision.
		float depth = 0.3f;
		var bounds = GetComponent<Collider2D>().bounds;
		var extents = bounds.extents;
		var left = bounds.min - extents;
		var right = new Vector3(bounds.max.x + extents.x, bounds.min.y - extents.y);
        checkBounds(left, extents);
		checkBounds(right, extents);
		Vector3 foot = new Vector3(bounds.center.x, bounds.min.y, 0);
		//Vector3 foot = transform.TransformPoint(new Vector3(0.15f, 0.1f, 0));
		var hits = Physics2D.RaycastAll(foot, Vector2.down, depth);
	}

	private bool checkBounds(Vector3 position, Vector3 extents) {
		var hits = Physics2D.OverlapAreaAll(position - extents, position + extents);
		var any = false;
		foreach (var hit in hits) {
			if (hit.gameObject == gameObject) continue;
			any = true;
		}
		if (any) {
			Debug.DrawLine(position - extents, position + extents, Color.red, 0, false);
		}
		return any;
	}

}
