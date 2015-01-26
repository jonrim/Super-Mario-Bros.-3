using UnityEngine;
using System.Collections;

public enum Bound_type {
	RIGHT,
	LEFT
}


public class Level_bounds : MonoBehaviour {
	public Bound_type bound_type;
	private CameraFollow mainCamera;

	// Use this for initialization
	void Start () {
//jrim
		camera = GameObject.Find ("Main Camera").GetComponent<CameraFollow>();
		camera.left_bound = false;
		camera.right_bound = false;
//jos
		mainCamera = GameObject.Find ("Main Camera").GetComponent<CameraFollow>();
//>>>>>>> origin/master
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnBecameVisible() {
		if (bound_type == Bound_type.RIGHT) {
			mainCamera.right_bound = true;
		} else if (bound_type == Bound_type.LEFT) {
			mainCamera.left_bound = true;
		}

	}

	void OnBecameInvisible() {
		if (bound_type == Bound_type.RIGHT) {
			mainCamera.right_bound = false;
		} else if (bound_type == Bound_type.LEFT) {
			mainCamera.left_bound = false;
		}
	}

}
