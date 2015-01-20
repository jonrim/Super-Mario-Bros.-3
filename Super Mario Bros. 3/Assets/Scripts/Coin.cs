using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {
	public AudioClip Sound;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
	}
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			audio.Play ();
			audio.PlayOneShot (Sound);
			Destroy (this.gameObject);
		}
	}
}
