using UnityEngine;
using System.Collections;

public class Finish : MonoBehaviour {
	public AudioClip Sound;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			audio.clip = Sound;
			audio.volume = 2.0f;
			audio.Play();
			Time.timeScale = 0;
		}
	}
}
