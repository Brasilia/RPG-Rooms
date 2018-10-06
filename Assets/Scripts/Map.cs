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

    // Valores para gerar salas sem o arquivo de definição interna
    public static int defaultRoomSizeX = 6;
    public static int defaultRoomSizeY = 6;
    public static int defaultTileID = 2;

	public Map(string text, string roomsFilePath = null){
		ReadMapFile (text); // lê o mapa global
        if (roomsFilePath != null){ // dá a opção de gerar o mapa com ou sem os tiles
            Debug.Log("Has Room File");
            ReadRoomsFile (roomsFilePath); // lê cada sala, com seus tiles
			Room.tiled = true; // o arquivo de tiles das salas foi lido; função de tiles ativada
		} else { // sala vazia padrão
            Debug.Log("Doesn't Have Room File");
            BuildDefaultRooms();
        }
	}

	private void ReadMapFile(string text){
        var splitFile = new string[] { "\r\n", "\r", "\n" };

        //StreamReader streamReaderMap = new StreamReader(filepath);
	    var NameLines = text.Split(splitFile, System.StringSplitOptions.RemoveEmptyEntries);

        //sizeX = int.Parse(streamReaderMap.ReadLine());
        //sizeY = int.Parse(streamReaderMap.ReadLine());
        sizeX = int.Parse(NameLines[0]);
        sizeY = int.Parse(NameLines[1]);

        //Debug.Log (filepath);
        Debug.Log("sizeX = " + sizeX + "   sizeY = " + sizeY);
        rooms = new Room[sizeX, sizeY];


        for (uint i = 2; i < NameLines.Length;)
        {
            int x, y;
            string code;
            x = int.Parse(NameLines[i++]);
            y = int.Parse(NameLines[i++]);
            code = NameLines[i++];

            rooms[x, y] = new Room(x, y);
            //Sala ou corredor(link)?
            if ((x % 2) + (y % 2) == 0)
            { // ambos pares: sala
                switch (code)
                {
                    case "s":
                        startX = x;
                        startY = y;
                        break;
                    case "B":
                        endX = x;
                        endY = y;
                        break;
                    default:
                        rooms[x, y].keyID = int.Parse(code);
                        break;
                }
            }
            else
            { // corredor (link)
                if (code != "c")
                {
                    rooms[x, y].lockID = -int.Parse(code);
                }
            }
        }
        
		Debug.Log ("Dungeon read.");
	}

	//Recebe os dados de tiles das salas
	private void ReadRoomsFile(string text){
        var splitFile = new string[] { "\r\n", "\r", "\n" };

        //StreamReader streamReaderMap = new StreamReader(filepath);
        var NameLines = text.Split(splitFile, System.StringSplitOptions.RemoveEmptyEntries);

        //sizeX = int.Parse(streamReaderMap.ReadLine());
        //sizeY = int.Parse(streamReaderMap.ReadLine());
        Room.sizeX = int.Parse(NameLines[0]);
        Room.sizeY = int.Parse(NameLines[1]);

        int txtLine = 3;
        for (uint i = 2; i < NameLines.Length;)
        {
            int roomX, roomY;
			roomX = int.Parse(NameLines[i++]);
			roomY = int.Parse(NameLines[i++]);
			txtLine += 2;
			//Debug.Log ("roomX " + roomX + "   roomY " + roomY + "   Line: " + txtLine);
			rooms [roomX, roomY].InitializeTiles (); // aloca memória para os tiles
			for (int x = 0; x < Room.sizeX; x++){
                for (int y = 0; y < Room.sizeY; y++)
                {
                    rooms[roomX, roomY].tiles [x, y] = int.Parse(NameLines[i++]); // FIXME Desinverter x e y: foi feito assim pois o arquivo de entrada foi passado em um formato invertido
					txtLine++;
				}
			}
		}
		Debug.Log ("Rooms read.");
	}

    //Cria salas vazias no tamanho padrão
    private void BuildDefaultRooms ()
    {
        Room.sizeX = defaultRoomSizeX;
        Room.sizeY = defaultRoomSizeY;
        for (int roomX = 0; roomX < sizeX; roomX+=2)
        {
            for (int roomY = 0; roomY < sizeY; roomY+=2)
            {
                if (rooms[roomX, roomY] == null)
                    continue;
                rooms[roomX, roomY].InitializeTiles(); // aloca memória para os tiles
                for (int x = 0; x < Room.sizeX; x++)
                {
                    for (int y = 0; y < Room.sizeY; y++)
                    {
                        rooms[roomX, roomY].tiles[x, y] = defaultTileID; // FIXME Desinverter x e y: foi feito assim pois o arquivo de entrada foi passado em um formato invertido
                    }
                }
            }
        }
    }
}
