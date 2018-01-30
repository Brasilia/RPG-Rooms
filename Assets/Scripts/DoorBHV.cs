using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBHV : MonoBehaviour {

	public int keyID;
	public TextMesh text;
	public Transform teleportTransform;
//	public int moveX;
//	public int moveY;
	[SerializeField]
	private DoorBHV destination;

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

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player"){
			if (Player.instance.keys.Contains(keyID) || keyID == 0){
				Player.instance.transform.position = destination.teleportTransform.position;
				RoomBHV parent = destination.transform.parent.GetComponent<RoomBHV> ();
				Player.instance.SetPosition (parent.x, parent.y);
			}
		}
	}

	public void SetDestination(DoorBHV other){
		destination = other;
	}
}
