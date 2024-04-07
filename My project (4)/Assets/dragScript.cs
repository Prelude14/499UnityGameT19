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

    public dbDisplay dbDisplay; //need to change values in each card's display script

    public int dragScriptwhosTurn = 0;//keep track of whos turn it is

    public GameObject currentCardZone; //check if its my card or not
    public GameObject hand;
    public GameObject oppHand;
    public GameObject playPanel;
    public GameObject oppPlayPanel;

    public bool checkCardPosition = false;
    public bool checkCardTurn = false;


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
    public bool checkMyCard()
    {
        //check all game objects that the card can be placed in when attempting to drag it
        hand = GameObject.Find("hand");
        oppHand = GameObject.Find("oppHand");
        oppPlayPanel = GameObject.Find("oppPlayPanel");
        playPanel = GameObject.Find("playPanel");

        dbDisplay = this.GetComponent<dbDisplay>(); //get this card's specific dbdisplay script in order to check if the card is playable or not

        currentCardZone = this.transform.parent.gameObject; //get the card that this drag script is attached to, and set the currentCardZone to the card's parent game object (it should be one of the four above)

        if (currentCardZone == oppHand) 
        {
            isDraggable = false; //can't drag cards in enemy hand
            return false;
        }
        else if (currentCardZone == oppPlayPanel)
        {
            isDraggable = false; //can't drag cards in enemy's playzone
            return false;
        }
        else if (currentCardZone == playPanel)
        {
            isDraggable = false; //can't drag cards once placed in your own play zone
            return false;
        }
        else if (currentCardZone == hand && dbDisplay.canBeSummoned == true)
        {
            isDraggable = true; //CAN ONLY DRAG CARDS THAT ARE IN YOUR HAND, AND THAT ARE SUMMONABLE***
            return true;
        }
        return false;
    }
    public void StartDrag()
    {
        checkCardTurn = checkTurn(); //sets isDraggable to appropriate value in order to allow drag or not (shouldn't be able to play cards if it isn't your turn)

        checkCardPosition = checkMyCard(); //need to check if the card i'm trying to drag is actually my card or not

        if (isDraggable == false || checkCardTurn == false || checkCardPosition == false)
        {
            return; //if we can't drag the object, then don't do anything
        }
        else if (isDraggable == true || checkCardTurn == true || checkCardPosition == true) {
            startParent = transform.parent.gameObject; //find the parent of this transform and find that game object so it should be hand
            startPos = transform.position;
            isDragging = true;
            Debug.Log("DRAGGING!");
        }
        
    }
    public bool checkTurn()
    {
        //find sharedvar game object in scene at runtime, CHECK for turn count
        GameObject sharedVarManagerObj = GameObject.Find("SharedVarManager");
        SharedVarManager sharedVarManager = sharedVarManagerObj.GetComponent<SharedVarManager>();

        dragScriptwhosTurn = sharedVarManager.whosTurn;//set dragScriptwhosTurn to be based off SharedVarManager's value

        if (dragScriptwhosTurn == 0)
        {
            //Debug.Log("Turn # 0 according to SharedVarManager");
            return false;
        }
        else if (dragScriptwhosTurn == 1) //its player one's turn
        {
            //Debug.Log("Player # 1's turn according to SharedVarManager");
            //locate the PlayerManager in the Client, need to check if it is player 1 or 2
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            PlayerManager = networkIdentity.GetComponent<PlayerManager>();

            if (PlayerManager.isPlayerOne == true && PlayerManager.isPlayerTwo == false) //if I'm player ONE and its PLAYER ONE'S Turn, then it IS my turn
            {
                isDraggable = true; //let card be draggable
                return true;
            }
            else if (PlayerManager.isPlayerTwo == true && PlayerManager.isPlayerOne == false) //if I'm player TWO and its PLAYER ONE'S Turn, then it is NOT my turn
            {
                isDraggable = false; //card should NOT be draggable, the startDrag() should fail
                return false;
            }

        }
        else if (dragScriptwhosTurn == 2)
        {
            //Debug.Log("Player # 2's turn according to SharedVarManager");
            //locate the PlayerManager in the Client, need to check if it is player 1 or 2
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            PlayerManager = networkIdentity.GetComponent<PlayerManager>();

            if (PlayerManager.isPlayerOne == true && PlayerManager.isPlayerTwo == false) //if I'm player ONE and its PLAYER TWO'S Turn, then it IS NOT my turn
            {
                isDraggable = false; //card should NOT be draggable, the startDrag() should fail
                return false;
            }
            else if (PlayerManager.isPlayerTwo == true && PlayerManager.isPlayerOne == false) //if I'm player TWO and its PLAYER TWO'S Turn, then it IS my turn
            {
                isDraggable = true; //let card be draggable
                return true;
            }
        }
        return false;
    }
    public void EndDrag()
    {
        //Debug.Log("Made it to End Drag()... isdraggable = ");
        checkCardTurn = checkTurn(); //redundant check for draggablility again 

        //checkCardPosition = checkMyCard(); //need to check if the card i'm trying to drag is actually my card or not

        if (isDraggable == false || checkCardTurn == false)//redundant check for draggablility again
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

            dbDisplay = this.GetComponent<dbDisplay>(); //get this card's specific dbdisplay script in order to send the card's cost to the server's updateManaCount method
            int manaCost = dbDisplay.cost;

            PlayerManager.PlayCard(gameObject, manaCost, networkIdentity); //passes its own game object to the playermanager object's script's PlayCard method, also passing the card's cost and its network identity to server

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
