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

        if (!gameStarted) //check if game has started yet, if not, dont do anything
        {
            gameDeckCombo = PlayerManager.clientDecks;//get sync variable from network manager

            if (!gameDeckCombo.Equals("Error 1: Not enough players/strings.") || !gameDeckCombo.Equals("Error 2: Too many players/strings.") ) //if game started with valid combo, ask server to deal and then start game
            {
                PlayerManager.CmdDraw(2);
                gameStarted = true;
            }
            else if (gameDeckCombo.Equals("Error 1: Not enough players/strings.") ) //if game isn't started yet, and it doesn't have enough players yet for a valid combo, keep getting server to check each time update runs
            {
                PlayerManager.CmdCreateDeck();
            }
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
