using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBHV : MonoBehaviour {

	public int keyID;
	public TextMesh text;

	// Use this for initialization
	void Start () {
		if (keyID < 0){
			Destroy (gameObject);
		} else if (keyID > 0){
			text.text = keyID.ToString ();
		} else {
			text.text = "";
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
