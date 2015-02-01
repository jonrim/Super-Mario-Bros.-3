using UnityEngine;
using System.Collections;

public class Stomp : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D other) {

		if (other.gameObject.tag == "Enemy") {
			other.gameObject.transform.GetComponent<Enemy_Death>().dead = true;

			bounce();
		}
		if (other.gameObject.tag == "Shell") {
			Shell shell = other.gameObject.GetComponent<Shell>();
			if (other.gameObject.GetComponent<Shell>().moving) {
				bounce ();
//				shell.moving = false;
//				shell.vel.x = 0;
//				shell.timer = 0;
			}
//			Vector2 pos = new Vector2(other.gameObject.transform.position.x, other.gameObject.transform.position.y + 0.5f);
//			other.gameObject.transform.position = pos;
//			else {
//				bounce ();
//				if (this.transform.position.x < other.transform.position.x) {
//					GetComponent<PE_Obj2D>().vel.x = -12.0f;
//				} else {
//					GetComponent<PE_Obj2D>().vel.x = 12.0f;
//				}
//				shell.moving = true;
//				shell.timer = 0;
//			}
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		OnTriggerEnter2D (other);
	}

	void bounce() {
		if (Input.GetButton("Jump")) {
			this.transform.parent.GetComponent<PlayerMovement>().HitJump = true;
			this.transform.parent.GetComponent<PlayerMovement>().Hit = false;
		}
		else {
			this.transform.parent.GetComponent<PlayerMovement>().HitJump = false;
			this.transform.parent.GetComponent<PlayerMovement>().Hit = true;
		}
	}
}
