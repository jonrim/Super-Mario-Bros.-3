using UnityEngine;
using System.Collections;
using System;



public class PE_Obj2D : MonoBehaviour {
	public bool			still = false;
	public PE_Collider2D	coll = PE_Collider2D.aabb;
	public PE_GravType2D	grav = PE_GravType2D.constant;
	public GameObject mainCamera;
	public GameObject go_mushroom;
	public GameObject go_tanooki;
	public GameObject go_multiplier;
	public bool MakeTanooki;
	public bool MakeClone;
	public Vector2		acc = Vector2.zero;

	public Vector2		vel = Vector2.zero;
	public Vector2		vel0 = Vector2.zero;
	public Vector2		pos0 = Vector2.zero;
	public Vector2		pos1 = Vector2.zero;
	public Vector2		thatP = Vector2.zero;
	public Vector2		delta = Vector2.zero;
	// public GameObject Block_empty;
	private bool blockhit = false;
	private Animator block_anim;
	virtual public void Start() {
		if (this.gameObject.tag == "Block_item") {
			block_anim = this.gameObject.GetComponentInParent<Animator>();
		}
		if (PhysEngine2D.objs.IndexOf(this) == -1) {
			PhysEngine2D.objs.Add(this);
		}
		mainCamera = GameObject.Find ("Main Camera");
		// Block_empty = GameObject.FindWithTag("Block_empty");
	}
	

	virtual public void OnTriggerEnter2D(Collider2D other) {
		// Ignore collisions of still objects
		if (still) return;

		PE_Obj2D otherPEO = other.gameObject.GetComponent<PE_Obj2D>();
		if (otherPEO == null) return;

		ResolveCollisionWith(otherPEO);
	}

	virtual public void OnTriggerStay2D(Collider2D other) {
		OnTriggerEnter2D(other);
	}
	
