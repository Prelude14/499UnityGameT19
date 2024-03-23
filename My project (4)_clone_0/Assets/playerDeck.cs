using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Mirror;

public class playerDeck : NetworkBehaviour
{
    //need access to player manager script that is unique to each client
    public PlayerManager PlayerManager;
    public static List<Card1> staticDeck = new List<Card1>();
    public static int deckSize = 40; //two decks combined should always equal 40 cards

    //this is used to make sure the cards come out the proper colour
    public static string playerColour;
    public string gameDeckCombo = "";
    public bool localGameStarted = false;
    public bool connectionError = false; //for debugging connecting to server

    //status message text for when player has started the game, --tells them to wait for other playert to do the same.
    public Text dealCardsStatusMessage;
    //get shuffle button game object in order to only allow it to be pressed once
    public GameObject dealButton;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(StartGame());

        //locate the PlayerManager in this Client and send the clients deck colour to the server so the server can deal the cards
        //NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        //PlayerManager = networkIdentity.GetComponent<PlayerManager>();

        //PlayerManager.CmdGetPlayerColours(playerColour); //send playermanager the deck colour to be used, once 2 clients have run this, the server should deal out car

    }

    public void onClick()
    {
        if (!localGameStarted && !connectionError) //check if game has started yet, as well if theres any errors and if not, dont do anything
        {
            //locate the PlayerManager in this Client and send the clients deck colour to the server so the server can deal the cards
            NetworkIdentity networkClientIdentity = NetworkClient.connection.identity;
            PlayerManager = networkClientIdentity.GetComponent<PlayerManager>();

            gameDeckCombo = PlayerManager.clientDecks;//get sync variable from network manager

            Debug.Log("client's gameCombo: " + gameDeckCombo + ", and colour :" + playerColour);

            //if game isn't started yet, and it doesn't have enough players yet for a valid combo when this button is clicked, then send our colour out to server, 
            if (gameDeckCombo.Equals("") || gameDeckCombo.Equals("BLACK") || gameDeckCombo.Equals("R") || gameDeckCombo.Equals("W") || gameDeckCombo.Equals("BLUE"))
            {
                PlayerManager.CmdGetPlayerColours(playerColour, networkClientIdentity); //send playermanager the deck colour to be used, once 2 clients have run this, the server should deal out car
                Debug.Log("Sent colour to server. Waiting for response... GameCombo now equals: " + PlayerManager.clientDecks);

                //change status message text in screen to tell first client that they have started the game, and they need to wait for the other player to start it as well.
                dealCardsStatusMessage.text = "COLOUR MIXING IN PROGRESS... (Waiting for other client to deal)...";
                //make button to deal inactive so that one client can't start game with 2 of its own colour as the deck and skip whatever the other client picked
                dealButton.SetActive(false);

                if (PlayerManager.deckShuffled == true) //if after sending our colour to server, the server created and shuffled the deck properly, we can now start the game
                {
                    //PlayerManager.CmdDraw(2, PlayerManager.clientDecks); //get server to deal out two cards for each client

                    //Debug.Log("Game started ");
                    //cmd draw also changed the sync var gamestarted bool, so now that game has started, this whole method shouldn't be able to do anything 
                    localGameStarted = true;
                }
            }
            else if (gameDeckCombo.Equals("Error 2: Too many players/strings."))
            {
                //PlayerManager.CmdCreateDeck();
                connectionError = true; //do nothing and exit
            }

        }
    }

    // Update is called once per frame --> This works the draw pile deck
    void Update()
    {
        //tell server to change size of deck and the text displaying how many cards are left in it, as well as deal new cards if turn has changed
        //PlayerManager.CmdUpdate();



        //reduce number of "visible" cards on the -deck- stack
        //staticDeck = PlayerManager.combinedDeck;

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
