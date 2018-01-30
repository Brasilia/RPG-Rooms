using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 5f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){
		float moveHorizontal = Input.GetAxisRaw ("Horizontal");
		float moveVertical = Input.GetAxisRaw ("Vertical");
		Vector2 movement = new Vector2 (moveHorizontal, moveVertical);
		movement.Normalize ();
		Move (movement);
	}

	protected void Move(Vector2 movement){
		transform.position += (Vector3)movement*speed;
		if(movement != Vector2.zero){
			movement.x *= 100000; //favorece olhar na horizontal (elimina olhar diagonal)
			transform.up = movement;	
		}
	}
}
