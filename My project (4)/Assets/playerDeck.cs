using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Mirror;

public class playerDeck : NetworkBehaviour
{
    //need access to player manager script that is unique to each client
    public playerManager PlayerManager;
    public static List<Card1> staticDeck = new List<Card1>();
    public static int deckSize = 40; //two decks combined should always equal 40 cards

    //this is used to make sure the cards come out the proper colour
    public static string playerColour;
    public string gameDeckCombo = "";
    public bool gameStarted = false;
<<<<<<< Updated upstream
=======
    public bool sent1 = false;
>>>>>>> Stashed changes

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(StartGame());

        //locate the PlayerManager in this Client and send the clients deck colour to the server so the server can deal the cards
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<playerManager>();

        PlayerManager.CmdGetPlayerColours(playerColour); //send playermanager the deck colour to be used, once 2 clients have run this, the server should deal out car

    }

    // Update is called once per frame --> This works the draw pile deck
    void Update()
    {
        //locate the PlayerManager in this Client and send the clients deck colour to the server so the server can deal the cards
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<playerManager>();

        //tell server to change size of deck and the text displaying how many cards are left in it, as well as deal new cards if turn has changed
        //PlayerManager.CmdUpdate();

<<<<<<< Updated upstream
        if (!gameStarted) //check if game has started yet, if not, dont do anything
        {
            gameDeckCombo = PlayerManager.clientDecks;//get sync variable from network manager

            if (!gameDeckCombo.Equals("Error 1: Not enough players/strings.") || !gameDeckCombo.Equals("Error 2: Too many players/strings.") ) //if game started with valid combo, ask server to deal and then start game
=======

        if (!gameStarted) //check if game has started yet, if not, dont do anything
        {

            gameDeckCombo = PlayerManager.clientDecks;//get sync variable from network manager
           

            //on update each frame, check if client deck string is still empty, meaning no one has sent their colour to server yet, then send your colour
            if (PlayerManager.clientDecks.Equals("") && sent1 == false)
            {
                PlayerManager.servercheckchangecombo(playerColour); //send playermanager the deck colour to be used, once 2 clients have run this, the server should deal out car
                sent1 = true;
                //Debug.Log("client sent this colour to server 1 time:" + playerColour);//check to see it updating
            }
            else if (sent1 == true && ( PlayerManager.clientDecks.Equals("BLACK") || PlayerManager.clientDecks.Equals("R") || PlayerManager.clientDecks.Equals("W") || PlayerManager.clientDecks.Equals("BLUE") ) )
            {
                //if client has already sent its colour to server, and there is one colour inside the shared string, then this client will wait until the other player connects
            }
            else if (!PlayerManager.clientDecks.Equals("Error 1: Not enough players/strings.") || !PlayerManager.clientDecks.Equals("Error 2: Too many players/strings.") ) //if game started with valid combo, ask server to deal and then start game
>>>>>>> Stashed changes
            {
                PlayerManager.CmdDraw(2);
                gameStarted = true;
            }
<<<<<<< Updated upstream
            else if (gameDeckCombo.Equals("Error 1: Not enough players/strings.") ) //if game isn't started yet, and it doesn't have enough players yet for a valid combo, keep getting server to check each time update runs
            {
                PlayerManager.CmdCreateDeck();
            }
=======
            //else if (gameDeckCombo.Equals("Error 1: Not enough players/strings.") ) //if game isn't started yet, and it doesn't have enough players yet for a valid combo, keep getting server to check each time update runs
            //{
            //    PlayerManager.CmdCreateDeck();
            //}
>>>>>>> Stashed changes
        }

        //reduce number of "visible" cards on the -deck- stack
        staticDeck = PlayerManager.combinedDeck;

        /*//display deck card count;
        deckCountText.text = "" + deckSize;
        if (turnScript.turnStart == true)
        {
            StartCoroutine(Draw(1));
            turnScript.turnStart = false;
        }
        */
    }

}
