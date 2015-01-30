using UnityEngine;
using System.Collections;

public enum PowerUp {
	none,
	mushroom,
	tanooki
}

public class Health : MonoBehaviour {
	public bool big = false;
	public bool tanooki = false;
	public bool gothurt = false;
	public bool felloff;
	public int item_number = 0;
	public PowerUp type = PowerUp.none;
	public GameObject mario_small;
	public GameObject mario_big;
	public GameObject mario_tanooki;
	public GameObject mario;
	public Animator anim;
	public float timer;
	public bool invincible;
	Vector2 position;
	Vector2 holderPos;
	// Use this for initialization
	void Start () {
		mario_small = GameObject.Find ("Mario");
		mario_big = GameObject.Find ("Mario_Big");
		mario_tanooki = GameObject.Find ("Mario_Tanooki");
		anim = mario_small.GetComponent<Animator>();
		big = false;
		tanooki = false;
		mario_small.gameObject.SetActive(true);
		mario_big.gameObject.SetActive(false);
		mario_tanooki.gameObject.SetActive(false);
		timer = 10.0f;
		holderPos = new Vector2(-13.0f,0.0f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if ((timer > 3.0f) && invincible){
			invincible = false;
			gothurt = false;
		}
		if ((gothurt && !big && !invincible) || (felloff)){
			gothurt = false;
			// make mario die
			// Time.timeScale = 0.001f;
			float pauseEndTime = Time.realtimeSinceStartup + 0.5f;
			anim.SetBool("Dead", true);
			while (Time.realtimeSinceStartup < pauseEndTime) {}
			// PhysEngine2D.objs.Remove(mario_small.GetComponent<PE_Obj2D>());
			// Time.timeScale = 1;
			Application.LoadLevel(Application.loadedLevel);
		}
		else if (gothurt && !invincible && (type == PowerUp.mushroom)){
			// make mario lose powerup
			gothurt = false;
			Time.timeScale = 0.001f;
			float pauseEndTime = Time.realtimeSinceStartup + 0.01f;
			// delete small mario and add big mario
			//Instantiate(mario_small, mario_big.transform.position, mario_big.transform.rotation);
			Vector3 facingDirection = new Vector3(mario_big.transform.localScale.x, mario_big.transform.localScale.y,
			                         mario_big.transform.localScale.z);
			Vector3 loc = new Vector2(mario_big.transform.position.x, mario_big.transform.position.y);
			mario_small.GetComponent<PE_Obj2D>().vel = mario_big.GetComponent<PE_Obj2D>().vel;
			mario_big.gameObject.SetActive(false);
			mario_small.transform.localScale = facingDirection;
			mario_small.transform.position = loc;
			mario_small.gameObject.SetActive(true);
			timer = 0;
			invincible = true;
			// PhysEngine2D.objs.Remove(mario_big.GetComponent<PE_Obj2D>());
			//Destroy (mario_big);
			//mario_big.active = false;
			anim = mario_small.GetComponent<Animator>();
			anim.speed = 1.0f/Time.timeScale;
			// play small to big mario animation
			//anim.SetBool("Mushroom", true);
			
			//anim.SetBool("Mushroom", false);
			while (Time.realtimeSinceStartup < pauseEndTime) {}
			anim.speed = 1.0f;
			Time.timeScale = 1;
			big = false;
			type = PowerUp.none;
			mario_small.GetComponent<PlayerMovement>().big = false;
		}
		else if (gothurt && !invincible && (type == PowerUp.tanooki)){
			// make mario lose powerup
			gothurt = false;
			Time.timeScale = 0.001f;
			float pauseEndTime = Time.realtimeSinceStartup + 0.01f;
			// delete small mario and add big mario
			//Instantiate(mario_small, mario_big.transform.position, mario_big.transform.rotation);
			Vector3 facingDirection = new Vector3(mario_tanooki.transform.localScale.x, mario_tanooki.transform.localScale.y,
			                                      mario_tanooki.transform.localScale.z);
			mario_big.GetComponent<PE_Obj2D>().vel = mario_tanooki.GetComponent<PE_Obj2D>().vel;
			Vector3 loc = new Vector2(mario_tanooki.transform.position.x, mario_tanooki.transform.position.y);
			mario_tanooki.gameObject.SetActive(false);
			mario_big.transform.localScale = facingDirection;
			mario_big.transform.position = loc;
			mario_big.gameObject.SetActive(true);
			timer = 0;
			invincible = true;
			// PhysEngine2D.objs.Remove(mario_big.GetComponent<PE_Obj2D>());
			//Destroy (mario_big);
			//mario_big.active = false;
			anim = mario_big.GetComponent<Animator>();
			anim.speed = 1.0f/Time.timeScale;
			// play small to big mario animation
			//anim.SetBool("Mushroom", true);
			
			//anim.SetBool("Mushroom", false);
			while (Time.realtimeSinceStartup < pauseEndTime) {}
			anim.speed = 1.0f;
			Time.timeScale = 1;
			big = true;
			tanooki = false;
			type = PowerUp.mushroom;
			mario_big.GetComponent<PlayerMovement>().big = true;
		}
		// mushroom
		if (item_number == 1) {
			if (!big) {
				// freeze time
				Time.timeScale = 0.001f;
				float pauseEndTime = Time.realtimeSinceStartup + 0.01f;
				// delete small mario and add big mario
				Vector3 facingDirection = new Vector3(mario_small.transform.localScale.x, mario_small.transform.localScale.y, mario_small.transform.localScale.z);
				Vector2 loc = new Vector2(mario_small.transform.position.x, mario_small.transform.position.y);
				mario_big.GetComponent<PE_Obj2D>().vel = mario_small.GetComponent<PE_Obj2D>().vel;

				mario_big.transform.localScale = facingDirection;
				mario_big.transform.position = loc;
				mario_big.gameObject.SetActive(true);
				mario_small.gameObject.SetActive(false);
				// Instantiate(mario_big, mario_small.transform.position, mario_small.transform.rotation);
				//mario_big.transform.localScale = new Vector3 (mario_small.transform.localScale.x, mario_small.transform.localScale.y,
				                                             // mario_small.transform.localScale.z);
				// mario_small.active = false;
				// PhysEngine2D.objs.Remove(mario_small.GetComponent<PE_Obj2D>());
				// Destroy (mario_small);
				anim = mario_big.GetComponent<Animator>();
				anim.speed = 1.0f/Time.timeScale;
				// play small to big mario animation
				//anim.SetBool("Mushroom", true);

				//anim.SetBool("Mushroom", false);
				while (Time.realtimeSinceStartup < pauseEndTime) {}
				anim.speed = 1.0f;
				Time.timeScale = 1;
				big = true;
				mario_big.GetComponent<PlayerMovement>().big = true;
			}
			item_number = 0;
		}
		// tanooki
		else if (item_number == 2) {
			if (!tanooki) {
				// freeze time
				Time.timeScale = 0.001f;
				float pauseEndTime = Time.realtimeSinceStartup + 0.01f;
				// delete small mario and add big mario
				Vector3 facingDirection;
				Vector2 loc;
				if (mario_big.activeSelf) {
					mario_big.gameObject.SetActive(false);
					facingDirection = new Vector3(mario_big.transform.localScale.x, mario_big.transform.localScale.y, mario_big.transform.localScale.z);
					loc = new Vector2(mario_big.transform.position.x, mario_big.transform.position.y);
					mario_tanooki.GetComponent<PE_Obj2D>().vel = mario_big.GetComponent<PE_Obj2D>().vel;
				}
				else {
					mario_small.gameObject.SetActive(false);
					facingDirection = new Vector3(mario_small.transform.localScale.x, mario_small.transform.localScale.y, mario_small.transform.localScale.z);
					loc = new Vector2(mario_small.transform.position.x, mario_small.transform.position.y);
					mario_tanooki.GetComponent<PE_Obj2D>().vel = mario_small.GetComponent<PE_Obj2D>().vel;
				}
				mario_tanooki.transform.localScale = facingDirection;
				mario_tanooki.transform.position = loc;
				mario_tanooki.gameObject.GetComponent<PlayerMovement>().tail.gameObject.SetActive(false);
				mario_tanooki.gameObject.SetActive(true);
				// Instantiate(mario_big, mario_small.transform.position, mario_small.transform.rotation);
				//mario_big.transform.localScale = new Vector3 (mario_small.transform.localScale.x, mario_small.transform.localScale.y,
				// mario_small.transform.localScale.z);
				// mario_small.active = false;
				// PhysEngine2D.objs.Remove(mario_small.GetComponent<PE_Obj2D>());
				// Destroy (mario_small);
				anim = mario_tanooki.GetComponent<Animator>();
				anim.speed = 1.0f/Time.timeScale;
				// play small to big mario animation
				//anim.SetBool("Mushroom", true);
				
				//anim.SetBool("Mushroom", false);
				while (Time.realtimeSinceStartup < pauseEndTime) {}
				anim.speed = 1.0f;
				Time.timeScale = 1;
				tanooki = true;
				mario_tanooki.GetComponent<PlayerMovement>().big = true;
			}
			item_number = 0;
		}
		else if (item_number == 3) {
			Vector2 pos = new Vector2( mario_small.transform.position.x, mario_small.transform.position.y + 5.0f);
			// Vector2 pos = new Vector2( mario_small.transform.position.x + 1.0f, mario_small.transform.position.y);
			Instantiate(mario, pos, mario_small.transform.rotation);
			item_number = 0;
		}
		if (type == PowerUp.none) {
			mario_tanooki.transform.position = holderPos;
			mario_big.transform.position = holderPos;
		}
		else if (type == PowerUp.mushroom) {
			mario_tanooki.transform.position = holderPos;
			mario_small.transform.position = holderPos;
		}
		else if (type == PowerUp.tanooki) {
			mario_big.transform.position = holderPos;
			mario_small.transform.position = holderPos;
			
		}
		timer += Time.fixedDeltaTime;
	}
}
