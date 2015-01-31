using UnityEngine;
using System.Collections;

public class Shell : PE_Obj2D {
	public LayerMask GroundLayers;
	private Transform is_on_ground;
	public bool canJump;
	private Animator anim;
	public float timer = 0;
	public bool moving;
	public GameObject Koopa;
	public AudioClip hit_by_shell;
	public AudioClip hit_wall;
	public AudioClip breakSound;

	void playSound(AudioClip sound, float vol){
		audio.clip = sound;
		audio.volume = vol;
		audio.Play();
	}

	// Use this for initialization
	public override void Start () {
		is_on_ground = transform.FindChild("IsOnGround");
		anim = GetComponent<Animator>();
		timer = 0;
		moving = false;

		base.Start ();
	}
	// Update is called once per frame
	void FixedUpdate () {
		if (vel.x == 0)
			moving = false;
		if (!moving) {
			if (timer > 4.0f) {
					anim.SetBool ("twitch", true);		
				}
				anim.speed = 1.0f + Mathf.Pow (timer / 7.0f, 7);
			if (timer >= 7.0f) {
					PhysEngine2D.objs.Remove (transform.gameObject.GetComponent<PE_Obj2D> ());
					Destroy (transform.gameObject);
					Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.5f);
					GameObject go = Instantiate (Koopa, pos, transform.rotation) as GameObject;
			}
			timer += Time.fixedDeltaTime;
		}
		Vector2 point1 = new Vector2(is_on_ground.transform.position.x - is_on_ground.collider2D.bounds.size.x/2, 
		                             is_on_ground.transform.position.y - is_on_ground.collider2D.bounds.size.y/2);
		Vector2 point2 = new Vector2(is_on_ground.transform.position.x + is_on_ground.collider2D.bounds.size.x/2, 
		                             is_on_ground.transform.position.y + is_on_ground.collider2D.bounds.size.y/2);
		canJump = Physics2D.OverlapArea(point1, point2, GroundLayers, 0, 0);
		// next bool needed so that you can't jump off walls

		if (!canJump) {
			GetComponent<PE_Obj2D>().acc.y = -60.0f;
			// terminal velocity
			if (GetComponent<PE_Obj2D>().vel.y <= -15.0f) {
				GetComponent<PE_Obj2D>().acc.y = 0;
				GetComponent<PE_Obj2D>().vel.y = -15.0f;
			}
		}

	}

	public override void OnTriggerEnter2D(Collider2D otherColl){
		// print ("I collided with something");
		PE_Obj2D other = otherColl.gameObject.GetComponent<PE_Obj2D>();
		if (other == null) {
			return;
		}

		//print ("collided with " + other.gameObject.tag);
		if (other.gameObject.tag == "Player" && !moving) 
		{
			Vector2 pos = new Vector2(other.gameObject.transform.position.x, other.gameObject.transform.position.y + 0.5f);
			other.gameObject.transform.position = pos;
			// print ("player hit me");
			if (this.transform.position.x < other.transform.position.x) {
				GetComponent<PE_Obj2D>().vel.x = -12.0f;
			} else {
				GetComponent<PE_Obj2D>().vel.x = 12.0f;
			}
			moving = true;
			timer = 0;
			playSound(hit_by_shell, 1.0f);
			//this.gameObject.tag = "Shell";
		}
		else if (other.gameObject.tag == "Player" && moving) {
			mainCamera.GetComponent<Health>().gothurt = true;
		}
		else if (other.gameObject.tag == "PlayerFeet" && moving) {
			print ("stop");
			moving = false;
			vel.x = 0;
			timer = 0;
		}
		
		else if (other.gameObject.tag == "Block_item" || other.gameObject.tag == "Block_empty" || other.gameObject.tag == "Block_breakable"
		         && moving) {
			if (other.gameObject.tag == "Block_breakable" && moving) {
				playSound(breakSound, 1.0f);
				PhysEngine2D.objs.Remove(other.gameObject.GetComponent<PE_Obj2D>());	
				Destroy(other.gameObject);
			}
			else if (moving) {
				GetComponent<PE_Obj2D>().vel.x = -GetComponent<PE_Obj2D>().vel.x;
				playSound (hit_wall, 1.0f);
			}
			//audio.Play ();
			//audio.PlayOneShot(hit_wall);
		} else if (other.gameObject.tag == "Enemy") {
			return;
		}
		else if (other.gameObject.tag == "Shell") {
			
			if (other.gameObject.GetComponent<Shell>().moving) {
				transform.localScale = new Vector3(1, -1, 1);
				vel.y = 4f;
				transform.position = new Vector3(transform.position.x, transform.position.y + .04f, transform.position.z);
				this.gameObject.layer = 0;
			}
		}
		else {
			base.OnTriggerEnter2D(otherColl);
		}
	}

	public override void OnTriggerStay2D(Collider2D other){
		OnTriggerEnter2D(other);
	}
}
