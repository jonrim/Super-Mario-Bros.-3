using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public static GameObject character;
	public float bound;
	public bool right_bound = false;
	public  bool left_bound = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float dist = character.transform.position.x - this.transform.position.x;
		if (dist > bound){
			if (right_bound)
				return;

			Vector3 pos = new Vector3(character.transform.position.x - bound, this.transform.position.y, -10);
			this.transform.position = pos;
		} else if (dist < -bound) {
			if (left_bound)
				return;

			Vector3 pos = new Vector3(character.transform.position.x + bound, this.transform.position.y, -10);
			this.transform.position = pos;
		}
	}
}

