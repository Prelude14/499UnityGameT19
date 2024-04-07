using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror; //need this script to be outside of script folder in order for it to use mirror for some reason
//using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour
{
    //bringing playerdeck script into this script

    //playermanager will spawn these on the clients when they click join as host or client
    //public GameObject[] clones;
    public GameObject hand; //this is the player's hand object
    public GameObject oppHand;
    public GameObject playPanel; //this is where the player can play their cards
    public GameObject oppPlayPanel;
    public GameObject cardInHand; //need for dealing cards

    //need lists to store and manipulate the different decks
    //public List<Card1> container = new List<Card1>(); //temp list for shuffling deck
    public int pM_DeckSize = 40; //two decks combined should always equal 40 cards
    //public List<Card1> combinedDeck = new List<Card1>(); //list of card1 game objects for combined deck

    //============================= THIS IS THE PROBLEM VARIABLE =====================================================================================================================
    [SyncVar(hook = nameof(OnMySyncClientDeckChanged))] public string clientDecks;//need to sync a string variable between client and servers that will tell server which game decks to use
    //hook is for a method specific to the sync var to forcibly change the sync var across everything (at least how I understand it)

    public bool deckShuffled = false; //need to track if game deck was made properly

    [SyncVar] public bool isPlayerOne = false; //need to sync who is player one or two
    [SyncVar] public bool isPlayerTwo = false; //redudant second bool for clarity

    //[SyncVar] public bool isPlayerManagersTurn = false; //need to sync if it is my turn or not


    public override void OnStartClient() //if our playermanager is acting as a client
    {
        base.OnStartClient();

        Debug.Log("Client Started and connected.");

        //find client side game objects at run time (they are spawned in by player manager)
        hand = GameObject.Find("hand");
        oppHand = GameObject.Find("oppHand");
        playPanel = GameObject.Find("playPanel");
        oppPlayPanel = GameObject.Find("oppPlayPanel");

        //CmdGetPlayerColours(playerDeck.playerColour); //each client will send their chosen deck colour to server when they connect
        //OnMySyncIntChanged(clientDeck, clientDeck);//attempting to get new clients to load correct version of synced string when they join (otherwise thee second client overwrites the string when they join, and game never starts)
    }

    //this "hook" is a method that gets called automatically whenever the sync var its hooked to is changed, and it is supposed to update the value across all clients.
    void OnMySyncClientDeckChanged(string oldValue, string newValue)
    {
        // Do something
        oldValue = newValue;
        Debug.Log("Old clientDeck = " + oldValue + ", New clientDeck = " + newValue);
    }



    [Server]
    public override void OnStartServer() //if our playermanager is acting as a server
    {
        base.OnStartServer();

        Debug.Log("Server Started.");
        //start waiting for at least 2 clients to update the sync variable string -- it tells the server what colour decks to mix
        //while (checkFor2PlayersAndTheirCombo().Equals("Error 1: Not enough players/strings.") || checkFor2PlayersAndTheirCombo().Equals("Error 2: Too many players/strings.") ) {}

        clientDecks = ""; //start clientDecks as empty string on server
        //once 2 clients have connected and they have a valid combo, the server can populate the combinedDeck using the proper combo
        //WANT SERVER TO CREATE DECK, AND THEN DEAL CARDS TO CLIENT USING RPC
        //CmdCreateDeck();//attempt to create deck //this is done inside the cmdplayercolours command now by the server after the second client's colour is recieceved and stored properly.

    }

    [Server]
    public override void OnStopServer() //if our playermanager is acting as a server
    {
        Debug.Log("Server Stopped."); //for some reason if a client disconnects, the host still recieves this debug message saying the server stopped, even though a client stopped, and supposedly, only the server can run this

    }

    //Client CMD to update the combined deck on the server using the sync variable's string (each client passes its unique deck colour on start)
    [Command(requiresAuthority = false)] //added the authority bit in the process of debugging, defaults to true, so I think only the owner's version of their clientDeck will be changed unless this is here (but haven't really seen proof yet)
    public void CmdGetPlayerColours(string clientColour, NetworkIdentity networkClientIdentity)
    {
        Debug.Log("Server's clientDeck string at start of cmd call:" + clientDecks);

        //find sharedvar game object in scene at runtime (for deck colour strings and game start booleans
        GameObject sharedVarManagerObj = GameObject.Find("SharedVarManager");

        // Check if the SharedVarManager GameObject is found AND 
        if (sharedVarManagerObj != null)
        {
            SharedVarManager sharedVarManager = sharedVarManagerObj.GetComponent<SharedVarManager>();
            //Call the command on the NetworkManager

            sharedVarManager.CmdGetPlayerColor(clientColour, networkClientIdentity); // this calls the SharedVarManager script, which should handle the game logic from here, and deal out cards properly
            //clientDecks = clientColour; //set sync var to your client's colour so that the PlayerManager can now see and track across network what colour the client chose specifically (there is one instance of this per player)
            //playerDeck.staticDeck = sharedVarManager;
            //sharedVarManager.setPlayerNum(); //after sending their colour, ask sharedVarManager check who is first player
        }
        else
        {
            Debug.Log("SharedVarManager GameObject not found!"); //couldn't send colour properly
        }

    }

    [Command]                                       //clients request the server to deal them their cards
    public void CmdDraw(int drawSize, string drawCheckCombo)
    {
        //find sharedvar game object in scene at runtime (for deck colour strings and game start booleans
        GameObject sharedVarManagerObj = GameObject.Find("SharedVarManager");

        SharedVarManager sharedVarManager = sharedVarManagerObj.GetComponent<SharedVarManager>();

        //want clients to wait here until server has created and shuffled a VALID deck -- bool sent by SharedVarManager AFTER DECK IS VALIDATED and created properly (in RPC call)
        //while (!deckShuffled)
        //{
        //    //if server hasn't shuffled a valid deck yet, stay inside while loop, doing nothing
        //}

        //Double check enough players have joined and the server has recieved a valid combo of colours from the clients to deal decks, go ahead and deal the client their two cards
        string drawCombo = sharedVarManager.checkFor2PlayersAndTheirCombo(drawCheckCombo); //REDUNDANT

        pM_DeckSize = sharedVarManager.deckSize; //get server's decksize value

        if (!drawCombo.Equals("Error 1: Not enough players/strings.") && !drawCombo.Equals("Error 2: Too many players/strings."))
        {
            if (pM_DeckSize > 0) //check if there's other cards. do not draw if no more cards do smth else;
            {
                //draw cards repeat until drawSize
                for (int x = 0; x < drawSize; x++)
                {
                    //slow down code so we don't draw too fast
                    //yield return new WaitForSeconds(.15F);
                    //Instantiate(cardInHand, transform.position, transform.rotation);
                    Debug.Log("client called draw command, so server is attempting to draw each card into their hand..."); //
                    GameObject card = Instantiate(cardInHand, new Vector2(0, 0), Quaternion.identity); //Server instantiates card object on the client side of client who requested CmdDraw
                    NetworkServer.Spawn(card, connectionToClient); //Server spawns object across network for other clients to use, and gives Client the authority of the object (their card in this case)
                    RpcShowCard(card, "Dealt"); //this gets server to display the card object across both clients (and it displays specificly based on who has authority inside the rpc method)
                                                //renderCardColour(card);
                    if (SharedVarManager.staticTurn == 1)
                    {
                        SharedVarManager.p1TotalDraw += drawSize;
                    }
                    else
                    {
                        SharedVarManager.p2TotalDraw += drawSize;
                    }
                    sharedVarManager.deckSize -= 1; //take away one card from the deckSize each time this loop deals a card
                }
                //sharedVarManager.gameStarted = true; //server should sync this value to all clients, and once 
            }
            else
            {
                //lose game
                //TODO: Add a lose game

            }
        }
    }
    //method for when player plays a card
    public void PlayCard(GameObject card, int manaCost, NetworkIdentity networkPlayIdentity)
    {
        CmdPlayCard(card, manaCost, networkPlayIdentity); //call server command
    }
    [Command(requiresAuthority = false)]
    void CmdPlayCard(GameObject card, int manaCost, NetworkIdentity networkPlayIdentity) //just run rpc on clients to show card as played now, also tell server to update mana of the player who played the card
    {
        //find sharedvar game object in scene at runtime (for deck colour strings and game start booleans
        GameObject sharedVarManagerObj = GameObject.Find("SharedVarManager");

        // Check if the SharedVarManager GameObject is found AND 
        if (sharedVarManagerObj != null)
        {
            SharedVarManager sharedVarManager = sharedVarManagerObj.GetComponent<SharedVarManager>();
            //Call the command on the NetworkManager

            sharedVarManager.CmdUpdateManaCount(manaCost, networkPlayIdentity); //call server's updateManaCount CMD to update mana variables accordingly
            Debug.Log("Card Played Info recieved from dragscript...manaCost sent to server's CmdUpdateManaCount...");
        }
        else
        {
            Debug.Log("SharedVarManager GameObject not found! In CmdplayCard"); //couldn't send attack info properly
        }

        RpcShowCard(card, "Played"); //actually tell clients about card played

    }

    //this method is requested by the Server to run on all Clients, and it decides how to display the cards being dealt or played in the game
    [ClientRpc]
    void RpcShowCard(GameObject card, string type)
    {
        hand = GameObject.Find("hand");//each client should find its version of hand and opphand to place the card into.
        oppHand = GameObject.Find("oppHand");

        playPanel = GameObject.Find("playPanel");//redundant check to force each client to find thier own versions of these gameobjects for cards played
        oppPlayPanel = GameObject.Find("oppPlayPanel");

        //check type and then the authority of the card to see if the card has been "Dealt" to the client, and display the dealt card according to if the client owns it or not
        if (type.Equals("Dealt"))
        {
            Debug.Log("isOwned = " + isOwned);
            if (isOwned)
            {
                Debug.Log("client 'owns' this card, and we are setting its parent to be the hand's transform...");
                card.transform.SetParent(hand.transform, false); //if its the client's card, put it in the player's hand
            }
            else
            {
                Debug.Log("client DOESN'T 'own' this card, and we are setting its parent to be the oppHand's transform...");
                //card.GetComponent<dbDisplay>();
                //dbDisplay.staticCardBack = true;
                card.transform.SetParent(oppHand.transform, false); //if its not, put it in the enemy's hand object
                                                                    //card.GetComponent<CardFlipper>().Flip(); //show the back of the card 
            }
        }
        //if the card has been "Played", check which client played it, and display it accordingly on each client's side
        else if (type.Equals("Played"))
        {
            Debug.Log("Made it to RPC Play card...");
            if (!isOwned)
            {
                card.transform.localScale = new Vector2((float).75, (float).75);
                card.transform.SetParent(oppPlayPanel.transform, false); //enemy play panel
                                                                         //card.GetComponent<CardFlipper>().Flip(); //show correct side of card when played
            }
            else
            {
                card.transform.localScale = new Vector2((float).75, (float).75);
                card.transform.SetParent(playPanel.transform, false); //if the client played it, move it to the regular playPlanel
            }
        }
    }
    //Client CMD to update the combined deck on the server using the sync variable's string (each client passes its unique deck colour on start)
    [Command(requiresAuthority = false)] //added the authority bit in the process of debugging, defaults to true, so I think only the owner's version of their clientDeck will be changed unless this is here (but haven't really seen proof yet)
    public void CmdSendTurnInfo(NetworkIdentity networkTurnIdentity)
    {
        //find sharedvar game object in scene at runtime (for deck colour strings and game start booleans
        GameObject sharedVarManagerObj = GameObject.Find("SharedVarManager");

        // Check if the SharedVarManager GameObject is found AND 
        if (sharedVarManagerObj != null)
        {
            SharedVarManager sharedVarManager = sharedVarManagerObj.GetComponent<SharedVarManager>();
            //Call the command on the NetworkManager

            sharedVarManager.CmdUpdateWhosTurn(networkTurnIdentity); // this calls the SharedVarManager script, which should handle the game logic from here, and update who's turn it is

        }
        else
        {
            Debug.Log("SharedVarManager GameObject not found! In CmdSendTurnInfo"); //couldn't send turn info properly
        }

    }

    //Client CMD to attack other player on the server, and get it to update sync variables for health accordingly
    [Command(requiresAuthority = false)] //added the authority bit in the process of debugging, defaults to true, so I think only the owner's version of their clientDeck will be changed unless this is here (but haven't really seen proof yet)
    public void CmdSendAttackInfo(int damage, NetworkIdentity networkAttackIdentity)
    {
        //find sharedvar game object in scene at runtime (for deck colour strings and game start booleans
        GameObject sharedVarManagerObj = GameObject.Find("SharedVarManager");

        // Check if the SharedVarManager GameObject is found AND 
        if (sharedVarManagerObj != null)
        {
            SharedVarManager sharedVarManager = sharedVarManagerObj.GetComponent<SharedVarManager>();
            //Call the command on the 

            sharedVarManager.CmdAttackOtherPlayer(damage, networkAttackIdentity); //call server's attackPlayer CMD to update health variables accordingly
            Debug.Log("Attacked info recieved by dbdisplay...attack sent to server's CmdAttackOtherPlayer...");
        }
        else
        {
            Debug.Log("SharedVarManager GameObject not found! In CmdSendAttackInfo"); //couldn't send attack info properly
        }

    }
    //cmdsendselfdamage info
    [Command(requiresAuthority = false)] //added the authority bit in the process of debugging, defaults to true, so I think only the owner's version of their clientDeck will be changed unless this is here (but haven't really seen proof yet)
    public void CmdSendSelfDamage(int damage, NetworkIdentity networkAttackIdentity)
    {
        //find sharedvar game object in scene at runtime (for deck colour strings and game start booleans
        GameObject sharedVarManagerObj = GameObject.Find("SharedVarManager");
        SharedVarManager sharedVarManager = sharedVarManagerObj.GetComponent<SharedVarManager>();
        sharedVarManager.CmdSelfDamage(damage);
        Debug.Log("SENDING " + damage + " to self");
    }
    //specific to black heal
    [Command(requiresAuthority = false)] //added the authority bit in the process of debugging, defaults to true, so I think only the owner's version of their clientDeck will be changed unless this is here (but haven't really seen proof yet)
    public void CmdHealDamage(float heal, NetworkIdentity networkAttackIdentity)
    {
        //find sharedvar game object in scene at runtime (for deck colour strings and game start booleans
        GameObject sharedVarManagerObj = GameObject.Find("SharedVarManager");
        SharedVarManager sharedVarManager = sharedVarManagerObj.GetComponent<SharedVarManager>();
        sharedVarManager.CmdSelfHealAbility(heal);
        Debug.Log("SENDING healing " + heal + " to self");
    }
    //general heal
    [Command(requiresAuthority = false)] //added the authority bit in the process of debugging, defaults to true, so I think only the owner's version of their clientDeck will be changed unless this is here (but haven't really seen proof yet)
    public void CmdSendHealing(int healed, NetworkIdentity networkAttackIdentity)
    {
        //find sharedvar game object in scene at runtime (for deck colour strings and game start booleans
        GameObject sharedVarManagerObj = GameObject.Find("SharedVarManager");
        SharedVarManager sharedVarManager = sharedVarManagerObj.GetComponent<SharedVarManager>();
        sharedVarManager.CmdHealDamage(healed);
        Debug.Log("SENDING " + healed + " to self");
    }
    [Command(requiresAuthority = false)] //added the authority bit in the process of debugging, defaults to true, so I think only the owner's version of their clientDeck will be changed unless this is here (but haven't really seen proof yet)
    public void CmdPingDamage(int ping, NetworkIdentity networkAttackIdentity)
    {
        //find sharedvar game object in scene at runtime (for deck colour strings and game start booleans
        GameObject sharedVarManagerObj = GameObject.Find("SharedVarManager");
        SharedVarManager sharedVarManager = sharedVarManagerObj.GetComponent<SharedVarManager>();
        sharedVarManager.CmdPingDamage(ping);
    }



}
