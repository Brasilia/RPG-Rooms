﻿using System.Collections;
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

	public Collider2D colNorth;
	public Collider2D colSouth;
	public Collider2D colEast;
	public Collider2D colWest;

	public TileBHV tilePrefab;

	// Use this for initialization
	void Start () {
		SetLayout ();
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

	void SetLayout(){
		doorNorth.keyID = northDoor;
		doorSouth.keyID = southDoor;
		doorEast.keyID = eastDoor;
		doorWest.keyID = westDoor;
		float centerX = Room.sizeX / 2 - 0.5f;
		float centerY = Room.sizeY / 2 - 0.5f;
		const float delta = 0.1f; //para que os colisores das portas e das paredes não se sobreponham completamente
		//Posiciona as portas - são somados/subtraídos 1 para que as portas e colisores estejam periféricos à sala
		doorNorth.transform.localPosition = new Vector2 (0.0f, centerY + 1 - delta);
		doorSouth.transform.localPosition = new Vector2 (0.0f, -centerY -1 + delta);
		doorEast.transform.localPosition = new Vector2 (centerX + 1 - delta, 0.0f);
		doorWest.transform.localPosition = new Vector2 (-centerX -1 + delta, 0.0f);

		//Posiciona os colisores das paredes da sala
		colNorth.transform.localPosition = new Vector2 (0.0f, centerY + 1);
		colSouth.transform.localPosition = new Vector2 (0.0f, -centerY - 1);
		colEast.transform.localPosition = new Vector2 (centerX + 1, 0.0f);
		colWest.transform.localPosition = new Vector2 (-centerX -1, 0.0f);
		colNorth.GetComponent<BoxCollider2D> ().size = new Vector2(Room.sizeX + 2, 1);
		colSouth.GetComponent<BoxCollider2D> ().size = new Vector2(Room.sizeX + 2, 1);
		colEast.GetComponent<BoxCollider2D> ().size = new Vector2 (1, Room.sizeY + 2);
		colWest.GetComponent<BoxCollider2D> ().size = new Vector2 (1, Room.sizeY + 2);

		//Posiciona os tiles
		Room thisRoom = GameManager.instance.GetMap().rooms[x, y]; //TODO fazer de forma similar para tirar construção de salas do GameManager
		for (int ix = 0; ix < Room.sizeX; ix++){
			for (int iy = 0; iy < Room.sizeY; iy++){
				int tileID = thisRoom.tiles [ix, iy];
				TileBHV tileObj = Instantiate (tilePrefab);
				tileObj.transform.SetParent (transform);
				tileObj.transform.localPosition = new Vector2 (ix - centerX, iy - centerY);
				Color c; //FIXME provisório para diferenciar sprites
				if (tileID != 1){ //é passável
					tileObj.GetComponent<Collider2D> ().enabled = false; //desativa colisor
					c = new Color (0.2f, 0.2f + 0.1f * tileID, 0.2f); //FIXME provisório para diferenciar sprites
				} else {
					c = Color.black;
				}
				tileObj.GetComponent<SpriteRenderer> ().color = c; //FIXME provisório para diferenciar sprites

			}
		}
	}
}
