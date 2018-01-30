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

	private void ReadFile(string filepath){
		StreamReader streamReader = new StreamReader(filepath);

		while(!streamReader.EndOfStream){
			int x, y;
			string code;
			x = int.Parse(streamReader.ReadLine () );
			y = int.Parse(streamReader.ReadLine () );
			code = streamReader.ReadLine ();

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
					rooms [x, y].keyID = int.Parse (code);
					break;
				}
			} else { // corredor (link)
				if (code != "c"){
					rooms [x, y].lockID = -int.Parse (code);
				}
			}
		}
	}
}
