using UnityEngine;
using System.Collections;



public class PE_Obj : MonoBehaviour {
	public bool			still = false;
	public PE_Collider	coll = PE_Collider.sphere;
	public PE_GravType	grav = PE_GravType.constant;
	
	public Vector3		acc = Vector3.zero;

	public Vector3		vel = Vector3.zero;
	public Vector3		vel0 = Vector3.zero;

	public Vector3		pos0 = Vector3.zero;
	public Vector3		pos1 = Vector3.zero;
	public Vector3		thatP = Vector3.zero;
	public Vector3		delta = Vector3.zero;
	

	void Start() {
		if (PhysEngine.objs.IndexOf(this) == -1) {
			PhysEngine.objs.Add(this);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter(Collider other) {
		// Ignore collisions of still objects
		if (still) return;

		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		if (otherPEO == null) return;

		ResolveCollisionWith(otherPEO);
	}

	void OnTriggerStay(Collider other) {
		OnTriggerEnter(other);
	}

	void ResolveCollisionWith(PE_Obj that) {
		// Assumes that "that" is still

		switch (this.coll) {
		case PE_Collider.sphere:

			switch (that.coll) {
			case PE_Collider.sphere:
				// Sphere / Sphere collision
				float thisR, thatR, rad;
				// Note, this doesn't work with non-uniform or negative scales!
				thisR = Mathf.Max( this.transform.lossyScale.x, this.transform.lossyScale.y, this.transform.lossyScale.z ) / 2;
				thatR = Mathf.Max( that.transform.lossyScale.x, that.transform.lossyScale.y, that.transform.lossyScale.z ) / 2;
				rad = thisR + thatR;

				Vector3 delta = pos1 - that.transform.position;
				delta.Normalize();
				delta *= rad;

				transform.position = that.transform.position + delta;
				break;
			}

			break;

		case PE_Collider.aabb:

			switch (that.coll) {
			case PE_Collider.aabb:


				// AABB / AABB collision
				float eX1, eY1, eX2, eY2, dX, dY, eX0, eY0;

				Vector3 overlap = Vector3.zero;
				thatP = that.transform.position;
				delta = pos1 - thatP;
				if (delta.x >= 0 && delta.y >= 0) { // Top, Right
					// Get the edges that we're concerned with
					eX0 = pos0.x - this.transform.lossyScale.x / 2;
					eY0 = pos0.y - this.transform.lossyScale.y / 2;
					eX1 = pos1.x - this.transform.lossyScale.x / 2;
					eY1 = pos1.y - this.transform.lossyScale.y / 2;
					eX2 = thatP.x + that.transform.lossyScale.x / 2 ;
					eY2 = thatP.y + that.transform.lossyScale.y / 2 ;

					float mThis = (eY0-eY1)/(eX0-eX1);
					float mThat = (eY0-eY2)/(eX0-eX2);

					if ((mThis < mThat) || ((eX0 == eX1) && (mThat <= 0) && (eX0 != eX2))) {// land on top
						float dist = this.transform.lossyScale.y/2 + that.transform.lossyScale.y/2;
						vel.y = 0;
						Vector3 pos = new Vector3(this.transform.position.x, that.transform.position.y + dist, this.transform.position.z);
						this.transform.position = pos;
					}
					else { // hit the right side
						float dist = this.transform.lossyScale.x/2 + that.transform.lossyScale.x/2;
						vel.x = 0;
						Vector3 pos = new Vector3(that.transform.position.x + dist, this.transform.position.y, this.transform.position.z);
						this.transform.position = pos;
					}
					// Distance traveled this step
//					dX = eX1-eX0;
//					dY = eY1-eY0;
//					float uX = 1;
//					float uY = 1;
//					if (eX1 < eX2) { // Overlap in X direction
//						uX = 1 - (eX2-eX1) / (eX0-eX1);
//						
//					}
//					if (eY1 < eY2) { // Overlap in Y direction
//						uY = 1 - (eY2-eY1) / (eY0-eY1);
//						
//					}
					// Find Overlaps (positive is an overlap, negative 
					// overlap.x = 

				} else if (delta.x >= 0 && delta.y < 0) { // Bottom, Right
					eX0 = pos0.x - this.transform.lossyScale.x / 2;
					eY0 = pos0.y + this.transform.lossyScale.y / 2;
					eX1 = pos1.x - this.transform.lossyScale.x / 2;
					eY1 = pos1.y + this.transform.lossyScale.y / 2;
					eX2 = thatP.x + that.transform.lossyScale.x / 2 ;
					eY2 = thatP.y - that.transform.lossyScale.y / 2 ;
					
					float mThis = (eY0-eY1)/(eX0-eX1);
					float mThat = (eY0-eY2)/(eX0-eX2);
					
					if ((mThis > mThat) || ((eX0 == eX1) && (mThat >= 0) && (eX0 != eX2))) {// hit the bottom
						float dist = this.transform.lossyScale.y/2 + that.transform.lossyScale.y/2;
						vel.y = 0;
						Vector3 pos = new Vector3(this.transform.position.x, that.transform.position.y - dist, this.transform.position.z);
						this.transform.position = pos;
					}
					else { // hit the right side
						float dist = this.transform.lossyScale.x/2 + that.transform.lossyScale.x/2;
						vel.x = 0;
						Vector3 pos = new Vector3(that.transform.position.x + dist, this.transform.position.y, this.transform.position.z);
						this.transform.position = pos;
					}
				} else if (delta.x < 0 && delta.y < 0) { // Bottom, Left
					eX0 = pos0.x + this.transform.lossyScale.x / 2;
					eY0 = pos0.y + this.transform.lossyScale.y / 2;
					eX1 = pos1.x + this.transform.lossyScale.x / 2;
					eY1 = pos1.y + this.transform.lossyScale.y / 2;
					eX2 = thatP.x - that.transform.lossyScale.x / 2 ;
					eY2 = thatP.y - that.transform.lossyScale.y / 2 ;
					
					float mThis = (eY0-eY1)/(eX0-eX1);
					float mThat = (eY0-eY2)/(eX0-eX2);
					
					if ((mThis < mThat) || ((eX0 == eX1) && (mThat <= 0) && (eX0 != eX2))) {// hit the bottom
						float dist = this.transform.lossyScale.y/2 + that.transform.lossyScale.y/2;
						vel.y = 0;
						Vector3 pos = new Vector3(this.transform.position.x, that.transform.position.y - dist, this.transform.position.z);
						this.transform.position = pos;
					}
					else { // hit the left side
						float dist = this.transform.lossyScale.x/2 + that.transform.lossyScale.x/2;
						vel.x = 0;
						Vector3 pos = new Vector3(that.transform.position.x - dist, this.transform.position.y, this.transform.position.z);
						this.transform.position = pos;
					}
				} else if (delta.x < 0 && delta.y >= 0) { // Top, Left
					eX0 = pos0.x + this.transform.lossyScale.x / 2;
					eY0 = pos0.y - this.transform.lossyScale.y / 2;
					eX1 = pos1.x + this.transform.lossyScale.x / 2;
					eY1 = pos1.y - this.transform.lossyScale.y / 2;
					eX2 = thatP.x - that.transform.lossyScale.x / 2 ;
					eY2 = thatP.y + that.transform.lossyScale.y / 2 ;
					
					float mThis = (eY0-eY1)/(eX0-eX1);
					float mThat = (eY0-eY2)/(eX0-eX2);
					
					if ((mThis > mThat) || ((eX0 == eX1) && (mThat <= 0) && (eX0 != eX2))) {// land on top
						float dist = this.transform.lossyScale.y/2 + that.transform.lossyScale.y/2;
						vel.y = 0;
						Vector3 pos = new Vector3(this.transform.position.x, that.transform.position.y + dist, this.transform.position.z);
						this.transform.position = pos;
					}
					else { // hit the left side
						float dist = this.transform.lossyScale.x/2 + that.transform.lossyScale.x/2;
						vel.x = 0;
						Vector3 pos = new Vector3(that.transform.position.x - dist, this.transform.position.y, this.transform.position.z);
						this.transform.position = pos;
					}
				}


				break;
			}

			break;
		}
	}


}
