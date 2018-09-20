using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerProfile : MonoBehaviour {

    public static PlayerProfile instance = null;

    private const string PostDataURL = "http://jogos.icmc.usp.br/pag/data/upload.php?";
    private int attemptNumber = 1; //TODO: entender o por quê desse int

    private string sessionUID;
    private string profileString;

    private int mapCount = 0;
    private int curMapId;

    private List<Vector2Int> visitedRooms = new List<Vector2Int>();
    private int mapVisitedCount = 0;
    private int mapVisitedCountUnique = 0;
    private int keysTaken = 0;
    private int keysUsed = 0;


    void Awake()
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        // FIXME: utilizar uma ID única corretamente
        string dateTime = System.DateTime.Now.ToString();
        dateTime = dateTime.Replace("/", "-");
        sessionUID = Random.Range(0, 99).ToString("00");
        sessionUID += "_";
        sessionUID += dateTime;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //Events
    //From DoorBHV
    public void OnRoomFailEnter(Vector2Int offset)
    {
        //Log
        //Mais métricas - organiza em TAD
    }

    //From DoorBHV
    public void OnRoomEnter (Vector2Int offset)
    {
        //Log
        //Mais métricas - organiza em TAD
        visitedRooms.Add(offset);
    }

    //From DoorBHV
    public void OnRoomFailExit(Vector2Int offset)
    {
        //Log
        //Mais métricas - organiza em TAD
    }

    //From DoorBHV
    public void OnRoomExit(Vector2Int offset)
    {
        //Log
        //Mais métricas - organiza em TAD
    }

    //From DoorBHV
    public void OnKeyUsed(int id)
    {
        //Log
        keysUsed++;
        //Mais métricas - organiza em TAD
    }

    //From GameManager
    public void OnMapStart (int id)
    {
        mapCount++;
        curMapId = id;
        //Log
        //Mais métricas - organiza em TAD
    }

    //From inheritance
    private void OnApplicationQuit()
    {
        //Log
    }

    //From TriforceBHV
    public void OnMapComplete ()
    {
        //Log
        //Mais métricas - organiza em TAD, agrega dados do nível
        //visitedRooms = visitedRooms.Distinct();
        mapVisitedCount = visitedRooms.Count;
        mapVisitedCountUnique = visitedRooms.Distinct().Count();
        //Save to remote file
        SendProfileToServer();
        //Reset all values
        visitedRooms.Clear();
        keysTaken = 0;
        keysUsed = 0;
        profileString = "";
    }

    //From KeyBHV
    public void OnGetKey (int id)
    {
        //Log
        keysTaken++;
        //Mais métricas - organiza em TAD
    }

    private void WrapProfileToString ()
    {
        profileString = "";
        profileString += mapVisitedCount + "," + mapVisitedCountUnique + "," + keysTaken + "," + keysUsed;
    }

    private void SendProfileToServer ()
    {
        WrapProfileToString();
        StartCoroutine(PostData(sessionUID+curMapId.ToString(), profileString)); //TODO: verificar corretamente como serão salvos os arquivos
    }

    IEnumerator PostData(string name, string stringData)
    {
        byte[] data = System.Text.Encoding.UTF8.GetBytes(stringData);
        //This connects to a server side php script that will write the data
        //string post_url = postDataURL + "name=" + WWW.EscapeURL(name) + "&data=" + data ;
        string post_url = PostDataURL;

        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddBinaryData("data", data, name + "_" + attemptNumber + ".txt", "text/plain");


        // Post the URL to the site and create a download object to get the result.
        WWW data_post = new WWW(post_url, form);
        yield return data_post; // Wait until the download is done

        if (data_post.error != null)
        {
            print("There was an error saving data: " + data_post.error);
        }
        else
        {
            Debug.Log("Upload complete!");
        }
    }


}
