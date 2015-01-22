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
		audio.PlayOneShot (Sound);
		Time.timeScale = 0;
	}
}
