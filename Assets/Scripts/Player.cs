using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public static Player instance = null;
	public List<int> keys = new List<int>();
	public int x { private set;  get;}
	public int y { private set;  get;}
	public Camera cam;

	void Awake(){
		if (instance == null){
			instance = this;
		} else if (instance != this){
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GetKey(int keyID){
		keys.Add (keyID);
	}
		
	public void SetPosition(int x, int y){
		GameManager gm = GameManager.instance;
		Transform roomTransf = gm.roomBHVMap [x, y].transform;
		cam.transform.position = new Vector3 (roomTransf.position.x, roomTransf.position.y, -10f);
		Vector2 roomCenter = (Vector2)roomTransf.position - new Vector2(0.5f, 0.5f);
		transform.position = roomCenter;

		//TODO arrumar daqui pra baixo, pra procurar um espaço vazio para o player
//		int centerX = (int)(roomCenter.x +0.5f); //- Room.sizeX;
//		int centerY = (int)(roomCenter.y +0.5f); //- Room.sizeY;
//		int searchRange = 0;
//		while ( gm.GetMap().rooms[x, y].tiles[(int)transform.position.x - centerX, (int)transform.position.y - centerY] == 1 ){ //enquanto a posição do jogador coincidir com um tile não-passável
//			Debug.Log("Search Range: " + searchRange);
//			for (int i = -searchRange; 
//				gm.GetMap ().rooms [x, y].tiles [(int)transform.position.x - centerX, (int)transform.position.y - centerY] == 1 &&
//				i <= searchRange; i++) {
//				for (int j = -searchRange; 
//					gm.GetMap ().rooms [x, y].tiles [(int)transform.position.x - centerX, (int)transform.position.y - centerY] == 1 &&
//					j <= searchRange; j++) {
//					transform.position = roomCenter + new Vector2 (i, j);
//				}
//			}
//			searchRange++; //TODO transformar o while em um for
//		}

	}
}
