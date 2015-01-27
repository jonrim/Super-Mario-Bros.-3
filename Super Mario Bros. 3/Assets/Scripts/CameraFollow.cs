using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public static GameObject character;
	public GameObject mario_small;
	public GameObject mario_big;
	public GameObject mario_tanooki;
	
	public float bound;
	public bool right_bound = false;
	public  bool left_bound = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		float dist = character.transform.position.x - this.transform.position.x;
		float vertdist = character.transform.position.y - this.transform.position.y;
		if (GetComponent<Health>().type == PowerUp.none) {
			character = mario_small;
		}
		else if (GetComponent<Health>().type == PowerUp.mushroom) {
			character = mario_big;
		}
		else if (GetComponent<Health>().type == PowerUp.tanooki) {
			character = mario_tanooki;
		}
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
//		if (vertdist > bound + 5.0f) {
//			if (this.transform.position.y < 2.14f)
//				return;
//			Vector3 pos = new Vector3(this.transform.position.x, character.transform.position.y - bound - 5.0f, -10);
//			this.transform.position = pos;
//		}
//		else if (vertdist < -bound - 5.0f) {
//			if (this.transform.position.y < 2.14f) {
//				return;
//			}
//			Vector3 pos = new Vector3(this.transform.position.x, character.transform.position.y + bound + 5.0f, -10);
//			this.transform.position = pos;
//		}
	}
}

