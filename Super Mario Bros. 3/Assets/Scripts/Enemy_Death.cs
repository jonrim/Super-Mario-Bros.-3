using UnityEngine;
using System.Collections;

public class Enemy_Death : MonoBehaviour {
	private Animator enemy_anim;
	public float timer;
	public bool dead = false;
	public bool dead_anim = false;
	// Use this for initialization
	void Start () {
		enemy_anim = transform.GetComponent<Animator>();
		timer = 10.0f;
	}
	
	// Update is called once per frame
	virtual public void FixedUpdate () {
		if (dead && !dead_anim) {
			enemy_anim.SetBool ("Dead", dead);
			timer = 0;
			PhysEngine2D.objs.Remove(transform.gameObject.GetComponent<PE_Obj2D>());
			transform.gameObject.GetComponent<PE_Obj2D>().still = true;
			dead_anim = true;
		}
		if (timer >= 0.2f && timer <= 5.0f && dead_anim) {
			// PhysEngine2D.objs.Remove(transform.parent.gameObject.GetComponent<PE_Obj2D>());
			Destroy (transform.gameObject);
		}
		timer += Time.fixedDeltaTime;
	}
}
