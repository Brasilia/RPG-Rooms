using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	private Map map = null;
	public RoomBHV roomPrefab;

	void Awake(){
		if (instance == null){
			instance = this;
		} else if (instance != this){
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
		LoadMap ("Assets/Data/data.txt");

	}

	// Use this for initialization
	void Start () {
		InstantiateRooms ();
	}

	void InstantiateRooms(){
		for (int y = 0; y < Map.SIZE; y++){
			for (int x = 0; x < Map.SIZE; x++){
				Debug.Log (map.rooms [x, y]);
			}
		}
		for (int y = 0; y < Map.SIZE; y+=2){
			for (int x = 0; x < Map.SIZE; x+=2){
				InstantiateRoom (x, y);
			}
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	void LoadMap(string filepath){
		map = new Map (filepath);

	}

	public Map GetMap(){
		return map;
	}

	void InstantiateRoom(int x, int y){
		if (map.rooms[x, y] == null){
			return;
		}
		Room room = map.rooms [x, y];
		RoomBHV newRoom = Instantiate (roomPrefab);
		//RoomBHV newRoom = newRoomGO.GetComponent<RoomBHV> ();
		if (x > 1){ // west
			if (map.rooms[x-1, y] != null){
				newRoom.westDoor = map.rooms [x - 1, y].lockID;
			}
		}
		if (y > 1){ // north
			if (map.rooms[x, y-1] != null){
				newRoom.northDoor = map.rooms [x, y - 1].lockID;
			}
		}
		if (x < Map.SIZE){ // east
			if (map.rooms[x+1, y] != null){
				newRoom.eastDoor = map.rooms [x + 1, y].lockID;
			}
		}
		if (y < Map.SIZE){ // south
			if (map.rooms[x, y+1] != null){
				newRoom.southDoor = map.rooms [x, y + 1].lockID;
			}
		}
		newRoom.x = x;
		newRoom.y = y;
		newRoom.availableKeyID = room.keyID;
		if (x == map.startX && y == map.startY){ // sala é a inicial
			newRoom.isStart = true;
		}
		if (x == map.endX && y == map.endY){ // sala é a final
			newRoom.isEnd = true;
		}

		newRoom.gameObject.transform.position = new Vector2 (12 * x, -8 * y);
	}
}
