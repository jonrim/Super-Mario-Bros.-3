using UnityEngine;
using System.Collections;

public class ManagerScript : MonoBehaviour {
	public bool IsMenuActive  = false;
	public bool Main = false;
	public AudioClip pauseSound;
	public AudioClip levelStart;
	public AudioClip levelMusic;

	void playSound(AudioClip sound, float vol){
		audio.clip = sound;
		audio.volume = vol;
		audio.Play();
	}

	void Awake () {
		// DontDestroyOnLoad(gameObject);
	}
	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
		playSound (levelStart, 0.8f);
	}
	
	// Update is called once per frame
	void Update () {
		if (!audio.isPlaying && Time.timeScale == 1) {
			playSound (levelMusic, 0.8f);
		}
		if (Time.timeScale == 0) {
			audio.Pause ();
		}
		if (Input.GetKeyDown(KeyCode.Return)) {
			if (IsMenuActive) {
				Time.timeScale = 1;
				audio.Play ();
			}
			else {
				audio.Pause();
				audio.PlayOneShot(pauseSound);
			}
			IsMenuActive = !IsMenuActive;
		}
	}

	void OnGUI() {
		const int Width = 400;
		const int Height = 300;
		if (IsMenuActive && Main) {
			Time.timeScale = 0;
			Rect windowRect = new Rect(
				(Screen.width - Width) / 2,
			 	(Screen.height - Height) / 2,
				Width, Height);
			GUILayout.Window(0, windowRect, MainMenu, "MAIN MENU");
			             
		}
		if (IsMenuActive && !Main) {
			Time.timeScale = 0;
			Rect windowRect = new Rect(
				(Screen.width - Width) / 2,
				(Screen.height - Height) / 2,
				Width, Height);
			GUILayout.Window(0, windowRect, PauseMenu, "PAUSED");
			
		}
	}

	private void MainMenu(int id){
		if(GUILayout.Button ("Start Level 1-1")) {
			IsMenuActive = false;
			Application.LoadLevel("Scene_Jonathan");
		}
		if(GUILayout.Button ("Start Level Custom")) {
			IsMenuActive = false;
			Application.LoadLevel("Scene_Custom");
		}
		if(!Application.isWebPlayer && !Application.isEditor) {
			if (GUILayout.Button ("Exit")) {
				Application.Quit ();
			}
		}
	}
	private void PauseMenu(int id){
		if(GUILayout.Button ("Unpause")) {
			IsMenuActive = false;
			Time.timeScale = 1;
		}
		if(GUILayout.Button ("Main Menu")) {
			IsMenuActive = false;
			Time.timeScale = 1;
			Application.LoadLevel("Scene_0");
		}
		if(!Application.isWebPlayer && !Application.isEditor) {
			if (GUILayout.Button ("Exit")) {
				Application.Quit ();
			}
		}
	}
}
