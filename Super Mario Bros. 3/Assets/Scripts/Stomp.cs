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
//Jrim
			if (other.gameObject.transform.GetComponent<Enemy_AI>().Health == 1) {
				other.gameObject.transform.GetComponent<Enemy_Death>().dead = true;
			}
			else {
				other.gameObject.transform.GetComponent<Enemy_AI>().hit = true;
				other.gameObject.transform.GetComponent<Enemy_AI>().Health--;
				other.gameObject.transform.GetComponent<PE_Obj2D>().vel.y = -5.0f;
			}
//josh

			other.gameObject.transform.GetComponent<Enemy_Death>().dead = true;

///>>>>>>> origin/master
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

	void OnTriggerStay2D(Collider2D other) {
		OnTriggerEnter2D (other);
	}
}
