using UnityEngine;
using System.Collections;

public class ManagerScript : MonoBehaviour {
	public bool IsMenuActive { get; set;}
	void Awake () {
		// DontDestroyOnLoad(gameObject);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		const int Width = 400;
		const int Height = 300;
		if (IsMenuActive) {
			Rect windowRect = new Rect(
				(Screen.width - Width) / 2,
			 	(Screen.height - Height) / 2,
				Width, Height);
			GUILayout.Window(0, windowRect, MainMenu, "Main Menu");
			             
		}
	}

	private void MainMenu(int id){
		if(GUILayout.Button ("Start Game")) {
			IsMenuActive = false;
		}
		if(!Application.isWebPlayer && !Application.isEditor) {
			if (GUILayout.Button ("Exit")) {
				Application.Quit ();
			}
		}
	}
}
