using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class turnScript : NetworkBehaviour
{
    public int playerNumTurn = 0;
    public static bool isMyTurn;
    public int myTurn;
    public int isTheirTurn;
    public Text turnText;
    public bool disable;
    public static int totalSummons;

    public int maxMana;

    public static int currentMana;
    public TMPro.TMP_Text manaText;

    public static bool turnStart;

    public static int turnCount = 0;
    public GameObject playArrows;
    public GameObject attackArrows;

    public static float p1StartingHP; // turn start HP
    public static float p2StartingHP; // turn start HP
    //need access to player manager script that is unique to each client
    public PlayerManager PlayerManager;

    public int playerNumber;

    // Start is called before the first frame update
    void Start()
    {
        //game start your turn set mana to 1
        isMyTurn = false; //start as false so isn't set until two clients are connected properly
        myTurn = 0;
        isTheirTurn = 0;

        maxMana = 1;
        currentMana = 1;
        turnStart = false;
        disable = false;
    }

    public void SetUpTurns()
    {
        //find sharedvar game object in scene at runtime, CHECK for turn count
        GameObject sharedVarManagerObj = GameObject.Find("SharedVarManager");
        SharedVarManager sharedVarManager = sharedVarManagerObj.GetComponent<SharedVarManager>();

        playerNumTurn = sharedVarManager.whosTurn;//set playerNumTurn to be based off SharedVarManager's value

        turnCount = sharedVarManager.serverTurnCount;//set turn count to match server value version

        if (playerNumTurn == 0)
        {
            //Debug.Log("Turn # 0 according to SharedVarManager");
        }
        else if (playerNumTurn == 1) //its player one's turn
        {
            //Debug.Log("Player # 1's turn according to SharedVarManager");
            //locate the PlayerManager in the Client, need to check if it is player 1 or 2
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            PlayerManager = networkIdentity.GetComponent<PlayerManager>();

            //set player number locally
            if(PlayerManager.isPlayerOne){playerNumber=1;}else if(PlayerManager.isPlayerTwo){playerNumber=2;}

            if (PlayerManager.isPlayerOne == true && PlayerManager.isPlayerTwo == false) //if I'm player ONE and its PLAYER ONE'S Turn, then it is my turn
            {
                isMyTurn = true;
                myTurn = 1;
                isTheirTurn = 0;
                 maxMana = SharedVarManager.p1MaxMana; //set max to equal shared var manager's current count
                currentMana = SharedVarManager.p1StaticMana; //update current mana to match full mana (instead of incrementing it? *********************)
            }
            else if (PlayerManager.isPlayerTwo == true && PlayerManager.isPlayerOne == false) //if I'm player TWO and its PLAYER ONE'S Turn, then it is not my turn
            {
                isMyTurn = false;
                myTurn = 0;
                isTheirTurn = 1;
                //maxMana = sharedVarManager.p1Mana; //set max to equal shared var manager's current count
            }

        }
        else if (playerNumTurn == 2)
        {
            //Debug.Log("Player # 2's turn according to SharedVarManager");
            //locate the PlayerManager in the Client, need to check if it is player 1 or 2
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            PlayerManager = networkIdentity.GetComponent<PlayerManager>();

            if (PlayerManager.isPlayerOne == true && PlayerManager.isPlayerTwo == false) //if I'm player ONE and its PLAYER TWO'S Turn, then it IS NOT my turn
            {
                isMyTurn = false;
                myTurn = 0;
                isTheirTurn = 1;
                 maxMana = SharedVarManager.p1StaticMana; //set max to equal shared var manager's current count
            }
            else if (PlayerManager.isPlayerTwo == true && PlayerManager.isPlayerOne == false) //if I'm player TWO and its PLAYER TWO'S Turn, then it IS my turn
            {
                isMyTurn = true;
                myTurn = 1;
                isTheirTurn = 0;
                    maxMana = SharedVarManager.p2MaxMana; //set max to equal shared var manager's current count
                currentMana = SharedVarManager.p2StaticMana; //update current mana to match full mana (instead of incrementing it? *********************)
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetUpTurns(); //find out if its my turn or not

        if (isMyTurn == true)
        {
            turnText.text = "Your turn";
            
        }
        else
        {
            turnText.text = "Opponent's turn";
        }
        manaText.text = currentMana + "/" + maxMana;

        if (turnCount == 0)
        {
            playArrows.SetActive(true);

        }
        else
        {
            playArrows.SetActive(false);
            attackArrows.SetActive(false);
        }

        if (totalSummons == 1 && disable == false)
        {
            attackArrows.SetActive(true);
        }
        else
        {
            attackArrows.SetActive(false);
        }

    }
    public void updateTurnCount()
    {
        //locate the PlayerManager in the Client, need to check if it is player 1 or 2
        NetworkIdentity networkTurnIdentity = NetworkClient.connection.identity;
        PlayerManager = networkTurnIdentity.GetComponent<PlayerManager>();

        PlayerManager.CmdSendTurnInfo(networkTurnIdentity);//call playermanager's cmd that calls server's update turn count method, and send who has pressed the button
        Debug.Log("endturn button pressed...updateturnCount sent Cmd...");

        //turnStart = PlayerManager.myTurnStart;//set this to update properly when updateTurn is triggered in place of the end opponent's turn method.
        
    }

    public void endTurn()
    {
        bool itsMyTurn = checkTurnInScript(); //check if it is the client that pressed the end turn button's turn before actually changing the turn values and dealing a new card to the client

        if (itsMyTurn == true) //*** It must be the client's turn in order for the button to actually change the hands and deal a card
        {
            isMyTurn = false;
            isTheirTurn = 1;
            playArrows.SetActive(false);
            attackArrows.SetActive(false);
            disable = true;

            //turnCount++;
            updateTurnCount();
            //redundantly locate the PlayerManager in the Client, need to call our specific version of playermanager's CmdDraw
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            PlayerManager = networkIdentity.GetComponent<PlayerManager>();
            PlayerManager.CmdDraw(1, PlayerManager.clientDecks);  //ONLY DEAL NEW CARD IF IT IS ACTUALLY MY TURN

            //update the health of each player at the end of each turn
            p1StartingHP = SharedVarManager.p1HP;
            p2StartingHP = SharedVarManager.p2HP;
        }
        //if itsMyTurn is not true, then nothing should happen when button is clicked
    }

    public bool checkTurnInScript()
    {
        //find sharedvar game object in scene at runtime, CHECK for turn count
        GameObject sharedVarManagerObj = GameObject.Find("SharedVarManager");
        SharedVarManager sharedVarManager = sharedVarManagerObj.GetComponent<SharedVarManager>();

        int turnScriptwhosTurn = sharedVarManager.whosTurn;//set turnScriptwhosTurn to be based off SharedVarManager's value

        if (turnScriptwhosTurn == 0)
        {
            //Debug.Log("Turn # 0 according to SharedVarManager");
            return false;
        }
        else if (turnScriptwhosTurn == 1) //its player one's turn
        {
            //Debug.Log("Player # 1's turn according to SharedVarManager");
            //locate the PlayerManager in the Client, need to check if it is player 1 or 2
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            PlayerManager = networkIdentity.GetComponent<PlayerManager>();

            if (PlayerManager.isPlayerOne == true && PlayerManager.isPlayerTwo == false) //if I'm player ONE and its PLAYER ONE'S Turn, then it IS my turn
            {
                //isDraggable = true; //let card be draggable
                return true;
            }
            else if (PlayerManager.isPlayerTwo == true && PlayerManager.isPlayerOne == false) //if I'm player TWO and its PLAYER ONE'S Turn, then it is NOT my turn
            {
                //isDraggable = false; //card should NOT be draggable, the startDrag() should fail
                return false;
            }

        }
        else if (turnScriptwhosTurn == 2)
        {
            //Debug.Log("Player # 2's turn according to SharedVarManager");
            //locate the PlayerManager in the Client, need to check if it is player 1 or 2
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            PlayerManager = networkIdentity.GetComponent<PlayerManager>();

            if (PlayerManager.isPlayerOne == true && PlayerManager.isPlayerTwo == false) //if I'm player ONE and its PLAYER TWO'S Turn, then it IS NOT my turn
            {
                //isDraggable = false; //card should NOT be draggable, the startDrag() should fail
                return false;
            }
            else if (PlayerManager.isPlayerTwo == true && PlayerManager.isPlayerOne == false) //if I'm player TWO and its PLAYER TWO'S Turn, then it IS my turn
            {
                //isDraggable = true; //let card be draggable
                return true;
            }
        }
        return false; //default returns false 
    }

    public void endOpponentTurn()
    {
        isMyTurn = true;
        myTurn = 1;
        maxMana += 1;
        currentMana = maxMana;
        turnStart = true;
        dbDisplay.hasAttacked = false;
        if (totalSummons < 1)
        {
            disable = false;
        }

        playerHealth.turnStartHealth = (int)playerHealth.HPStatic; //updates turnstart health to track how much life they started w/
        Debug.Log("Your health is at: " + playerHealth.turnStartHealth);
        
    }


}