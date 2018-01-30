using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBHV : MonoBehaviour {

	public int keyID;
	public TextMesh text;

	// Use this for initialization
	void Start () {
		text.text = keyID.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player"){
			Player.instance.GetKey (keyID);
			Destroy (gameObject);
		}
	}
}
