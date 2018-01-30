using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	private Map map = null;
	public RoomBHV roomPrefab;
	public Transform roomsParent;  //Transform to hold rooms for leaner hierarchy view
	public RoomBHV[,] roomBHVMap; //2D array for easy room indexing
	public float roomSpacingX = 10.5f; //Spacing between rooms: X
	public float roomSpacingY = 6f; //Spacing between rooms: Y
	public string filePath = "Assets/Data/data.txt"; //Path to load map data from

	void Awake(){
		//Singleton
		if (instance == null){
			instance = this;
		} else if (instance != this){
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
		//Loads map from data
		LoadMap (filePath);
	}

	// Use this for initialization
	void Start () {
		roomBHVMap = new RoomBHV[Map.SIZE, Map.SIZE];
		for (int x = 0; x < Map.SIZE; x++){
			for (int y = 0; y < Map.SIZE; y++) {
				roomBHVMap [x, y] = null;
			}
		}
		InstantiateRooms ();
		//Starts player to an initial position
		Player.instance.transform.position = roomBHVMap [map.startX, map.startY].transform.position;
		Player.instance.SetPosition (map.startX, map.startY);
	}

	void InstantiateRooms(){
//		for (int y = 0; y < Map.SIZE; y++){
//			for (int x = 0; x < Map.SIZE; x++){
//				Debug.Log (map.rooms [x, y]);
//			}
//		}
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
		RoomBHV newRoom = Instantiate (roomPrefab, roomsParent);
		roomBHVMap[x, y] = newRoom;
		if (x > 1){ // west
			if (map.rooms[x-1, y] != null){
				//Sets door
				newRoom.westDoor = map.rooms [x - 1, y].lockID;
				//Links room doors - assumes the rooms are given in a specific order from data: incr X, incr Y
				roomBHVMap [x, y].doorWest.SetDestination (roomBHVMap [x - 2, y].doorEast);
				roomBHVMap [x - 2, y].doorEast.SetDestination (roomBHVMap [x, y].doorWest);
			}
		}
		if (y > 1){ // north
			if (map.rooms[x, y-1] != null){
				//Sets door
				newRoom.northDoor = map.rooms [x, y - 1].lockID;
				//Links room doors - assumes the rooms are given in a specific order from data: incr X, incr Y
				roomBHVMap [x, y].doorNorth.SetDestination (roomBHVMap [x, y-2].doorSouth);
				roomBHVMap [x, y-2].doorSouth.SetDestination (roomBHVMap [x, y].doorNorth);
			}
		}
		if (x < Map.SIZE){ // east
			if (map.rooms[x+1, y] != null){
				//Sets door
				newRoom.eastDoor = map.rooms [x + 1, y].lockID;
			}
		}
		if (y < Map.SIZE){ // south
			if (map.rooms[x, y+1] != null){
				//Sets door
				newRoom.southDoor = map.rooms [x, y + 1].lockID;
			}
		}
		newRoom.x = x; //TODO: check use
		newRoom.y = y; //TODO: check use
		newRoom.availableKeyID = room.keyID; // Avaiable key to be collected in that room
		if (x == map.startX && y == map.startY){ // sala é a inicial
			newRoom.isStart = true;
		}
		if (x == map.endX && y == map.endY){ // sala é a final
			newRoom.isEnd = true;
		}
		//Sets room transform position
		newRoom.gameObject.transform.position = new Vector2 (roomSpacingX * x, -roomSpacingY * y);
	}

}
