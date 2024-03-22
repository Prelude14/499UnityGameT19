using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Mirror; //need this script to be outside of script folder in order for it to use mirror for some reason (EACH CARD SPAWNED has copy of this script)


public class dragScript : NetworkBehaviour
{
    public GameObject Canvas;
    public GameObject dropZone;//only playPanel
    private GameObject startParent;
    private Vector2 startPos;
    private bool isOverDropZone = false;

    private bool isDragging = false;
    public static bool isDraggable = true; //for dragging only your own cards
    public GameObject placeholder = null;

    //need access to player manager script that is unique to each client
    public PlayerManager PlayerManager;


    void Start()
    {
        Canvas = GameObject.Find("Canvas");

        if (!isOwned)
        {
            isDraggable = false; //if we don't own this card's version of this script, we shouldn't be able to drag it.
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log("Colliding");
        isOverDropZone = true;
        dropZone = collision.gameObject;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {

        Debug.Log("Not colliding");
        isOverDropZone = false;
        dropZone = null;
    }
    public void StartDrag()
    {

        if (isDraggable == false)
        {
            return; //if we can't drag the object, then don't do anything
        }
        else if (isDraggable == true) {
            startParent = transform.parent.gameObject; //find the parent of this transform and find that game object so it should be hand
            startPos = transform.position;
            isDragging = true;
            Debug.Log("DRAGGING!");
        }
        
    }
    public void EndDrag()
    {
        Debug.Log("Made it to End Drag()... isdraggable = ");
        if (isDraggable == false) //redundant check for draggablility again
        {
            return; //if we can't drag the object, then don't do anything
        }

        isDragging = false;
        if (isOverDropZone) //if player stops dragging over their own dropzone
        {
            transform.SetParent(dropZone.transform, false); //dropZone set inside oncollisionenter2d
            isDraggable = false; //once card dropped, can't remove it from playpanel

            //tell network we played card, by sending the card we played to our specific playerManager's playCard command
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            PlayerManager = networkIdentity.GetComponent<PlayerManager>();
            PlayerManager.PlayCard(gameObject); //passes its own game object to the playermanager object's script's PlayCard method

        }
        else
        {
            if (startParent == null)
            {
                transform.position = startPos;
                startParent = transform.parent.gameObject;
                return;
            }
            //not over dropzone, then send it back to start position
            transform.position = startPos;
            transform.SetParent(startParent.transform, false);
            transform.localScale = new Vector2(1,1);
        }

    }

    // Update is called once per frame
    void Update()
    {


        if (isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            transform.SetParent(Canvas.transform, true);
        }
    }


}
