using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfile : MonoBehaviour {

    public static PlayerProfile instance = null;

    private int mapCount = 0;
    private int curMapId;

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
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //From DoorBHV
    public void OnRoomTryEnter(Vector2Int offset)
    {
        //Log
        //Mais métricas - organiza em TAD
    }

    //From DoorBHV
    public void OnRoomEnter (Vector2Int offset)
    {
        //Log
        //Mais métricas - organiza em TAD
    }

    //From DoorBHV
    public void OnRoomTryExit(Vector2Int offset)
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
    }

    //From KeyBHV
    public void OnGetKey (int id)
    {
        //Log
        //Mais métricas - organiza em TAD
    }
}
