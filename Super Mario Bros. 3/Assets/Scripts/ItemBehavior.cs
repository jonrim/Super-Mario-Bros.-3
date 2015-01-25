using UnityEngine;
using System.Collections;

public class ItemBehavior : PE_Obj2D {
	public LayerMask GroundLayers;
	private Transform is_on_ground;
	public bool canJump;
	public bool canJump2;
	public bool destroyed = false;
	public bool spawned = false;
	private Animator anim;
	private float timer = 0;
	// Use this for initialization
	public override void Start () {
		is_on_ground = transform.FindChild("IsOnGround");
		anim = GetComponent<Animator>();
		base.Start ();
	}
	// Update is called once per frame
	void FixedUpdate () {
		if (mushroom && timer >= 1.0f && !spawned) {
			GetComponent<PE_Obj2D>().vel.x = -3.0f;
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
		if (!canJump2) {
			GetComponent<PE_Obj2D>().acc.y = -60.0f;
			// terminal velocity
			if (GetComponent<PE_Obj2D>().vel.y <= -20.0f) {
				GetComponent<PE_Obj2D>().acc.y = 0;
				GetComponent<PE_Obj2D>().vel.y = -20.0f;
			}
		}
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
