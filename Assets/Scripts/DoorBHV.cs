using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBHV : MonoBehaviour {

    //public GameManager gm;
	public int keyID;
	public Sprite lockedSprite;
	public Transform teleportTransform;
//	public int moveX;
//	public int moveY;
	[SerializeField]
	private DoorBHV destination;
    private RoomBHV parentRoom;

    private void Awake()
    {
        parentRoom = transform.parent.GetComponent<RoomBHV>();
    }

    // Use this for initialization
    void Start () {
		if (keyID < 0){
			Destroy (gameObject);
		} else if (keyID > 0){
            //Render the locked door sprite with the color relative to its ID
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.sprite = lockedSprite;
            sr.color = Util.colorId[keyID-1];
            //text.text = keyID.ToString ();
		}
        //gm = GameManager.instance;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player"){
			if (Player.instance.keys.Contains(keyID) || keyID == 0){ //changes room
				Player.instance.transform.position = destination.teleportTransform.position;
				RoomBHV parent = destination.parentRoom;
                if(Player.instance.keys.Contains(keyID) && !Player.instance.usedKeys.Contains(keyID))
                {
                    Player.instance.usedKeys.Add(keyID);
                    //TODO: Add some analytics to flag when the player openned the lock
                }
                /*if(parent.isEnd)
                {
                    Debug.Log("The end");
                    GameManager.state = GameManager.LevelPlayState.Won;
                    //TODO change this to when the sierpinsk-force is taken
                    gm.LevelComplete();
                    return;
                }*/
				Player.instance.AdjustCamera (parent.x, parent.y);
                if (keyID != 0)
                {
                    OnKeyUsed(keyID);
                }
			}
		}
	}

	public void SetDestination(DoorBHV other){
		destination = other;
	}

    //Methods to Player Profile
    private void OnRoomTryEnter ()
    {
        PlayerProfile.instance.OnRoomTryEnter(new Vector2Int(destination.parentRoom.x, destination.parentRoom.y));
    }

    private void OnRoomEnter ()
    {
        PlayerProfile.instance.OnRoomEnter(new Vector2Int(destination.parentRoom.x, destination.parentRoom.y));
    }

    private void OnRoomTryExit ()
    {
        PlayerProfile.instance.OnRoomTryExit(new Vector2Int(parentRoom.x, parentRoom.y));
    }

    private void OnRoomExit ()
    {
        PlayerProfile.instance.OnRoomExit(new Vector2Int(parentRoom.x, parentRoom.y));
    }

    private void OnKeyUsed (int id)
    {
        PlayerProfile.instance.OnKeyUsed(id);
    }


}
