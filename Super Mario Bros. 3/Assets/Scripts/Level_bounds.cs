using UnityEngine;
using System.Collections;

public enum Bound_type {
	RIGHT,
	LEFT
}


public class Level_bounds : MonoBehaviour {
	public Bound_type bound_type;
	private CameraFollow camera;

	// Use this for initialization
	void Start () {
		camera = GameObject.Find ("Main Camera").GetComponent<CameraFollow>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnBecameVisible() {
		if (bound_type == Bound_type.RIGHT) {
			camera.right_bound = true;
		} else if (bound_type == Bound_type.LEFT) {
			camera.left_bound = true;
		}

	}

	void OnBecameInvisible() {
		if (bound_type == Bound_type.RIGHT) {
			camera.right_bound = false;
		} else if (bound_type == Bound_type.LEFT) {
			camera.left_bound = false;
		}
	}

}
