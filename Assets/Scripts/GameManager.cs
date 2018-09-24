using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    private List<TextAsset> maps = new List<TextAsset>();
    private List<TextAsset> rooms = new List<TextAsset>();
    private Map map = null;
    public AudioSource audioSource;
    public AudioClip bgMusic, fanfarreMusic;
    public TextMeshProUGUI keyText;
    public TextMeshProUGUI roomText;
    public RoomBHV roomPrefab;
    public Transform roomsParent;  //Transform to hold rooms for leaner hierarchy view
    public RoomBHV[,] roomBHVMap; //2D array for easy room indexing
    public float roomSpacingX = 10.5f; //Spacing between rooms: X
    public float roomSpacingY = 6f; //Spacing between rooms: Y
    private string mapDirectory;
    //private static string[] maps = null;
    //private static string[] rooms = null;
    private int currentMapId = 0;
    private int currentTestBatchId = 0;
    //public string mapFilePath = "Assets/Data/map.txt"; //Path to load map data from
    //public string roomsFilePath = "Assets/Data/rooms.txt";
    public bool readRooms = true;
    public GameObject formMenu;

    public enum LevelPlayState { InProgress, Won, Lost, Skip, Quit }
    public static LevelPlayState state = LevelPlayState.InProgress;
    private static float secondsElapsed = 0;

    void Awake() {
        //Singleton
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        //mapDirectory = Application.dataPath + "/Data/Batch";

        audioSource = GetComponent<AudioSource>();

        readRooms = false;
        DontDestroyOnLoad(gameObject);
        AnalyticsEvent.GameStart();

        maps.Add(Resources.Load<TextAsset>("Batch0/Lizard"));
        maps.Add(Resources.Load<TextAsset>("Batch0/MyMoon"));
        maps.Add(Resources.Load<TextAsset>("Batch0/MyLizard"));
        maps.Add(Resources.Load<TextAsset>("Batch0/Dragon"));
        maps.Add(Resources.Load<TextAsset>("Batch0/MyDragon"));
        maps.Add(Resources.Load<TextAsset>("Batch0/Moon"));
        
        /*if (Directory.Exists(mapDirectory + currentTestBatchId))
        {
            Debug.Log(mapDirectory + currentTestBatchId);
            // This path is a directory
            ProcessDirectory(mapDirectory + currentTestBatchId, "map*txt", ref maps);
            ProcessDirectory(mapDirectory + currentTestBatchId, "room*txt", ref rooms);
        }
        else
        {
            Debug.Log("Something is wrong with the map directory!");
        }*/


    }

    // Process all files in the directory passed in, recurse on any directories 
    // that are found, and process the files they contain.
    public static void ProcessDirectory(string targetDirectory, string search, ref string[] files)
    {
        // Process the list of files found in the directory.
        files = Directory.GetFiles(targetDirectory, search);
        foreach (string file in files)
        {
            //AssetDatabase.ImportAsset(file);
            TextAsset asset = Resources.Load<TextAsset>("test");
            Debug.Log("File: " + file);
        }
    }

    // Use this for initialization
    void Start() {
        //LoadNewLevel();
    }

    void InstantiateRooms() {
        for (int y = 0; y < Map.sizeY; y += 2) {
            for (int x = 0; x < Map.sizeX; x += 2) {
                InstantiateRoom(x, y);
            }
        }
    }

    // Update is called once per frame
    void Update() {
        secondsElapsed += Time.deltaTime;
    }

    void LoadMap(int mapId) {
        if (readRooms) { //deve ler também os tiles das salas?
            map = new Map(maps[mapId].text, rooms[mapId].text);
        } else { //apenas as salas, sem tiles
            map = new Map(maps[mapId].text);
        }
    }

    public Map GetMap() {
        return map;
    }

    void InstantiateRoom(int x, int y) {
        if (map.rooms[x, y] == null) {
            return;
        }
        Room room = map.rooms[x, y];
        RoomBHV newRoom = Instantiate(roomPrefab, roomsParent);
        roomBHVMap[x, y] = newRoom;
        if (x > 1) { // west
            if (map.rooms[x - 1, y] != null) {
                //Sets door
                newRoom.westDoor = map.rooms[x - 1, y].lockID;
                //Links room doors - assumes the rooms are given in a specific order from data: incr X, incr Y
                roomBHVMap[x, y].doorWest.SetDestination(roomBHVMap[x - 2, y].doorEast);
                roomBHVMap[x - 2, y].doorEast.SetDestination(roomBHVMap[x, y].doorWest);
            }
        }
        if (y > 1) { // north
            if (map.rooms[x, y - 1] != null) {
                //Sets door
                newRoom.northDoor = map.rooms[x, y - 1].lockID;
                //Links room doors - assumes the rooms are given in a specific order from data: incr X, incr Y
                roomBHVMap[x, y].doorNorth.SetDestination(roomBHVMap[x, y - 2].doorSouth);
                roomBHVMap[x, y - 2].doorSouth.SetDestination(roomBHVMap[x, y].doorNorth);
            }
        }
        if (x < Map.sizeX) { // east
            if (map.rooms[x + 1, y] != null) {
                //Sets door
                newRoom.eastDoor = map.rooms[x + 1, y].lockID;
            }
        }
        if (y < Map.sizeY) { // south
            if (map.rooms[x, y + 1] != null) {
                //Sets door
                newRoom.southDoor = map.rooms[x, y + 1].lockID;
            }
        }
        newRoom.x = x; //TODO: check use
        newRoom.y = y; //TODO: check use
        newRoom.availableKeyID = room.keyID; // Avaiable key to be collected in that room
        if (x == map.startX && y == map.startY) { // sala é a inicial
            newRoom.isStart = true;
        }
        if (x == map.endX && y == map.endY) { // sala é a final
            newRoom.isEnd = true;
        }
        //Sets room transform position
        newRoom.gameObject.transform.position = new Vector2(roomSpacingX * x, -roomSpacingY * y);
    }

    public void LoadNewLevel()
    {
        ChangeMusic(bgMusic);
        if (maps != null)
        {
            Debug.Log("MapSize: " + maps.Count);
            foreach (TextAsset file in maps)
            {
                Debug.Log("Map: " + file.text);
            }
            
            AnalyticsEvent.LevelStart(currentMapId);
            //Loads map from data
            LoadMap(currentMapId);
        }
        else
        {
            Debug.Log("Something is wrong with the map directory!");
        }
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
        UpdateRoomGUI(map.startX, map.startY);
        OnStartMap(currentMapId);
    }

    private void OnStartMap (int id)
    {
        PlayerProfile.instance.OnMapStart(id);
        PlayerProfile.instance.OnRoomEnter(new Vector2Int(map.startX, map.startY));
        Debug.Log("Started Profiling");
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
        ChangeMusic(fanfarreMusic);
        //TODO save every gameplay data
        //TODO make it load a new level
        Debug.Log("MapID:" +currentMapId);
        Debug.Log("MapsLength:" + maps.Count);

        //Analytics for the level
        Dictionary<string, object> customParams = new Dictionary<string, object>();
        customParams.Add("seconds_played", secondsElapsed);
        customParams.Add("keys", Player.instance.keys.Count);
        customParams.Add("locks", Player.instance.usedKeys.Count);

        switch (state)
        {
            case LevelPlayState.Won:
                AnalyticsEvent.LevelComplete(currentTestBatchId+currentMapId, customParams);
                break;
            case LevelPlayState.Lost:
                AnalyticsEvent.LevelFail(currentTestBatchId + currentMapId, customParams);
                break;
            case LevelPlayState.Skip:
                AnalyticsEvent.LevelSkip(currentTestBatchId + currentMapId, customParams);
                break;
            case LevelPlayState.InProgress:
            case LevelPlayState.Quit:
            default:
                AnalyticsEvent.LevelQuit(currentTestBatchId + currentMapId, customParams);
                break;
        }
        if (currentMapId < (maps.Count - 1))
        {
            Debug.Log("Next map");
            currentMapId++;
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
            

            
        }
        else
        {
            Time.timeScale = 0f;
            Debug.Log("Load Form");
            LoadForm();
        }

    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Level Loaded");
        Debug.Log(scene.name);
        Debug.Log(mode);
        if (scene.name == "Level")
        {
            Player pl = Player.instance;
            pl.cam = Camera.main;
            formMenu = GameObject.Find("Canvas").transform.Find("FormPanel").gameObject;
            keyText = GameObject.Find("KeyUIText").GetComponent<TextMeshProUGUI>();
            roomText = GameObject.Find("RoomUI").GetComponent<TextMeshProUGUI>();
            LoadNewLevel();
        }
    }

    void OnDestroy()
    {
        
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
        Time.timeScale = 1f;
        formMenu.SetActive(false);
        currentTestBatchId++;
        currentMapId = 0;
        if (currentTestBatchId == 1)
            readRooms = true;

        maps.Clear();
        maps = new List<TextAsset>();
        rooms.Clear();
        rooms = new List<TextAsset>();

        maps.Add(Resources.Load<TextAsset>("Batch1/Eagle"));
        maps.Add(Resources.Load<TextAsset>("Batch1/MyEagle"));
        maps.Add(Resources.Load<TextAsset>("Batch1/Lion"));
        maps.Add(Resources.Load<TextAsset>("Batch1/Snake"));
        maps.Add(Resources.Load<TextAsset>("Batch1/MyLion"));
        maps.Add(Resources.Load<TextAsset>("Batch1/MySnake"));

        rooms.Add(Resources.Load<TextAsset>("Batch1/EagleRoom"));
        rooms.Add(Resources.Load<TextAsset>("Batch1/MyEagleRoom"));
        rooms.Add(Resources.Load<TextAsset>("Batch1/LionRoom"));
        rooms.Add(Resources.Load<TextAsset>("Batch1/SnakeRoom"));
        rooms.Add(Resources.Load<TextAsset>("Batch1/MyLionRoom"));
        rooms.Add(Resources.Load<TextAsset>("Batch1/MySnakeRoom"));

        /*if (Directory.Exists(mapDirectory))
        {
            // This path is a directory
            ProcessDirectory(mapDirectory+currentTestBatchId, "map*", ref maps);
            ProcessDirectory(mapDirectory+currentTestBatchId, "room*", ref rooms);
        }*/
        LoadNewLevel();
        
    }

    public void UpdateKeyGUI()
    {
        keyText.text = "x" + Player.instance.keys.Count;
    }

    public void UpdateRoomGUI(int x, int y)
    {
        roomText.text = "Room: " + x/2 + "," + y/2;
    }

    public void ChangeMusic(AudioClip music)
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
        audioSource.clip = music;
        audioSource.loop = true;
        audioSource.Play();
    }
}
