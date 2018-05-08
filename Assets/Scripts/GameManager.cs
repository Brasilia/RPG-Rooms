using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Analytics;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	private Map map = null;
	public RoomBHV roomPrefab;
	public Transform roomsParent;  //Transform to hold rooms for leaner hierarchy view
	public RoomBHV[,] roomBHVMap; //2D array for easy room indexing
	public float roomSpacingX = 10.5f; //Spacing between rooms: X
	public float roomSpacingY = 6f; //Spacing between rooms: Y
    private string mapDirectory = "Assets/Data/Batch";
    private static string[] maps = null;
    private static string[] rooms = null;
    private int currentMapId = 0;
    private int currentTestBatchId = 0;
	//public string mapFilePath = "Assets/Data/map.txt"; //Path to load map data from
	//public string roomsFilePath = "Assets/Data/rooms.txt";
	public bool readRooms = true;
    public GameObject formMenu;

    public enum LevelPlayState { InProgress, Won, Lost, Skip, Quit }
    public static LevelPlayState state = LevelPlayState.InProgress;
    private static float secondsElapsed = 0;

    void Awake(){
		//Singleton
		if (instance == null){
			instance = this;
		} else if (instance != this){
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
        AnalyticsEvent.GameStart();
        if (Directory.Exists(mapDirectory))
        {
            Debug.Log(mapDirectory + currentTestBatchId);
            // This path is a directory
            ProcessDirectory(mapDirectory+currentTestBatchId, "map*", ref maps);
            ProcessDirectory(mapDirectory+currentTestBatchId, "room*", ref rooms);
        }
        if (maps != null)
        {
            AnalyticsEvent.LevelStart(currentMapId);
            //Loads map from data
            LoadMap(currentMapId);
        }
        else
        {
            Debug.Log("Something is wrong with the map directory!");
        }
        
	}

    // Process all files in the directory passed in, recurse on any directories 
    // that are found, and process the files they contain.
    public static void ProcessDirectory(string targetDirectory, string search, ref string[] files)
    {
        // Process the list of files found in the directory.
        files = Directory.GetFiles(targetDirectory, search);
    }

    // Use this for initialization
    void Start () {
		roomBHVMap = new RoomBHV[Map.sizeX, Map.sizeY];
		for (int x = 0; x < Map.sizeX; x++){
			for (int y = 0; y < Map.sizeY; y++) {
				roomBHVMap [x, y] = null;
			}
		}
		InstantiateRooms ();
		Player.instance.AdjustCamera (map.startX, map.startY);
        Player.instance.SetRoom(map.startX, map.startY);
        
    }

    void InstantiateRooms(){
		for (int y = 0; y < Map.sizeY; y+=2){
			for (int x = 0; x < Map.sizeX; x+=2){
				InstantiateRoom (x, y);
			}
		}
	}

	// Update is called once per frame
	void Update () {
        secondsElapsed += Time.deltaTime;
    }

	void LoadMap(int mapId){
		if (readRooms){ //deve ler também os tiles das salas?
			map = new Map (maps[mapId], rooms[mapId]);
		} else { //apenas as salas, sem tiles
			map = new Map (maps[mapId]);
		}
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
		if (x < Map.sizeX){ // east
			if (map.rooms[x+1, y] != null){
				//Sets door
				newRoom.eastDoor = map.rooms [x + 1, y].lockID;
			}
		}
		if (y < Map.sizeY){ // south
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

    void OnApplicationQuit()
    {
        AnalyticsEvent.GameOver();
    }

    public void SetLevelPlayState(LevelPlayState newState)
    {
        state = newState;
    }

    public void LevelComplete()
    {
        //TODO save every gameplay data
        //TODO make it load a new level
        if(currentMapId < (maps.Length - 1))
        {
            currentMapId++;
        }
        else
        {
            LoadForm();
        }

    }

    void OnDestroy()
    {
        Dictionary<string, object> customParams = new Dictionary<string, object>();
        customParams.Add("seconds_played", secondsElapsed);
        customParams.Add("keys", Player.instance.keys.Count);
        customParams.Add("locks", Player.instance.usedKeys.Count);

        switch (state)
        {
            case LevelPlayState.Won:
                AnalyticsEvent.LevelComplete("0", customParams);
                break;
            case LevelPlayState.Lost:
                AnalyticsEvent.LevelFail("0", customParams);
                break;
            case LevelPlayState.Skip:
                AnalyticsEvent.LevelSkip("0", customParams);
                break;
            case LevelPlayState.InProgress:
            case LevelPlayState.Quit:
            default:
                AnalyticsEvent.LevelQuit("0", customParams);
                break;
        }
    }
    public void LoadForm()
    {
        //Open a GUI here
        formMenu.SetActive(true);
        //TODO: Should check if there is a new batch, if not, set as inactive the continue button.
    }
    //Load a new batch of levels, if it exists
    public void LoadNewBatch()
    {
        currentTestBatchId++;
        currentMapId = 0;
        //TODO: add directory;
        mapDirectory = "newdirectory";
        if (Directory.Exists(mapDirectory))
        {
            // This path is a directory
            ProcessDirectory(mapDirectory+currentTestBatchId, "map*", ref maps);
            ProcessDirectory(mapDirectory+currentTestBatchId, "room*", ref rooms);
        }
        if (maps != null)
        {
            AnalyticsEvent.LevelStart(currentMapId);
            //Loads map from data
            LoadMap(currentMapId);
        }
        else
        {
            Debug.Log("Something is wrong with the map directory!");
        }
        //TODO test if this will work. Maybe a reload of the scene will be necessary
        roomBHVMap = new RoomBHV[Map.sizeX, Map.sizeY];
        for (int x = 0; x < Map.sizeX; x++)
        {
            for (int y = 0; y < Map.sizeY; y++)
            {
                roomBHVMap[x, y] = null;
            }
        }
        InstantiateRooms();
        Player.instance.AdjustCamera(map.startX, map.startY);
        Player.instance.SetRoom(map.startX, map.startY);
    }
}