	void ResolveCollisionWith(PE_Obj2D that) {

			// print ("Collision between this: " + this.gameObject.tag + " and that: " + that.gameObject.tag);

		if ((this.gameObject.tag == "Item") && (that.gameObject.tag == "Player")) {
			
				// print ("ResolveCollisionWith 2");
			
			transform.GetComponent<ItemBehavior>().destroyed = true;
		}
		else if ((this.gameObject.tag == "Enemy") && (that.gameObject.tag == "Player")) {
			
			// print ("ResolveCollisionWith 3");
			// print ("Hit");
			//Time.timeScale = 0;

			//if we got stomped, don't damage mario
			if (!this.collider2D.bounds.Intersects(that.gameObject.GetComponent("IsOnGround").collider2D.bounds)) {
				// damage Mario
				mainCamera = GameObject.Find ("Main Camera");
				mainCamera.GetComponent<Health>().gothurt = true;
			}
		}
		if ((that.gameObject.tag != "Player") && (this.gameObject.tag != "ItemBottom") &&
		    !(this.gameObject.tag == "Item" && that.gameObject.tag == "Item") && 
		    !(this.gameObject.tag == "Enemy" && that.gameObject.tag == "Enemy") && 
		    (this.gameObject.tag != "Block_item") && !(that.gameObject.tag == "Enemy" &&
		     this.gameObject.tag == "Player" && mainCamera.GetComponent<Health>().invincible == true)){

			if (this.gameObject.tag == "Shell" && that.gameObject.tag == "Player") {
				print ("ResolveCollisionWith 4");
				
			}

			switch (this.coll) {
			

			case PE_Collider2D.aabb:

				switch (that.coll) {
				case PE_Collider2D.aabb:

					// AABB / AABB collision
					float eX1, eY1, eX2, eY2, eX0, eY0;

					//Vector2 overlap = Vector2.zero;
					thatP = that.transform.position;
					delta = pos1 - thatP;
					if ((this.transform.position.x <= thatP.x + that.collider2D.bounds.size.x/2 ) &&
					    (this.transform.position.x >= thatP.x - that.collider2D.bounds.size.x/2 )) { // if the center of this obj is between the x-bounds of that obj

						if ((pos1.y >= thatP.y) 
						&& !((that.gameObject.tag == "Platform") && (vel.y > 0))){ // land on top
							float dist = this.collider2D.bounds.size.y/2 + that.collider2D.bounds.size.y/2;
							vel.y = 0;
							acc.y = 0;
							Vector2 pos = new Vector2(this.transform.position.x, that.collider2D.transform.position.y + dist+0.03f);
							this.transform.position = pos;
						}
						else if (that.gameObject.tag != "Platform") { // hit the bottom
							float dist = this.collider2D.bounds.size.y/2 + that.collider2D.bounds.size.y/2;
							vel.y = -1;
							Vector2 pos = new Vector2(this.transform.position.x, that.transform.position.y - dist-0.03f);
							this.transform.position = pos;
	//						if (that.gameObject.tag == "Block_item") {
	//							Instantiate (Block_empty, that.transform.position, that.transform.rotation);
	//							Destroy(that.gameObject);
	//						}
						}
					}

					else if (delta.x >= 0 && delta.y >= 0) { // Top, Right
						// Get the edges that we're concerned with
						eX0 = pos0.x - this.collider2D.bounds.size.x / 2; // prev Left side of object.
						eY0 = pos0.y - this.collider2D.bounds.size.y / 2; // prev bottom side
						eX1 = pos1.x - this.collider2D.bounds.size.x / 2; // current right side
						eY1 = pos1.y - this.collider2D.bounds.size.y / 2; // current bottom side
						eX2 = thatP.x + that.collider2D.bounds.size.x / 2 ; // other object's right side 
						eY2 = thatP.y + that.collider2D.bounds.size.y / 2 ; // other object's  top side.
						if (((Mathf.Abs(eY0 - eY2) <= 0.1)) 
						    && !((that.gameObject.tag == "Platform") && (vel.y > 0))) { // land on top
							float dist = this.collider2D.bounds.size.y/2 + that.collider2D.bounds.size.y/2;
							vel.y = 0;
							acc.y = 0;
							Vector2 pos = new Vector2(this.transform.position.x, that.transform.position.y + dist + 0.03f);
							this.transform.position = pos;
						}
						else if ((this.gameObject.tag != "Enemy") && (this.gameObject.tag != "Item")
						         && (that.gameObject.tag != "Platform")) { // hit the right side
							float dist = this.collider2D.bounds.size.x/2 + that.collider2D.bounds.size.x/2;
							if (pos0.x > pos1.x) {
								vel.x = 0;
								acc.x = 0;
							}
							Vector2 pos = new Vector2(that.transform.position.x + dist + 0.1f, this.transform.position.y);
							this.transform.position = pos;
						}

					} else if (delta.x >= 0 && delta.y < 0) { // Bottom, Right
						eX0 = pos0.x - this.collider2D.bounds.size.x / 2;
						eY0 = pos0.y + this.collider2D.bounds.size.y / 2;
						eX1 = pos1.x - this.collider2D.bounds.size.x / 2;
						eY1 = pos1.y + this.collider2D.bounds.size.y / 2;
						eX2 = thatP.x + that.collider2D.bounds.size.x / 2 ;
						eY2 = thatP.y - that.collider2D.bounds.size.y / 2 ;

						if (((Mathf.Abs(eY1 - eY2) <= 0.1) && (Mathf.Abs(eX1 - eX2) >= 0.4)) 
						    && !((that.gameObject.tag == "Platform") && (vel.y > 0))){ // hit the bottom
							float dist = this.collider2D.bounds.size.y/2 + that.collider2D.bounds.size.y/2;
							vel.y = 0;
							Vector2 pos = new Vector2(this.transform.position.x, that.transform.position.y - dist - 0.03f);
							this.transform.position = pos;
						}
						else if ((this.gameObject.tag != "Enemy") && (this.gameObject.tag != "Item")
						         && (that.gameObject.tag != "Platform")) { // hit the right side
							float dist = this.collider2D.bounds.size.x/2 + that.collider2D.bounds.size.x/2;
							if (pos0.x > pos1.x) {
								vel.x = 0;
								acc.x = 0;
							}
							Vector2 pos = new Vector2(that.transform.position.x + dist + 0.1f, this.transform.position.y);
							this.transform.position = pos;
						}
					} else if (delta.x < 0 && delta.y < 0) { // Bottom, Left
						eX0 = pos0.x + this.collider2D.bounds.size.x / 2;
						eY0 = pos0.y + this.collider2D.bounds.size.y / 2;
						eX1 = pos1.x + this.collider2D.bounds.size.x / 2;
						eY1 = pos1.y + this.collider2D.bounds.size.y / 2;
						eX2 = thatP.x - that.collider2D.bounds.size.x / 2 ;
						eY2 = thatP.y - that.collider2D.bounds.size.y / 2 ;

						if ((Mathf.Abs(eY1 - eY2) <= 0.1) && (Mathf.Abs(eX1 - eX2) >= 0.4) 
						    && !((that.gameObject.tag == "Platform") && (vel.y > 0))) { // land on top
							float dist = this.collider2D.bounds.size.y/2 + that.collider2D.bounds.size.y/2;
							vel.y = 0;
							Vector2 pos = new Vector2(this.transform.position.x, that.transform.position.y - dist - 0.03f);
							this.transform.position = pos;
						}
						else if ((this.gameObject.tag != "Enemy") && (this.gameObject.tag != "Item")
						         && (that.gameObject.tag != "Platform")) { // hit the left side
							float dist = this.collider2D.bounds.size.x/2 + that.collider2D.bounds.size.x/2;
							if (pos0.x < pos1.x) {
								vel.x = 0;
								acc.x = 0;
							}
							Vector2 pos = new Vector2(that.transform.position.x - dist - 0.1f, this.transform.position.y);
							this.transform.position = pos;
						}
					} else if (delta.x < 0 && delta.y >= 0) { // Top, Left
						eX0 = pos0.x + this.collider2D.bounds.size.x / 2;
						eY0 = pos0.y - this.collider2D.bounds.size.y / 2;
						eX1 = pos1.x + this.collider2D.bounds.size.x / 2;
						eY1 = pos1.y - this.collider2D.bounds.size.y / 2;
						eX2 = thatP.x - that.collider2D.bounds.size.x / 2 ;
						eY2 = thatP.y + that.collider2D.bounds.size.y / 2 ;

						if ((Mathf.Abs(eY1 - eY2) <= 0.1)  
							&& !((that.gameObject.tag == "Platform") && (vel.y > 0))) { // land on top
							float dist = this.collider2D.bounds.size.y/2 + that.collider2D.bounds.size.y/2;
							vel.y = 0;
							acc.y = 0;
							Vector2 pos = new Vector2(this.transform.position.x, that.transform.position.y + dist + 0.03f);
							this.transform.position = pos;
						}
						else if ((this.gameObject.tag != "Enemy") && (this.gameObject.tag != "Item")
						         && (that.gameObject.tag != "Platform")){ // hit the left side
							float dist = this.collider2D.bounds.size.x/2 + that.collider2D.bounds.size.x/2;
							if (pos0.x < pos1.x) {
								vel.x = 0;
								acc.x = 0;
							}
							Vector2 pos = new Vector2(that.transform.position.x - dist-0.1f, this.transform.position.y);
							this.transform.position = pos;
						}
					}


					break;
				case PE_Collider2D.incline:


					float angle = 0;
					Vector3 axis = Vector3.zero;

					that.transform.rotation.ToAngleAxis(out angle, out axis);
					double angleRad = (Math.PI * angle) / 180.0;

					//positive slope case
					if (angle > 90.0f) {
						//aabb's bottom right corner

						// because turning involves scale, we need this little hack for eX1
						if (this.transform.lossyScale.x > 0) {
							eX1 = pos1.x + this.transform.lossyScale.x / 2;
						} else {
							eX1 = pos1.x - this.transform.lossyScale.x / 2;
						}
						eY1 = pos1.y - this.transform.lossyScale.y / 2;
						
						//construct the equation of the incline using a point on the incline.
						//  Slope will be tan(angle)
						float slopeHeight = that.transform.lossyScale.x / 2 ;
						float slopeY = (slopeHeight * (float)Math.Sin(angleRad)) + that.transform.position.y;
						float slopeX = (slopeHeight * (float)Math.Cos(angleRad)) + that.transform.position.x;
						float slopeM = (float)Math.Tan(angleRad - (Math.PI / 2));



						//B = Y - mX
						float slopeB = slopeY - (slopeM * slopeX);


						float dist = (slopeM * eX1) + slopeB - eY1;
						//calculate new Y position = mX + B
						Vector2 pos = new Vector2(this.transform.position.x, this.transform.position.y + dist);
						// Vector2 pos = new Vector2(slopeX, slopeY);

						this.transform.position = pos;
						vel.y = 0;
						acc.y = 0;
					}

					break;
				}

				break;
			}
		}
		else if ((this.gameObject.tag == "Block_item") && (that.gameObject.tag == "Player") && (!blockhit)){
			thatP = that.transform.position;
			if ((thatP.x <= this.transform.position.x + this.collider2D.bounds.size.x/2 ) &&
			    (thatP.x >= this.transform.position.x - this.collider2D.bounds.size.x/2 )) { // if the center of this obj is between the x-bounds of that obj
				if (thatP.y < this.transform.position.y) {
//					float dist = this.collider2D.bounds.size.y/2 + that.collider2D.bounds.size.y/2;
//					Vector2 pos = new Vector2(that.transform.position.x, this.transform.position.y - dist-0.03f);
//					that.transform.position = pos;
//					that.gameObject.GetComponent<PE_Obj2D>().vel.y = -1;
					blockhit = true;
					block_anim.SetBool ("BlockHit", blockhit);
					if (MakeClone) {
						Instantiate(go_multiplier, new Vector2(this.transform.position.x,
						                                    this.transform.position.y + this.collider2D.bounds.size.y/2 + 0.5f), 
						            this.transform.rotation);
					}
					else if (!that.gameObject.GetComponent<PlayerMovement>().big) {
						Instantiate(go_mushroom, new Vector2(this.transform.position.x,
						                                  this.transform.position.y + this.collider2D.bounds.size.y/2 + 0.5f), 
						            this.transform.rotation);

					}
					else if (MakeTanooki) {
						Instantiate(go_tanooki, new Vector2(this.transform.position.x,
						                                  this.transform.position.y + this.collider2D.bounds.size.y/2 + 0.5f), 
						            this.transform.rotation);
					}
				}
			}
		}
		if ((this.gameObject.tag == "Player") && (that.gameObject.tag == "Item")) {
			// mainCamera = GameObject.Find ("Main mainCamera");
			if (that.gameObject.GetComponent<ItemBehavior>().mushroom) {
				mainCamera.GetComponent<Health>().item_number = 1;
				mainCamera.GetComponent<Health>().type = PowerUp.mushroom;
			}
			else if (that.gameObject.GetComponent<ItemBehavior>().tanooki) {
				mainCamera.GetComponent<Health>().item_number = 2;
				mainCamera.GetComponent<Health>().type = PowerUp.tanooki;
			}
			else if (that.gameObject.GetComponent<ItemBehavior>().multiplier) {
				mainCamera.GetComponent<Health>().item_number = 3;
			}
		}
	}
	
	
}
