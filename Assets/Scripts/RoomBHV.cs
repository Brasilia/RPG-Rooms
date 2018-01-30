using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBHV : MonoBehaviour {

	public int x;
	public int y;
	public int northDoor = -1; //-1 for non-existant
	public int southDoor = -1;
	public int eastDoor = -1;
	public int westDoor = -1;
	public int availableKeyID = 0;
	public bool isStart = false;
	public bool isEnd = false;

	public DoorBHV doorNorth;
	public DoorBHV doorSouth;
	public DoorBHV doorEast;
	public DoorBHV doorWest;

	public KeyBHV keyPrefab;

	// Use this for initialization
	void Start () {
		doorNorth.keyID = northDoor;
		doorSouth.keyID = southDoor;
		doorEast.keyID = eastDoor;
		doorWest.keyID = westDoor;
		if (availableKeyID > 0){ // existe uma chave
			// instancia chave
			KeyBHV key = Instantiate(keyPrefab, transform);
			key.keyID = availableKeyID;
		}
		if (isStart){
			//Algum efeito
			transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
		}
		if (isEnd){
			//Algum efeito
			transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
