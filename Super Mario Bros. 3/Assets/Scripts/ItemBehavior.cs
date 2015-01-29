using UnityEngine;
using System.Collections;

public class ItemBehavior : PE_Obj2D {
	public LayerMask GroundLayers;
	private Transform is_on_ground;
	public bool canJump;
	public bool canJump2;
	public bool mushroom;
	public bool tanooki;
	public bool coin;
	public bool multiplier;
	public bool destroyed = false;
	public bool spawned = false;
	public float end_pos= 1000.0f;
	private Animator anim;
	public float timer = 0;
	// Use this for initialization
	public override void Start () {
		is_on_ground = transform.FindChild("IsOnGround");
		anim = GetComponent<Animator>();
		timer = 0;
		if (tanooki) {
			vel.y = 15.0f;
		}
		base.Start ();
	}
	// Update is called once per frame
	void FixedUpdate () {
		if (coin) {
			Vector3 newPos = new Vector3(transform.position.x, transform.position.y + .3f, transform.position.z);
			if (newPos.y >= end_pos) {
				Destroy (transform.gameObject);
			}
			transform.position = newPos;
			return;
		}
		if (!tanooki && !coin && timer <= 1.0f) {
			Vector3 newPos = new Vector3(transform.position.x, transform.position.y + .08f, transform.position.z);
			if (newPos.y >= end_pos) {
				newPos.y = end_pos;
			}
			transform.position = newPos;
		}
		if (mushroom && timer >= 1.0f && !spawned) {
			vel.x = -3.0f;
			// transform.FindChild("IsOnGround").GetComponent<PE_Obj2D>().vel.x = -3.0f;
			timer = 0;
			spawned = true;
		}
		anim.speed = 1.0f + Mathf.Pow (timer/7.0f,7);
		if (timer >= 5.0f) {
			anim.SetBool("Disappear", true);
		}
		if ((destroyed) || (timer >= 10.0f)) {
			PhysEngine2D.objs.Remove(transform.gameObject.GetComponent<PE_Obj2D>());
			Destroy (transform.gameObject);
		}
		Vector2 point1 = new Vector2(is_on_ground.transform.position.x - is_on_ground.collider2D.bounds.size.x/2, 
		                             is_on_ground.transform.position.y - is_on_ground.collider2D.bounds.size.y/2);
		Vector2 point2 = new Vector2(is_on_ground.transform.position.x + is_on_ground.collider2D.bounds.size.x/2, 
		                             is_on_ground.transform.position.y + is_on_ground.collider2D.bounds.size.y/2);
		canJump = Physics2D.OverlapArea(point1, point2, GroundLayers, 0, 0);
		// next bool needed so that you can't jump off walls
		canJump2 = Physics2D.OverlapPoint(is_on_ground.position, GroundLayers);

		if (!canJump2 && !tanooki) {
			acc.y = -60.0f;
			// terminal velocity
			if (vel.y <= -15.0f) {
				acc.y = 0;
				vel.y = -15.0f;
			}
		}
		if (tanooki) {

			timer = 0;
		}
		timer += Time.fixedDeltaTime;
	}

	public override void OnTriggerEnter2D(Collider2D otherColl){
		PE_Obj2D other = otherColl.gameObject.GetComponent<PE_Obj2D>();
		if (other == null) {
			return;
		}
		else if (((other.gameObject.tag == "Block_item" && timer > 1.0f) || other.gameObject.tag == "Block_empty") && 
		         (transform.position.y < otherColl.transform.position.y + otherColl.collider2D.bounds.size.y/2)) {
			vel.x = -vel.x;
			float sign = Mathf.Sign (vel.x);
			transform.position = new Vector2(transform.position.x + sign * 0.1f, transform.position.y);
			transform.localScale = new Vector3(sign, 1, 1);
			base.OnTriggerEnter2D(otherColl);
		}
		else if (other.gameObject.tag == "Block_item" && (transform.position.y < end_pos - 0.1f) && !tanooki) {}
		else if (tanooki && other.gameObject.tag != "Player") {}
		else {
			base.OnTriggerEnter2D(otherColl);
		}
	}
	
	public override void OnTriggerStay2D(Collider2D other){
		OnTriggerEnter2D(other);
	}

}
