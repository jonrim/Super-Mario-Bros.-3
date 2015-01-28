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
			if (other.gameObject.GetComponent<Shell>().moving) {
				bounce ();
			}
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
