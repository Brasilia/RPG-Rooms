using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Map {

	public static int sizeX;
	public static int sizeY;

	public Room[,] rooms;
	public int startX, startY;
	public int endX, endY;

	public Map(string filepath, string roomsFilePath = null){
		ReadMapFile (filepath); // lê o mapa global
		if (roomsFilePath != null){ // dá a opção de gerar o mapa com ou sem os tiles
			ReadRoomsFile (roomsFilePath); // lê cada sala, com seus tiles
			Room.tiled = true; // o arquivo de tiles das salas foi lido; função de tiles ativada
		}
	}

	private void ReadMapFile(string filepath){
		StreamReader streamReaderMap = new StreamReader(filepath);
		sizeX = int.Parse (streamReaderMap.ReadLine ());
		sizeY = int.Parse (streamReaderMap.ReadLine ());
		Debug.Log (filepath);
		Debug.Log ("sizeX = " + sizeX + "   sizeY = " + sizeY);
		rooms = new Room[sizeX, sizeY];

		while(!streamReaderMap.EndOfStream){
			int x, y;
			string code;
			x = int.Parse(streamReaderMap.ReadLine () );
			y = int.Parse(streamReaderMap.ReadLine () );
			code = streamReaderMap.ReadLine ();

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
		Debug.Log ("Dungeon read.");
	}

	//Recebe os dados de tiles das salas
	private void ReadRoomsFile(string filepath){
		StreamReader streamReaderRoom = new StreamReader (filepath);
		Room.sizeX = int.Parse( streamReaderRoom.ReadLine () );
		Room.sizeY = int.Parse( streamReaderRoom.ReadLine () );
		int txtLine = 3;
		while (!streamReaderRoom.EndOfStream){
			int roomX, roomY;
			roomX = int.Parse( streamReaderRoom.ReadLine () );
			roomY = int.Parse( streamReaderRoom.ReadLine () );
			txtLine += 2;
			Debug.Log ("roomX " + roomX + "   roomY " + roomY + "   Line: " + txtLine);
			rooms [roomX, roomY].InitializeTiles (); // aloca memória para os tiles
			for (int x = 0; x < Room.sizeX; x++){
				for (int y = 0; y < Room.sizeY; y++){
					rooms [roomX, roomY].tiles [y, x] = int.Parse( streamReaderRoom.ReadLine () ); // FIXME Desinverter x e y: foi feito assim pois o arquivo de entrada foi passado em um formato invertido
					txtLine++;
				}
			}
		}
		Debug.Log ("Rooms read.");
	}
}
