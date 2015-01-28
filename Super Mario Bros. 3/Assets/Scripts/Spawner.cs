using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	public GameObject go;
	private GameObject myObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnBecameVisible() {
		if (myObject == null) {
			myObject = Instantiate(go, this.transform.position, Quaternion.identity) as GameObject;
		}
	}
}
