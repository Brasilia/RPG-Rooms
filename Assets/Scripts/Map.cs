using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Map {

	public const int SIZE = 12;

	public Room[,] rooms;
	public int startX, startY;
	public int endX, endY;

	public Map(string filepath){
		rooms = new Room[SIZE,SIZE];
		ReadFile (filepath);

	}

//	public void Start(){
//		rooms = new Room[SIZE,SIZE];
//		Debug.Log (rooms [0, 0]);
//		ReadFile ("Assets/Data/data.txt");
//		Debug.Log (rooms [0, 0]);
//
//		for (int y = 0; y < SIZE; y++){
//			for (int x = 0; x < SIZE; x++){
//				Debug.Log (rooms [x, y]);
//			}
//		}
//	}

	private void ReadFile(string filepath){
		StreamReader streamReader = new StreamReader(filepath);

		while(!streamReader.EndOfStream){
			int x, y;
			string code;
			x = int.Parse(streamReader.ReadLine () );
			y = int.Parse(streamReader.ReadLine () );
			code = streamReader.ReadLine ();
			//Debug.Log ("x = " + x);
			//Debug.Log ("y = " + y);
			//Debug.Log ("code = " + code);
			rooms [x, y] = new Room (x, y);
			//Sala ou corredor(link)?
			if ((x%2)+(y%2) == 0){ // ambos pares: sala
				switch(code){
				case "s":
					startX = x;
					startY = y;
					break;
				case "B":
					endX = x;
					endY = y;
					break;
				default:
					//Debug.Log ("Default");
					rooms [x, y].keyID = int.Parse (code);
					break;
				}
			} else { // corredor (link)
				if (code != "c"){
					//Debug.Log ("Else code: " + code);
					rooms [x, y].lockID = -int.Parse (code);
				}
			}
		}
	}
}
