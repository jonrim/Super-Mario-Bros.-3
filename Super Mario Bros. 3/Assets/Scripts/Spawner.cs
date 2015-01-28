using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	public GameObject go;
	public GameObject myObject;
	public bool respawn = true;
	public float timer = 3f;
	public bool coroutineRunning = false;

	// Use this for initialization
	void Start () {
	
	}


	void OnBecameVisible() {
		if (myObject == null && respawn == true) {
			myObject = Instantiate(go, this.transform.position, Quaternion.identity) as GameObject;
			respawn = false;
		}
	}

	void OnBecameInvisible() {
	}

	void Update() {
		if (!respawn) {
			if (myObject == null) ;

			else if (myObject.renderer.isVisible)
					timer = 3f;
			else if (renderer.isVisible)
					timer = 3f;
			
			timer -= Time.deltaTime;

			if (timer <= 0) {
				Destroy (myObject);
				respawn = true;
			}

		}
	}
}
