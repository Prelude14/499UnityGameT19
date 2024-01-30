using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror; //need this script to be outside of script folder in order for it to use mirror for some reason
//using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour
{
    //bringing playerdeck script into this script
    //backs of cards in draw pile -- to be spawned and controlled by server once deck is created by server
    public GameObject deckPanel; //object containing all the below cardindeck objects 
    //public GameObject cardInDeck1;
    //public GameObject cardInDeck2;
    //public GameObject cardInDeck3;
    //public GameObject cardInDeck4;
    //public GameObject cardInDeck5;
    //public GameObject cardInDeck6;
    public Text deckCountText;

    //playermanager will spawn these on the clients when they click join as host or client
    //public GameObject[] clones;
    public GameObject hand;
    public GameObject oppHand;
    public GameObject playPanel;
    public GameObject oppPlayPanel;
    public GameObject cardInHand;//need for dealing cards

    //need lists to store and manipulate the different decks
    public List<Card1> container = new List<Card1>(); //temp list for shuffling deck
    public static int deckSize = 40; //two decks combined should always equal 40 cards
    public List<Card1> combinedDeck = new List<Card1>(); //list of card1 game objects for combined deck

    [SyncVar(hook = "OnMySyncIntChanged")] public string clientDecks = "";//need to sync a string variable between client and servers that will tell server which game decks to use
    [SyncVar] //need to sync a string variable between client and servers that will tell server which game decks to use
    public bool validDeckShuffled = false; //need to track if game deck was made properly
    [SyncVar]
    public bool gameStarted = false;



    public override void OnStartClient() //if our playermanager is acting as a client
    {
        base.OnStartClient();

        //find client side game objects at run time (they are spawned in by player manager)
        hand = GameObject.Find("hand");
        oppHand = GameObject.Find("oppHand");
        playPanel = GameObject.Find("playPanel");
        oppPlayPanel = GameObject.Find("oppPlayPanel");

        //CmdGetPlayerColours(playerDeck.playerColour); //each client will send their chosen deck colour to server when they connect
        OnMySyncIntChanged(clientDecks, clientDecks);//attempting to get new clients to load correct version of synced string when they join (otherwise thee second client overwrites the string when they join, and game never starts)
    }

    void OnMySyncIntChanged(string oldValue, string newValue)
    {
        // Do something
        clientDecks = newValue;
    }



    //[Server]
    public override void OnStartServer() //if our playermanager is acting as a server
    {
        base.OnStartServer();


        //start waiting for at least 2 clients to update the sync variable string -- it tells the server what colour decks to mix
        //while (checkFor2PlayersAndTheirCombo().Equals("Error 1: Not enough players/strings.") || checkFor2PlayersAndTheirCombo().Equals("Error 2: Too many players/strings.") ) {}


        //once 2 clients have connected and they have a valid combo, the server can populate the combinedDeck using the proper combo
        //WANT SERVER TO CREATE DECK, AND THEN DEAL CARDS TO CLIENT USING RPC
        //CmdCreateDeck();//attempt to create deck

    }

    //Client CMD to update the combined deck on the server using the sync variable's string (each client passes its unique deck colour on start)
    [Command]
    public void CmdGetPlayerColours(string clientColour)
    {
        Debug.Log("Server's clientDecks string at start of cmd call:" + clientDecks);
        //if there is not enough players yet, then update sync var using client's colour
        if (checkFor2PlayersAndTheirCombo().Equals("Error 1: Not enough players/strings.") && validDeckShuffled == false)         
        {
            clientDecks += clientColour;//if empty or only one colour, add one, then check again to start game incase that was the second client colour needed to start
            Debug.Log("Server's clientDecks string after adding a colour:" + clientDecks);
            //if client deck is now full after adding the last CLIENT, then create decks so the game can begin (added redundant check for 2nd error message as well, just to be safe
            string secondCheck = checkFor2PlayersAndTheirCombo();
            Debug.Log("Server's secondCheck string:" + secondCheck);
            if (!secondCheck.Equals("Error 1: Not enough players/strings.") && !secondCheck.Equals("Error 2: Too many players/strings.")) 
            {
                serverCreateDeck(); //build deck **2ND CLIENT ALWAYS STARTS GAME
                //after serverCreateDeck is run, validDeckShuffled becomes TRUE inside the shuffle() function, so the draw cmd can deal cards out
            }
            else if (secondCheck.Equals("Error 1: Not enough players/strings.")) //if still not enough after adding itself, then dont do anything, the OTHER CLIENT needs to add its colour
            {
                //do nothing
                Debug.Log("The game was not started. Still need one more player. Client Decks equals:"+clientDecks);
            }

        }   //each client should have its own private playerDeck colour, and the server will mix them
        else if (checkFor2PlayersAndTheirCombo().Equals("Error 2: Too many players/strings.") && validDeckShuffled == false)          //if there is too many strings, tell playerDeck script the error message
        {
            clientDecks = "Error 2: Too many players/strings."; //reset clientDecks to error message for all clients to see
        }
        else //if the check isn't an error, than the game should be good to start, 
        {
            //serverCreateDeck();//build deck so that 
            //after serverCreateDeck is run, validDeckShuffled becomes TRUE inside the shuffle() function, so the draw cmd can deal cards out
        }
        /*
        while (!validDeckShuffled)
        {
            //I want the clients to wait here until the server has created and shuffled the deck properly before dealing out the cards
        }
        //CmdDraw(2);//after telling the server what colour deck to mix, it asks server to deal out 2 cards
        */
        RpcShowCombo(clientDecks); //send result of this back to client so they can check if there is two players in game before asking server to deal cards
    }
    [ClientRpc]
    void RpcShowCombo(string clientscombo)
    {
        clientDecks = clientscombo; //send combo string from server to client scripts
        Debug.Log("Server Called RPC to clients. Client Decks equals:" + clientDecks);

    }

    //we will have server check for this until there is enough players  -- ONLY SERVER CAN DO THIS
    [Server]
    public string checkFor2PlayersAndTheirCombo()
    {
        //check if ClientDecks is a combo of two valid colours

        //if clientDecks IS EMPTY, OR has ONLY ONE playercolour in it, return error message
        if (clientDecks.Equals("") || clientDecks.Equals("BLACK") || clientDecks.Equals("R") || clientDecks.Equals("W") || clientDecks.Equals("BLUE"))
        {
            Debug.Log("There is only one string inside the clientDecks string, or its still empty, meaning not enough clients have updated the clientDecks string.");
            return "Error 1: Not enough players/strings."; //only one client has updated clientDecks
        }
        //Now we check for each combo of colours (VALID ONLY), and return which combo they are
        //============================================================================= CHECK BLACK COMBOS ====================================
        else if (clientDecks.Equals("BLACKBLACK"))
        {         //BLACK & BLACK    
            return "BLACKBLACK";
        }
        else if (clientDecks.Equals("BLACKR") || clientDecks.Equals("RBLACK")) //clientDecks.Equals("RR") || clientDecks.Equals("RW") || clientDecks.Equals("RBLUE") ) 
        {         //BLACK & RED OR RED & BLACK 
            return "BLACKRED";
        }
        else if (clientDecks.Equals("BLACKW") || clientDecks.Equals("WBLACK"))
        {         //BLACK & WHITE OR WHITE & BLACK 
            return "BLACKWHITE";
        }
        else if (clientDecks.Equals("BLACKBLUE") || clientDecks.Equals("BLUEBLACK"))
        {         //BLACK & BLUE OR BLUE & BLACK
            return "BLACKBLUE";
        }
        //============================================================================= CHECK RED COMBOS ====================================
        else if (clientDecks.Equals("RR"))
        {         //RED & RED   
            return "REDRED";
        }
        else if (clientDecks.Equals("RW") || clientDecks.Equals("WR"))
        {         //RED & WHITE OR WHITE & RED   
            return "REDWHITE";
        }
        else if (clientDecks.Equals("RBLUE") || clientDecks.Equals("BLUER"))
        {         //RED & BLUE OR BLUE & RED   
            return "REDBLUE";
        }
        //============================================================================= CHECK WHITE COMBOS ====================================
        else if (clientDecks.Equals("WW"))
        {         //WHITE & WHITE  
            return "WHITEWHITE";
        }
        else if (clientDecks.Equals("WBLUE") || clientDecks.Equals("BLUEW"))
        {         //WHITE & BLUE OR BLUE & WHITE   
            return "WHITEBLUE";
        }
        //============================================================================= CHECK BLUE COMBOS  ====================================
        else if (clientDecks.Equals("BLUEBLUE"))
        {         //BLUE & BLUE
            return "BLUEBLUE";
        }
        //else there is too many colours in the string, or the strings are misspelt -- its and invalid combo
        else
        {
            Debug.Log("There are too many strings inside the clientDecks string, meaning there might be too many clients.");
            return "Error 2: Too many players/strings.";
        }

    }
    [Server]
    public void serverCreateDeck() //want to have method for checking if enough players have joined
    {
        //once 2 clients have connected and they have a valid combo, the server can populate the combinedDeck using the proper combo
        //WANT SERVER TO CREATE DECK, AND THEN DEAL CARDS TO CLIENT USING RPC
        string combo = checkFor2PlayersAndTheirCombo(); //now that while loop stopped, store method string as one variable so we don't need to call method every time
        if (combo.Equals("Error 1: Not enough players/strings."))       //IF first error code
        {
            Debug.Log("There is not enough players.");
        }
        //============================ BLACK COMBOS =====================================================================           ***BLACK == 1
        else if (combo.Equals("BLACKBLACK"))       //IF both players chose BLACK deck, combinedDeck becomes 2 black decks
        {
            //deal and shuffle 2 black decks together to be dealt to clients Done inside the functions below
            populateCombinedDeck(11);

            shuffleCombinedDeck();
        }
        else if (combo.Equals("BLACKRED"))       //IF players chose 1 BLACK and 1 Red deck, combinedDeck becomes blackred deck
        {
            //deal and shuffle 1 black and 1 red deck together to be dealt to clients Done inside the functions below
            populateCombinedDeck(12);

            shuffleCombinedDeck();
        }
        else if (combo.Equals("BLACKWHITE"))       //IF players chose 1 BLACK and 1 White deck, combinedDeck becomes blackwhite deck
        {
            //deal and shuffle 1 black and 1 white deck together to be dealt to clients Done inside the functions below
            populateCombinedDeck(13);

            shuffleCombinedDeck();
        }
        else if (combo.Equals("BLACKBLUE"))       //IF players chose 1 BLACK and 1 Blue deck, combinedDeck becomes blackblue deck
        {
            //deal and shuffle 1 black and 1 blue deck together to be dealt to clients Done inside the functions below
            populateCombinedDeck(14);

            shuffleCombinedDeck();
        } //============================ RED COMBOS =====================================================================            ***RED == 2
        if (combo.Equals("REDRED"))       //IF both players chose RED deck, combinedDeck becomes 2 red decks
        {
            //deal and shuffle 2 red decks together to be dealt to clients Done inside the functions below
            populateCombinedDeck(22);

            shuffleCombinedDeck();
        }
        else if (combo.Equals("REDWHITE"))       //IF players chose 1 Red and 1 White deck, combinedDeck becomes redwhite deck
        {
            //deal and shuffle 1 red and 1 white deck together to be dealt to clients Done inside the functions below
            populateCombinedDeck(23);

            shuffleCombinedDeck();
        }
        else if (combo.Equals("REDBLUE"))       //IF players chose 1 BLACK and 1 White deck, combinedDeck becomes redblue deck
        {
            //deal and shuffle 1 red and 1 blue deck together to be dealt to clients Done inside the functions below
            populateCombinedDeck(24);

            shuffleCombinedDeck();
        }//============================ WHITE COMBOS =====================================================================          ***WHITE == 3
        else if (combo.Equals("WHITEWHITE"))       //IF both players chose White deck, combinedDeck becomes 2 white decks
        {
            //deal and shuffle 2 white decks together to be dealt to clients Done inside the functions below
            populateCombinedDeck(33);

            shuffleCombinedDeck();
        }
        else if (combo.Equals("WHITEBLUE"))       //IF players chose 1 White deck anbd 1 Blue deck, combinedDeck becomes whiteblue deck
        {
            //deal and shuffle 1 white and 1 blue deck together to be dealt to clients Done inside the functions below
            populateCombinedDeck(34);

            shuffleCombinedDeck();
        }//============================ BLUE COMBOS =====================================================================           ***BLUE == 4
        else if (combo.Equals("BLUEBLUE"))       //IF both players chose Blue deck, combinedDeck becomes 2 blue decks
        {
            //deal and shuffle 2 blue decks together to be dealt to clients Done inside the functions below
            populateCombinedDeck(44);

            shuffleCombinedDeck();
        }
        else
        {
            Debug.Log("The Combo was invalid.");
        }
    }

    //add 40 cards from the cardDatabase using 2 neutral decks plus two colours (which gets selected by player) -- ONLY SERVER CAN DO THIS
    [Server]
    void populateCombinedDeck(int selectedcombo)
    {
        //every deck gets 16 neutral cards added to it first (there is only 8 cards per deck regularly, but this is the combination of 2)
        for (int i = 0; i < 2; i++)
        {
            for (int k = 0; k < 8; k++)
            {
                int offset = 1 * i;
                combinedDeck[k + offset] = cardDatabase.neutralCardList[k];
            } //ADD 8 Neutral cards first iteration, then 2nd 8 cards in the 2nd iteration

        }//16 neutral cards should now be in 1st 16 slots of deck list
        //====================================================== ADD COLOURS =======================================================
        int j = 16;//start adding the other 24 cards from each deck at index 16 because of neutral cards

        //===================================================== BLACK COMBOS =======================================================

        if (selectedcombo == 11)      //== 2 BLACK DECKS *********************************************************
        {
            for (int i = 0; i < 2; i++) //need to loop through 12 cards of black deck twice
            {
                for (int k = 0; k < 12; k++)                //ADD first 12 BLACK cards starting at index 16
                {
                    int offset = 1 * i;
                    combinedDeck[j + offset] = cardDatabase.blackCardList[i];
                    j++;
                }
                //j will hit index 16+12 = 28 after first iteration , then 28+12 =40 for second
            }
            j = 16; //reset j to 16 after loop is done, to make sure other decks don't get indexed incorrectly in future
        }
        else if (selectedcombo == 12)      //== 1 BLACK 1 RED DECK *********************************************************
        {
            for (int i = 0; i < 12; i++)                //ADD 12 BLACK cards after first 16 neutral
            {
                combinedDeck[j] = cardDatabase.blackCardList[i];
                j++; //j should go from 16-28
            }
            for (int i = 0; i < 12; i++)                //ADD 12 RED cards after 12 black
            {
                combinedDeck[j] = cardDatabase.redCardList[i];
                j++; //j should go from 28-40
            }
            j = 16; //reset j to 16 after loop is done, to make sure other decks don't get indexed incorrectly in future
        }
        else if (selectedcombo == 13)      //== 1 BLACK 1 WHITE DECK *********************************************************
        {
            for (int i = 0; i < 12; i++)                //ADD 12 BLACK cards after first 16 neutral
            {
                combinedDeck[j] = cardDatabase.blackCardList[i];
                j++; //j should go from 16-28
            }
            for (int i = 0; i < 12; i++)                //ADD 12 WHITE cards after 12 black
            {
                combinedDeck[j] = cardDatabase.whiteCardList[i];
                j++; //j should go from 28-40
            }
            j = 16; //reset j to 16 after loop is done, to make sure other decks don't get indexed incorrectly in future
        }
        else if (selectedcombo == 14)      //== 1 BLACK 1 BLUE DECK *********************************************************
        {
            for (int i = 0; i < 12; i++)                //ADD 12 BLACK cards after first 16 neutral
            {
                combinedDeck[j] = cardDatabase.blackCardList[i];
                j++; //j should go from 16-28
            }
            for (int i = 0; i < 12; i++)                //ADD 12 BLUE cards after 12 black
            {
                combinedDeck[j] = cardDatabase.blueCardList[i];
                j++; //j should go from 28-40
            }
            j = 16; //reset j to 16 after loop is done, to make sure other decks don't get indexed incorrectly in future
        }
        //===================================================== RED COMBOS =======================================================
        else if (selectedcombo == 22)      //== 2 RED DECKS *********************************************************
        {
            for (int i = 0; i < 2; i++) //need to loop through 12 cards of red deck twice
            {
                for (int k = 0; k < 12; k++)                //ADD first 12 RED cards starting at index 16
                {
                    int offset = 1 * i;
                    combinedDeck[j + offset] = cardDatabase.redCardList[i];
                    j++;
                }
                //j will hit index 16+12 = 28 after first iteration , then 28+12 =40 for second
            }
            j = 16; //reset j to 16 after loop is done, to make sure other decks don't get indexed incorrectly in future
        }
        else if (selectedcombo == 23)      //== 1 RED 1 WHITE DECK *********************************************************
        {
            for (int i = 0; i < 12; i++)                //ADD 12 RED cards after first 16 neutral
            {
                combinedDeck[j] = cardDatabase.redCardList[i];
                j++; //j should go from 16-28
            }
            for (int i = 0; i < 12; i++)                //ADD 12 WHITE cards after 12 red
            {
                combinedDeck[j] = cardDatabase.whiteCardList[i];
                j++; //j should go from 28-40
            }
            j = 16; //reset j to 16 after loop is done, to make sure other decks don't get indexed incorrectly in future
        }
        else if (selectedcombo == 24)      //== 1 RED 1 BLUE DECK *********************************************************
        {
            for (int i = 0; i < 12; i++)                //ADD 12 RED cards after first 16 neutral
            {
                combinedDeck[j] = cardDatabase.redCardList[i];
                j++; //j should go from 16-28
            }
            for (int i = 0; i < 12; i++)                //ADD 12 BLUE cards after 12 red
            {
                combinedDeck[j] = cardDatabase.blueCardList[i];
                j++; //j should go from 28-40
            }
            j = 16; //reset j to 16 after loop is done, to make sure other decks don't get indexed incorrectly in future
        }
        //===================================================== WHITE COMBOS =======================================================
        else if (selectedcombo == 33)      //== 2 WHITE DECKS *********************************************************
        {
            for (int i = 0; i < 2; i++) //need to loop through 12 cards of white deck twice
            {
                for (int k = 0; k < 12; k++)                //ADD first 12 WHITE cards starting at index 16
                {
                    int offset = 1 * i;
                    combinedDeck[j + offset] = cardDatabase.whiteCardList[i];
                    j++;
                }
                //j will hit index 16+12 = 28 after first iteration , then 28+12 =40 for second
            }
            j = 16; //reset j to 16 after loop is done, to make sure other decks don't get indexed incorrectly in future
        }
        else if (selectedcombo == 34)      //== 1 WHITE 1 BLUE DECK *********************************************************
        {
            for (int i = 0; i < 12; i++)                //ADD 12 WHITE cards after first 16 neutral
            {
                combinedDeck[j] = cardDatabase.whiteCardList[i];
                j++; //j should go from 16-28
            }
            for (int i = 0; i < 12; i++)                //ADD 12 BLUE cards after 12 white
            {
                combinedDeck[j] = cardDatabase.blueCardList[i];
                j++; //j should go from 28-40
            }
            j = 16; //reset j to 16 after loop is done, to make sure other decks don't get indexed incorrectly in future
        }
        //===================================================== BLUE COMBOS =======================================================
        else if (selectedcombo == 44)      //== 2 BLUE DECKS *********************************************************
        {
            for (int i = 0; i < 2; i++) //need to loop through 12 cards of blue deck twice
            {
                for (int k = 0; k < 12; k++)                //ADD first 12 BLUE cards starting at index 16
                {
                    int offset = 1 * i; //second round will add indexes 28-40
                    combinedDeck[j + offset] = cardDatabase.whiteCardList[i];
                    j++;
                }
                //j will hit index 16+12 = 28 after first iteration , then 28+12 =40 for second
            }
            j = 16; //reset j to 16 after loop is done, to make sure other decks don't get indexed incorrectly in future
        }

    }//end populateCombinedDeck

    [Server]
    public void shuffleCombinedDeck() //takes whatever deck is, and shuffles it using random indexing
    {
        for (int i = 0; i < deckSize; i++)
        {
            container[0] = combinedDeck[i];
            int randomIndex = Random.Range(i, deckSize);
            combinedDeck[i] = combinedDeck[randomIndex];
            combinedDeck[randomIndex] = container[0];
        }
        //now that deck is shuffled, change bool variable to reflect this
        validDeckShuffled = true;
    }
    [Server]
    public int getCombinedDeckSize()
    {
        return deckSize;
    }
    [Server]
    public string getCombinedDeckTop()
    {
        return combinedDeck[0].ToString();
    }

    [Command]                                       //clients request the server to deal them their cards
    public void CmdDraw(int drawSize)
    {
        //want clients to wait here until server has created and shuffled a VALID deck
        while (!validDeckShuffled)
        {
            //if server hasn't shuffled a valid deck yet, stay inside while loop, doing nothing
        }
        //Double check enough players have joined and the server has recieved a valid combo of colours from the clients to deal decks, go ahead and deal the client their two cards
        if (!checkFor2PlayersAndTheirCombo().Equals("Error 1: Not enough players/strings.") && !checkFor2PlayersAndTheirCombo().Equals("Error 2: Too many players/strings."))
        {
            if (deckSize > 0) //check if there's other cards. do not draw if no more cards do smth else;
            {
                //draw cards repeat until drawSize
                for (int x = 0; x < drawSize; x++)
                {
                    //slow down code so we don't draw too fast
                    //yield return new WaitForSeconds(.15F);
                    //Instantiate(cardInHand, transform.position, transform.rotation);
                    GameObject card = Instantiate(cardInHand, new Vector2(0, 0), Quaternion.identity); //Server instantiates card object on the client side of client who requested CmdDraw
                    NetworkServer.Spawn(card, connectionToClient); //Server spawns object across network for other clients to use, and gives Client the authority of the object (their card in this case)
                    RpcShowCard(card, "Dealt"); //this gets server to display the card object across both clients (and it displays specificly based on who has authority inside the rpc method)
                    //renderCardColour(card);
                }
                gameStarted = true; //server should sync this value to all clients, and once 
            }
            else
            {
                //lose game
                //TODO: Add a lose game

            }
        }
    }
    //this method is requested by the Server to run on all Clients, and it decides how to display the cards being dealt or played in the game
    [ClientRpc]
    void RpcShowCard(GameObject card, string type)
    {
        //check type and then the authority of the card to see if the card has been "Dealt" to the client, and display the dealt card according to if the client owns it or not
        if (type.Equals("Dealt"))
        {
            if (isOwned)
            {
                card.transform.SetParent(hand.transform, false); //if its the client's card, put it in the player's hand
            }
            else
            {
                card.transform.SetParent(oppHand.transform, false); //if its not, put it in the enemy's hand object
                //card.GetComponent<CardFlipper>().Flip(); //show the back of the card 
            }
        }
        //if the card has been "Played", check which client played it, and display it accordingly on each client's side
        else if (type.Equals("Played"))
        {
            if (!isOwned)
            {
                card.transform.SetParent(oppPlayPanel.transform, false); //enemy play panel
                //card.GetComponent<CardFlipper>().Flip(); //show correct side of card when played
            }
            else
            {
                card.transform.SetParent(playPanel.transform, false); //if the client played it, move it to the regular playPlanel
            }
        }
    }
    /*
    [Command]
    public void CmdUpdate()
    {
        RpcChangeSize();
    }
    =
    [ClientRpc]
    void RpcChangeSize()
    {
        //these ifs change what the draw pile of cards looks like based on how many cards are left in the pile.
        if (deckSize < 20)
        {
            cardInDeck6.SetActive(false);
        }
        if (deckSize < 15)
        {
            cardInDeck5.SetActive(false);
        }
        if (deckSize < 10)
        {
            cardInDeck4.SetActive(false);
        }
        if (deckSize < 5)
        {
            cardInDeck3.SetActive(false);
        }
        if (deckSize < 3)
        {
            cardInDeck2.SetActive(false);
        }
        if (deckSize < 1)
        {
            cardInDeck1.SetActive(false);
        }

        //display deck card count;
        //deckCountText.text = "" + deckSize;

        //if new turn for client, draw them a card
        //if (turnScript.turnStart == true)
        //{
        //    CmdDraw(1);
        //    turnScript.turnStart = false;
        //}
    } */

}
