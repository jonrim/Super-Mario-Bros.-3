using UnityEngine;
using System.Collections;

public class Enemy_AI : PE_Obj2D {
	public LayerMask GroundLayers;
	private Transform is_on_ground;
	private Animator anim;
	public GameObject camera;
	private GameObject mario;
	public bool set_direction = true;
	public bool canJump;
	public bool canJump2;
	public bool wingedGoomba;
	public float jumpTimer = 0;
	public float timer = 0;
	public int counter = 0;
	public int Health = 1;
	public bool hit;
	public Vector3 startingPos;
	// Use this for initialization
/// <summary>
/// /Jrim
/// </summary>
	public override void Start () {
		camera = GameObject.Find ("Main Camera");
		if (camera.GetComponent<Health>().type == PowerUp.none)
			mario = GameObject.Find ("Mario");
		else if (camera.GetComponent<Health>().type == PowerUp.mushroom)
			mario = GameObject.Find ("Mario_Big");
		else if (camera.GetComponent<Health>().type == PowerUp.tanooki)
			mario = GameObject.Find ("Mario_Tanooki");
			
		anim = GetComponent<Animator>();
		// vel.x = -2.0f;
		startingPos = this.transform.position;
		transform.localScale = new Vector3(-1, 1, 1);
		is_on_ground = transform.FindChild("IsOnGround");
		counter = 0;
		if (wingedGoomba) {
			Health = 2;
		}
		base.Start ();
	}
//Josh
//	public override void Start () {
//		vel.x = -2.0f;
//		transform.localScale = new Vector3(-1, 1, 1);
//		is_on_ground = transform.FindChild("IsOnGround");
//		base.Start ();
//////>>>>>>>>>>>>>
	// Update is called once per frame
	void FixedUpdate () {
		if (camera.GetComponent<Health>().type == PowerUp.none)
			mario = GameObject.Find ("Mario");
		else if (camera.GetComponent<Health>().type == PowerUp.mushroom)
			mario = GameObject.Find ("Mario_Big");
		else if (camera.GetComponent<Health>().type == PowerUp.tanooki)
			mario = GameObject.Find ("Mario_Tanooki");
		if ((mario == null) || (GetComponent<Enemy_Death>().deadbytail)){
			return;
		}
		if ((Mathf.Abs(mario.transform.position.x - startingPos.x) <= 20) && set_direction){
			vel.x = 2.0f*Mathf.Sign(mario.transform.position.x - startingPos.x);
			set_direction = false;
		}
		else if ((Mathf.Abs(mario.transform.position.x - startingPos.x) > 20) && (Mathf.Abs(mario.transform.position.x - this.transform.position.x) > 20)) {
			this.transform.position = startingPos;
			set_direction = true;
		}
		Vector2 point1 = new Vector2(is_on_ground.transform.position.x - is_on_ground.collider2D.bounds.size.x/2, 
		                             is_on_ground.transform.position.y - is_on_ground.collider2D.bounds.size.y/2);
		Vector2 point2 = new Vector2(is_on_ground.transform.position.x + is_on_ground.collider2D.bounds.size.x/2, 
		                             is_on_ground.transform.position.y + is_on_ground.collider2D.bounds.size.y/2);
		canJump = Physics2D.OverlapArea(point1, point2, GroundLayers, 0, 0);
		// next bool needed so that you can't jump off walls
		canJump2 = Physics2D.OverlapPoint(is_on_ground.position, GroundLayers);
		// go in opposite direction if the enemy approaches a cliff
		if (this.gameObject.transform.position.y <= camera.transform.position.y - 12) {
			Destroy(this.gameObject);
		}
		if (wingedGoomba) {
			if (hit) {
				anim.SetBool("Hit", hit);
			}
			if (canJump && Health == 2) {
				anim.SetBool("Jump", false);
				if ((counter < 3) && (jumpTimer > 1.0f)) {
					anim.SetBool("Jump", true);
					vel.y = 10.0f;
					counter++;
				}
				else if (counter == 3) {
					anim.SetBool("Jump", true);
					vel.y = 17.0f;
					jumpTimer = 0;
					counter = 0;
				}
				timer = 0;
			}
		}
		if (!canJump){
			if ((vel.y > 0) && (timer < 0.005f)) {
				acc.y = 0;
			}
			else
				acc.y = -60.0f;
			// terminal velocity
			if (vel.y <= -15.0f) {
				acc.y = 0;
				vel.y = -15.0f;
			}
		}
		if (!canJump2 && vel.y == 0) {
			vel.x = -vel.x;
			transform.localScale = new Vector3(Mathf.Sign(vel.x), 1, 1);
		}
		// canJump2 = canJump2 || (Mathf.Abs (acc.y) < 0.1f);
		jumpTimer += Time.fixedDeltaTime;
		timer += Time.fixedDeltaTime;
	}

	public override void OnTriggerEnter2D(Collider2D otherColl){
		PE_Obj2D other = otherColl.gameObject.GetComponent<PE_Obj2D>();
		if (other == null) {
			return;
		}
		else if (other.gameObject.tag == "Block_item" || other.gameObject.tag == "Block_empty") {
			vel.x = -vel.x;
			float sign = Mathf.Sign (vel.x);
			transform.position = new Vector2(transform.position.x + sign * 0.1f, transform.position.y);
			transform.localScale = new Vector3(sign, 1, 1);
			base.OnTriggerEnter2D(otherColl);
		} else {
			base.OnTriggerEnter2D(otherColl);
		}
	}
	
	public override void OnTriggerStay2D(Collider2D other){
		OnTriggerEnter2D(other);
	}
}
