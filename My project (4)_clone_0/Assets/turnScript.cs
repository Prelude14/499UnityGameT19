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
    public Text manaText;

    public static bool turnStart;

    public static int turnCount = 0;
    public GameObject playArrows;
    public GameObject attackArrows;

    //need access to player manager script that is unique to each client
    public PlayerManager PlayerManager;

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

            if (PlayerManager.isPlayerOne == true && PlayerManager.isPlayerTwo == false) //if I'm player ONE and its PLAYER ONE'S Turn, then it is my turn
            {
                isMyTurn = true;
                myTurn = 1;
                isTheirTurn = 0;
                maxMana = sharedVarManager.p1Mana; //set max to equal shared var manager's current count
                currentMana = maxMana; //update current mana to match full mana (instead of incrementing it? *********************)
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
                maxMana = sharedVarManager.p1Mana; //set max to equal shared var manager's current count
            }
            else if (PlayerManager.isPlayerTwo == true && PlayerManager.isPlayerOne == false) //if I'm player TWO and its PLAYER TWO'S Turn, then it IS my turn
            {
                isMyTurn = true;
                myTurn = 1;
                isTheirTurn = 0;
                maxMana = sharedVarManager.p2Mana; //set max to equal shared var manager's current count
                currentMana = maxMana; //update current mana to match full mana (instead of incrementing it? *********************)
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
        isMyTurn = false;
        isTheirTurn = 1;
        playArrows.SetActive(false);
        attackArrows.SetActive(false);
        disable = true;

        //turnCount++;
        updateTurnCount();
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
    }


}
