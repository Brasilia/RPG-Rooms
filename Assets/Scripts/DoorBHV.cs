using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBHV : MonoBehaviour {

    //public GameManager gm;
	public int keyID;
    public bool isOpen;
	public Sprite lockedSprite;
	public Transform teleportTransform;
//	public int moveX;
//	public int moveY;
	[SerializeField]
	private DoorBHV destination;

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
            if (keyID == 0)
            {
                Player.instance.transform.position = destination.teleportTransform.position;
                RoomBHV parent = destination.transform.parent.GetComponent<RoomBHV>();
                Player.instance.AdjustCamera(parent.x, parent.y);
            }
            else if(isOpen)
            {
                Player.instance.transform.position = destination.teleportTransform.position;
                RoomBHV parent = destination.transform.parent.GetComponent<RoomBHV>();
                Player.instance.AdjustCamera(parent.x, parent.y);
                
                //TODO: Add some analytics to flag when the player openned the lock
            }
            else if (Player.instance.keys.Contains(keyID) && (Player.instance.usedKeys.Count < Player.instance.keys.Count))
            {
                Player.instance.usedKeys.Add(keyID);
                isOpen = true;

                Player.instance.transform.position = destination.teleportTransform.position;
                RoomBHV parent = destination.transform.parent.GetComponent<RoomBHV>();
                destination.isOpen = true;

                Player.instance.AdjustCamera(parent.x, parent.y);
            }
            /*if(parent.isEnd)
            {
                Debug.Log("The end");
                GameManager.state = GameManager.LevelPlayState.Won;
                //TODO change this to when the sierpinsk-force is taken
                gm.LevelComplete();
                return;
            }*/
		}
	}

	public void SetDestination(DoorBHV other){
		destination = other;
	}
}
