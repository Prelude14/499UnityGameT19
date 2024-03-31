using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SharedVarManager : NetworkBehaviour
{
    //=====================================================================  VARIABLES  ===============================================================================
    //we will share a list of strings to use in order to create the game
    private List<string> playerColors = new List<string>();

    //need access to player manager script that is unique to each client
    public PlayerManager PlayerManager;
    public PlayerManager PlayerTurnManager; //new one for turn set up just to double check it is new playermanager connection from the Cmdgetplayercolour command
    public PlayerManager PlayerManaManager; //new one for attack set up just to double check it is new playermanager connection from the Cmdgetplayercolour or CmdUpdateWhosTurn
    public PlayerManager PlayerAttackManager; //new one for attack set up just to double check it is new playermanager connection from the Cmdgetplayercolour, CmdUpdateWhosTurn, or CmdUpdateManaCount
   

    //I ended up using some sync vars for these 2 bools, to add additional checks to the game starting logic
    [SyncVar]
    public bool validDeckShuffled = false; //need to track if game deck was made properly
    [SyncVar]
    public bool gameStarted = false; //last bool tracking if game started properly

    //need lists to store and manipulate the different decks in the createDeck and shuffle method
    public List<Card1> container = new List<Card1>(); //temp list for shuffling deck
    public static int deckSize = 40; //two decks combined should always equal 40 cards
    public List<Card1> combinedDeck = new List<Card1>(); //list of card1 game objects for combined deck

    public List<Card1> temp_combinedDeck = new List<Card1>(); //list of card1 game objects for server's temporary combined deck

    //public CardDatabase cardDatabase; // Add this in order to initialize the card deck lists script for use later

    //get deal button menu as a game object in order to get rid of the screen when the game begins properly
    public GameObject dealButtonMenu;


    [SyncVar]//need to store who's turn it is
    public int whosTurn = 0; //start at 0, but it will switch from 1 and 2 to communicate if its player 1 or 2's turn (first player to send colour is player 1)
    [SyncVar]//need to store who's turn it is
    public int serverTurnCount = 0; //start at 0, but it will be incremented each time a turn ends

    [SyncVar] public int p1Mana = 1; //need sync var to track each player's mana
    [SyncVar] public int p2Mana = 1; //need sync var to track each player's mana

    [SyncVar] public float p1Health = 30; //need sync var to track each player's health (start with full 30 points)
    [SyncVar] public float p2Health = 30; //need sync var to track each player's health
    //=====================================================================  METHODS  ===============================================================================

    public static float p1HP;
    public static float p2HP;
    public static int staticTurn;

    public static float p1TotalHeal;
    public static float p2TotalHeal;

    public static int p1TotalDraw;
    public static int p2TotalDraw;
    //command when turn is ended on client side (they press end turn button and call this command in turnscript)
    [Command(requiresAuthority = false)]
    public void CmdUpdateWhosTurn(NetworkIdentity networkTurnIdentity)
    {
        PlayerTurnManager = networkTurnIdentity.GetComponent<PlayerManager>(); //want to track who is ending each turn

        if (whosTurn == 1) //if its player one's turn
        {
            if (PlayerTurnManager.isPlayerOne == true && PlayerTurnManager.isPlayerTwo == false) //if player one ended their turn
            {
                serverTurnCount++;//increment turn count each time this is called
                whosTurn = 2; //if it was player 1's turn, then now it is player 2
                p1Mana++; //add one mana to player one since they will need it come the next turn, player 2 still needs to take their turn before gaining more mana

                //PlayerTurnManager.isPlayerManagersTurn = false;//It is no longer my turn
            }
            else if (PlayerTurnManager.isPlayerTwo == true && PlayerTurnManager.isPlayerOne == false) //if player 2 ended player 1's turn somehow
            {
                //Do NOTHING SINCE player2 should never be able to end turn when whosTurn = 1
                //Debug.log("Player 2 tried to end player 1's turn");
            }
        }
        else if (whosTurn == 2)
        {
            if (PlayerTurnManager.isPlayerOne == true && PlayerTurnManager.isPlayerTwo == false) //if player one ended player 2's turn somehow
            {
                //Do NOTHING SINCE player1 should never be able to end turn when whosTurn = 2
                //Debug.log("Player 1 tried to end player 2's turn");
            }
            else if (PlayerTurnManager.isPlayerTwo == true && PlayerTurnManager.isPlayerOne == false) //if player 2 ended their turn
            {
                serverTurnCount++; //increment turn count each time this is called
                whosTurn = 1; //if it was player 2's turn, then now it is player 1's
                p2Mana++; //add one mana to player two since they will need it come the next turn, player 1 still needs to take their turn before gaining more mana

                //PlayerTurnManager.isPlayerManagersTurn = false;//It is no longer my turn
            }
        }
    }
    //command when client plays a card and needs to update their mana count
    [Command(requiresAuthority = false)]
    public void CmdUpdateManaCount(int manaCost, NetworkIdentity networkManaIdentity)
    {
        PlayerManaManager = networkManaIdentity.GetComponent<PlayerManager>(); //want to track who is playing the card

        if (whosTurn == 1) //if its player one's turn
        {
            if (PlayerManaManager.isPlayerOne == true && PlayerManaManager.isPlayerTwo == false) //if player one played the card
            {
                p1Mana -= manaCost; //subtract mana cost from player one's overall mana (since they have to pay to place a card down)

                //PlayerTurnManager.isPlayerManagersTurn = false;//It is no longer my turn
            }
            else if (PlayerManaManager.isPlayerTwo == true && PlayerManaManager.isPlayerOne == false) //if player 2 played a card on player 1's turn somehow
            {
                //Do NOTHING SINCE player2 should never be able to end turn when whosTurn = 1
                //Debug.log("Player 2 tried to play during player 1's turn");
            }
        }
        else if (whosTurn == 2)
        {
            if (PlayerManaManager.isPlayerOne == true && PlayerManaManager.isPlayerTwo == false) //if player one played a card on player 2's turn somehow
            {
                //Do NOTHING SINCE player1 should never be able to end turn when whosTurn = 2
                //Debug.log("Player 1 tried to play during player 2's turn");
            }
            else if (PlayerManaManager.isPlayerTwo == true && PlayerManaManager.isPlayerOne == false) //if player 2 played the card
            {
                p2Mana -= manaCost; //subtract mana cost from player 2's overall mana (since they have to pay to place a card down)

                //PlayerTurnManager.isPlayerManagersTurn = false;//It is no longer my turn
            }
        }
    }

    //command when a client attacks the other player (they drag a played card into the opponent's avatar in order to attack)
    [Command(requiresAuthority = false)]
    public void CmdAttackOtherPlayer(int damage, NetworkIdentity networkAttackIdentity)
    {
        PlayerAttackManager = networkAttackIdentity.GetComponent<PlayerManager>(); //want to track who is attacking

        if (whosTurn == 1) //if its player one's turn
        {
            if (PlayerAttackManager.isPlayerOne == true && PlayerAttackManager.isPlayerTwo == false) //if player one attacked
            {
                p2Health -= damage; //update p2 Health to equal old health minus the amount of damage that was sent by p1

            }
            else if (PlayerAttackManager.isPlayerTwo == true && PlayerAttackManager.isPlayerOne == false) //if player 2 attacked player 1 somehow
            {
                //Do NOTHING SINCE player2 should never be able to attack when whosTurn = 1
                //Debug.log("Player 2 tried to attack p1 during player 1's turn");
            }
        }
        else if (whosTurn == 2)
        {
            if (PlayerAttackManager.isPlayerOne == true && PlayerAttackManager.isPlayerTwo == false) //if player one attacked player 2 somehow
            {
                //Do NOTHING SINCE player1 should never be able to attack when whosTurn = 2
                //Debug.log("Player 1 tried to attack p2 during player 2's turn");
            }
            else if (PlayerAttackManager.isPlayerTwo == true && PlayerAttackManager.isPlayerOne == false) //if player 2 attacked
            {
                p1Health -= damage; //update p1 Health to equal old health minus the amount of damage that was sent by p2
            }
        }
    }

    //command to self damage your own health
    [Command(requiresAuthority = false)]
    public void CmdSelfDamage(int damage){
        if(whosTurn == 1){ //if player one played this
            p1Health -= damage;
        }else {
            p2Health -= damage;
        }
    }

    //ping bits of damage
    [Command(requiresAuthority = false)]
    public void CmdPingDamage(int damage){
        if(whosTurn == 1){ //if player one played this
            p2Health -= damage;
        }else {
            p1Health -= damage;
        }
    }
    [Command(requiresAuthority = false)]
    public void CmdHealDamage(int healed){
        if(whosTurn == 1){ //if player one played this
            p1Health += healed;
            p1TotalHeal += healed;
        }else {
            p2Health += healed;
            p2TotalHeal += healed;
        }
    }

     [Command(requiresAuthority = false)]
    public void CmdSelfHealAbility(float heal){
        float toHeal;
        if(whosTurn == 1){ //if player one played this
            toHeal = turnScript.p1StartingHP - p1Health; //difference in heal
            if(toHeal >= 0){
                p1Health += toHeal;
                p1TotalHeal += toHeal;
            }else{
                p1Health += 0;
            }
        }else {
            toHeal = turnScript.p2StartingHP - p2Health; //difference in heal
            if(toHeal >= 0){
                p2Health += toHeal;
                p2TotalHeal += toHeal;
            }else{
                p2Health += 0;
            }
        }
    }

    //this command receives a client's colour, and then checks to see if game can be started
    [Command(requiresAuthority = false)]
    public void CmdGetPlayerColor(string clientColour, NetworkIdentity networkClientIdentity) //clients will call this server function to add their colour to this list of strings (list should get around the 2 instances of playerManager 
    {
        // Add the client's color to the list
        playerColors.Add(clientColour);

        if (playerColors.Count == 1)
        {
            PlayerManager = networkClientIdentity.GetComponent<PlayerManager>();
            PlayerManager.isPlayerOne = true; //SET sync variables from player manager to true or false in order for it to track who is player ONE
            PlayerManager.isPlayerTwo = false;
            //PlayerManager.isPlayerManagersTurn = true; //1st player gets to go first
        }
        // Check if there is a valid combination, ONLY TWO Strings will allow game to start, 
        else if (playerColors.Count == 2)
        {
            string combo = playerColors[0] + playerColors[1]; //get strings as one string for checks


            //send combo to checkFor2PlayersAndTheirCombo method to ensure valid colour combination
            string secondCheck = checkFor2PlayersAndTheirCombo(combo);
            //if combo string after the check method is VALID, then create decks so the game can begin (added redundant checks for too little or too many strings since the old logic has checks for it anyways

            if (!secondCheck.Equals("Error 1: Not enough players/strings.") && !secondCheck.Equals("Error 2: Too many players/strings."))
            {
                PlayerManager = networkClientIdentity.GetComponent<PlayerManager>();
                PlayerManager.isPlayerOne = false; //SET sync variables from player manager to true or false in order for it to track who is player TWO
                PlayerManager.isPlayerTwo = true;  //first player to run this command is player TWO

                serverCreateDeck(secondCheck); //build deck
                RpcStartGame(combo, validDeckShuffled, temp_combinedDeck); //send all info inside rpc to all clients

                whosTurn = 1;//start with player one's turn when game is officially started

                //after serverCreateDeck is run, validDeckShuffled becomes TRUE inside the shuffle() function, so the draw cmd can deal cards out

            }
            else if (secondCheck.Equals("Error 1: Not enough players/strings.")) //if this error is triggering, something is very wrong with the list of strings, or the combo string made from the first 2 entries of the list
            {
                //do nothing
                Debug.Log("The game was not started. Something has gone wrong in the combo check, since it thinks there is only one string in the list.");
            }
            else if (secondCheck.Equals("Error 2: Too many players/strings.")) //if this error is triggering, something is very wrong with the list of strings, or the combo string made from the first 2 entries of the list
            {
                //do nothing
                Debug.Log("The game was not started. Something has gone wrong in the combo check, since it thinks there is too many strings in the list.");
            }
            else //if the check isn't an error, than the game should be good to start, 
            {
                //serverCreateDeck();//build deck so that 
            }

        }
    }
    //this will tell the clients that the server has recieved 2 strings, so the game should be ready to start
    [ClientRpc]
    void RpcStartGame(string combo, bool deckShuff, List<Card1> gameDeck)
    {
        // Inform the clients of the valid combination and execute necessary logic
        Debug.Log($"Valid combination: {combo}");

        //locate the PlayerManager in each Client and send set the combined deck colours to be the proper string, AND CHANGE BOOLS so clients no that the game has started
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();

        PlayerManager.clientDecks = combo; //SET sync variable from network manager

        Debug.Log("deckShuffled: " + deckShuff);

        PlayerManager.deckShuffled = deckShuff; //set playermanager's bool to equal whatever this script's bool is

        //add game deck to other client
        combinedDeck.AddRange(gameDeck);//add parameter list to other client's sharedvarmanager instance (will overwrite other to be the same)

        //add the full game deck to the playerDeck script as well
        playerDeck.staticDeck.AddRange(gameDeck); //add ALL OF GAME DECK to playerDeck's static deck list, so that db display can access the right cards.

        PlayerManager.CmdDraw(1, combo); //get server to deal out two cards for each client

        Debug.Log("Game started !");

        dealButtonMenu = GameObject.Find("startScreen background"); //get startgame and deal cards menu and text object so that we can deactivate it when the cards are dealt and the game begins.
        dealButtonMenu.SetActive(false);

        gameStarted = true; //store game started value
    }

    //we will have server check for this until there is enough players  -- ONLY SERVER CAN DO THIS (supposedly)
    [Server]
    public string checkFor2PlayersAndTheirCombo(string checkString)
    {
        //check if checkString is a combo of two valid colours

        //if checkString IS EMPTY, OR has ONLY ONE playercolour in it, return error message
        if (checkString.Equals("") || checkString.Equals("BLACK") || checkString.Equals("R") || checkString.Equals("W") || checkString.Equals("BLUE"))
        {
            Debug.Log("There is only one string inside the checkString string, or its still empty, meaning not enough clients have updated the checkString string.");
            return "Error 1: Not enough players/strings."; //Something is wrong with what strings were stored in the list
        }
        //Now we check for each combo of colours (VALID ONLY), and return which combo they are
        //============================================================================= CHECK BLACK COMBOS ====================================
        else if (checkString.Equals("BLACKBLACK"))
        {         //BLACK & BLACK    
            return "BLACKBLACK";
        }
        else if (checkString.Equals("BLACKR") || checkString.Equals("RBLACK")) //checkString.Equals("RR") || checkString.Equals("RW") || checkString.Equals("RBLUE") ) 
        {         //BLACK & RED OR RED & BLACK 
            return "BLACKRED";
        }
        else if (checkString.Equals("BLACKW") || checkString.Equals("WBLACK"))
        {         //BLACK & WHITE OR WHITE & BLACK 
            return "BLACKWHITE";
        }
        else if (checkString.Equals("BLACKBLUE") || checkString.Equals("BLUEBLACK"))
        {         //BLACK & BLUE OR BLUE & BLACK
            return "BLACKBLUE";
        }
        //============================================================================= CHECK RED COMBOS ====================================
        else if (checkString.Equals("RR"))
        {         //RED & RED   
            return "REDRED";
        }
        else if (checkString.Equals("RW") || checkString.Equals("WR"))
        {         //RED & WHITE OR WHITE & RED   
            return "REDWHITE";
        }
        else if (checkString.Equals("RBLUE") || checkString.Equals("BLUER"))
        {         //RED & BLUE OR BLUE & RED   
            return "REDBLUE";
        }
        //============================================================================= CHECK WHITE COMBOS ====================================
        else if (checkString.Equals("WW"))
        {         //WHITE & WHITE  
            return "WHITEWHITE";
        }
        else if (checkString.Equals("WBLUE") || checkString.Equals("BLUEW"))
        {         //WHITE & BLUE OR BLUE & WHITE   
            return "WHITEBLUE";
        }
        //============================================================================= CHECK BLUE COMBOS  ====================================
        else if (checkString.Equals("BLUEBLUE"))
        {         //BLUE & BLUE
            return "BLUEBLUE";
        }
        //else there is too many colours in the string, or the strings are misspelt -- its and invalid combo
        else
        {
            Debug.Log("There are too many strings inside the checkString string, meaning there might be too many clients, or too many strings.");
            return "Error 2: Too many players/strings.";
        }

    }

    [Server]
    public void serverCreateDeck(string checkCombo) //want to have method for checking if enough players have joined
    {
        //once 2 clients have connected and they have a valid combo, the server can populate the combinedDeck using the proper combo
        //WANT SERVER TO CREATE DECK, AND THEN DEAL CARDS TO CLIENT USING RPC

        //redudant combo check again
        //string finalCombo = checkFor2PlayersAndTheirCombo(checkCombo); 

        if (checkCombo.Equals("Error 1: Not enough players/strings."))       // first error code
        {
            Debug.Log("There is not enough players.");
        }
        //============================ BLACK COMBOS =====================================================================           ***BLACK == 1
        else if (checkCombo.Equals("BLACKBLACK"))       //IF both players chose BLACK deck, combinedDeck becomes 2 black decks
        {
            //deal and shuffle 2 black decks together to be dealt to clients Done inside the functions below
            populateTempCombinedDeck(11);

            shuffleTempCombinedDeck();
        }
        else if (checkCombo.Equals("BLACKRED"))       //IF players chose 1 BLACK and 1 Red deck, combinedDeck becomes blackred deck
        {
            //deal and shuffle 1 black and 1 red deck together to be dealt to clients Done inside the functions below
            populateTempCombinedDeck(12);

            shuffleTempCombinedDeck();
        }
        else if (checkCombo.Equals("BLACKWHITE"))       //IF players chose 1 BLACK and 1 White deck, combinedDeck becomes blackwhite deck
        {
            //deal and shuffle 1 black and 1 white deck together to be dealt to clients Done inside the functions below
            populateTempCombinedDeck(13);

            shuffleTempCombinedDeck();
        }
        else if (checkCombo.Equals("BLACKBLUE"))       //IF players chose 1 BLACK and 1 Blue deck, combinedDeck becomes blackblue deck
        {
            //deal and shuffle 1 black and 1 blue deck together to be dealt to clients Done inside the functions below
            populateTempCombinedDeck(14);

            shuffleTempCombinedDeck();
        } //============================ RED COMBOS =====================================================================            ***RED == 2
        else if (checkCombo.Equals("REDRED"))       //IF both players chose RED deck, combinedDeck becomes 2 red decks
        {
            //deal and shuffle 2 red decks together to be dealt to clients Done inside the functions below
            populateTempCombinedDeck(22);

            shuffleTempCombinedDeck();
        }
        else if (checkCombo.Equals("REDWHITE"))       //IF players chose 1 Red and 1 White deck, combinedDeck becomes redwhite deck
        {
            //deal and shuffle 1 red and 1 white deck together to be dealt to clients Done inside the functions below
            populateTempCombinedDeck(23);

            shuffleTempCombinedDeck();
        }
        else if (checkCombo.Equals("REDBLUE"))       //IF players chose 1 BLACK and 1 White deck, combinedDeck becomes redblue deck
        {
            //deal and shuffle 1 red and 1 blue deck together to be dealt to clients Done inside the functions below
            populateTempCombinedDeck(24);

            shuffleTempCombinedDeck();
        }//============================ WHITE COMBOS =====================================================================          ***WHITE == 3
        else if (checkCombo.Equals("WHITEWHITE"))       //IF both players chose White deck, combinedDeck becomes 2 white decks
        {
            //deal and shuffle 2 white decks together to be dealt to clients Done inside the functions below
            populateTempCombinedDeck(33);

            shuffleTempCombinedDeck();
        }
        else if (checkCombo.Equals("WHITEBLUE"))       //IF players chose 1 White deck anbd 1 Blue deck, combinedDeck becomes whiteblue deck
        {
            //deal and shuffle 1 white and 1 blue deck together to be dealt to clients Done inside the functions below
            populateTempCombinedDeck(34);

            shuffleTempCombinedDeck();
        }//============================ BLUE COMBOS =====================================================================           ***BLUE == 4
        else if (checkCombo.Equals("BLUEBLUE"))       //IF both players chose Blue deck, combinedDeck becomes 2 blue decks
        {
            //deal and shuffle 2 blue decks together to be dealt to clients Done inside the functions below
            populateTempCombinedDeck(44);

            shuffleTempCombinedDeck();
        }
        else
        {
            Debug.Log("The Combo was invalid.");
        }
    }

    //NEED TO CREATE ALL 5 Decks
    //we've got 8 NEUTRAL cards that will be mixed in to every other deck
    public static List<Card1> neutralCardList = new List<Card1>();

    public void populateNeutralList()
    { //colour 5 is for neutral since 0 is the old deck and 1-4 are the colours

        //colour,  id,  name,  cost, pow,  hp,  description
        neutralCardList.Add(new Card1(5, 0, "Stick Man", 1, 2, 3, "No abilities. Just a Dude."));
        neutralCardList.Add(new Card1(5, 1, "Stick Man", 1, 2, 3, "No abilities. Just a Dude."));
        neutralCardList.Add(new Card1(5, 2, "PickPocket", 2, 1, 3, "Selectively pick a card that matches your chosen deck colour from the player deck."));
        neutralCardList.Add(new Card1(5, 3, "PickPocket", 2, 1, 3, "Selectively pick a card that matches your chosen deck colour from the player deck."));
        neutralCardList.Add(new Card1(5, 4, "ToyPistol", 5, 3, 4, "Deal 3 damage to target."));
        neutralCardList.Add(new Card1(5, 5, "ToyPistol", 5, 3, 4, "Deal 3 damage to target."));
        neutralCardList.Add(new Card1(5, 6, "Toy Bomb", 7, 3, 3, "Destroy all other creatures currently in play."));
        neutralCardList.Add(new Card1(5, 7, "Toy Bomb", 7, 3, 3, "Destroy all other creatures currently in play."));

    } // end NEUTRAL deck ============================================================

    //this will be the BLACK deck of cards. Its made up of 12 cards that specialize in high risk high reward play styles. 
    public static List<Card1> blackCardList = new List<Card1>();

    public void populateBlackList()
    { //colour 1 is for black cards

        //colour,  id,  name,  cost, pow,  hp,  description
        blackCardList.Add(new Card1(1, 0, "Curse Gamble", 1, 3, 3, "Deal 2 damage to yourself."));
        blackCardList.Add(new Card1(1, 1, "Curse Gamble", 1, 3, 3, "Deal 2 damage to yourself."));

        blackCardList.Add(new Card1(1, 2, "Eye for an Eye", 3, 1, 2, "Discard a random card from your hand, but destroy a target creature as a trade."));
        blackCardList.Add(new Card1(1, 3, "Eye for an Eye", 3, 1, 2, "Discard a random card from your hand, but destroy a target creature as a trade."));

        blackCardList.Add(new Card1(1, 4, "Necromancer's Summon", 7, 5, 5, "If this card gets discarded, it gets summoned to the play area instead."));
        blackCardList.Add(new Card1(1, 5, "Necromancer's Summon", 7, 5, 5, "If this card gets discarded, it gets summoned to the play area instead."));

        blackCardList.Add(new Card1(1, 6, "Russian Roulette", 6, 3, 4, "Deal 1 damage for every card you've discarded so far to a single random enemy."));
        blackCardList.Add(new Card1(1, 7, "Russian Roulette", 6, 3, 4, "Deal 1 damage for every card you've discarded so far to a single random enemy."));

        blackCardList.Add(new Card1(1, 8, "Sacrificial Lamb", 3, 1, 1, "Discard a random card, and then draw a new card for each card you have discarded so far."));
        blackCardList.Add(new Card1(1, 9, "Sacrificial Lamb", 3, 1, 1, "Discard a random card, and then draw a new card for each card you have discarded so far."));

        blackCardList.Add(new Card1(1, 10, "Vengeful/Spiteful Spirits", 4, 3, 2, "Discard a random card, and then deal 3 damage to all enemy creatures. If you have discarded 3 or more cards so far, then it deals 6 damage to all enemy creatures instead."));
        blackCardList.Add(new Card1(1, 11, "Vengeful/Spiteful Spirits", 4, 3, 2, "Discard a random card, and then deal 3 damage to all enemy creatures. If you have discarded 3 or more cards so far, then it deals 6 damage to all enemy creatures instead."));

    } //end BLACK deck  ============================================================


    //this will be the RED deck of cards. Its made up of 12 cards that specialize in aggressive but low cost play styles. 
    public static List<Card1> redCardList = new List<Card1>();

    public void populateRedList()
    { //colour 2 is for red cards

        //colour,  id,  name,  cost, pow,  hp,  description
        redCardList.Add(new Card1(2, 0, "Toxic Blade", 1, 2, 1, "If this creature attacks an enemy, deal another 2 damage."));
        redCardList.Add(new Card1(2, 1, "Toxic Blade", 1, 2, 1, "If this creature attacks an enemy, deal another 2 damage."));

        redCardList.Add(new Card1(2, 2, "Beserker", 2, 1, 1, "Gives all your creatures played a +1/+0 stat buff."));
        redCardList.Add(new Card1(2, 3, "Beserker", 2, 1, 1, "Gives all your creatures played a +1/+0 stat buff."));

        redCardList.Add(new Card1(2, 4, "Summoner", 4, 2, 3, "Draw a new card for each creature you currently control."));
        redCardList.Add(new Card1(2, 5, "Summoner", 4, 2, 3, "Draw a new card for each creature you currently control."));

        redCardList.Add(new Card1(2, 6, "Dogpiler", 5, 2, 4, "Deal 2 damage for every creature you have attacked with so far, then destroy all other creatures."));
        redCardList.Add(new Card1(2, 7, "Dogpiler", 5, 2, 4, "Deal 2 damage for every creature you have attacked with so far, then destroy all other creatures."));

        redCardList.Add(new Card1(2, 8, "Revenge Seeker", 3, 2, 2, "Gives all your creatures played a +2/+0 stat buff."));
        redCardList.Add(new Card1(2, 9, "Revenge Seeker", 3, 2, 2, "Gives all your creatures played a +1/+0 stat buff."));

        redCardList.Add(new Card1(2, 10, "Suicide Pact", 8, 4, 4, "Destroy all of your other creatures in play, and for each creature you sacrifice, deal 2 damage to a random enemy target."));
        redCardList.Add(new Card1(2, 11, "Suicide Pact", 8, 4, 4, "Destroy all of your other creatures in play, and for each creature you sacrifice, deal 2 damage to a random enemy target."));

    } //end RED deck  =============================================================


    //this will be the WHITE deck of cards. Its made up of 12 cards that specialize in lifegain and more slow sustainability play styles. 
    public static List<Card1> whiteCardList = new List<Card1>();

    public void populateWhiteList()
    { //colour 3 is for white cards

        //colour,  id,  name,  cost, pow,  hp,  description
        whiteCardList.Add(new Card1(3, 0, "Healer", 1, 1, 1, "Heal yourself for 2 damage. Maybe also able to heal other creatures."));
        whiteCardList.Add(new Card1(3, 1, "Healer", 1, 1, 1, "Heal yourself for 2 damage. Maybe also able to heal other creatures."));

        whiteCardList.Add(new Card1(3, 2, "Spiteful Healer", 4, 2, 2, "Heal yourself for 2 damage, and deal 2 damage to the enemy as well."));
        whiteCardList.Add(new Card1(3, 3, "Spiteful Healer", 4, 2, 2, "Heal yourself for 2 damage, and deal 2 damage to the enemy as well."));

        whiteCardList.Add(new Card1(3, 4, "Lucky Healer", 5, 5, 5, "Draw a new card, and heal yourself an amount of damage equal to the cost of the card that was drawn."));
        whiteCardList.Add(new Card1(3, 5, "Lucky Healer", 5, 5, 5, "Draw a new card, and heal yourself an amount of damage equal to the cost of the card that was drawn."));

        whiteCardList.Add(new Card1(3, 6, "Field Medic", 3, 2, 4, "Heal a target creature for 3 damage."));
        whiteCardList.Add(new Card1(3, 7, "Field Medic", 3, 2, 4, "Heal a target creature for 3 damage."));

        whiteCardList.Add(new Card1(3, 8, "Copycat", 4, 3, 2, "Summon a copy of this creature if you have healed 5 damage so far."));
        whiteCardList.Add(new Card1(3, 9, "Copycat", 4, 3, 2, "Summon a copy of this creature if you have healed 5 damage so far."));

        whiteCardList.Add(new Card1(3, 10, "Karma Heal", 6, 5, 5, "Deal an amount of damage equal to the amount you have healed this game to the enemy."));
        whiteCardList.Add(new Card1(3, 11, "Karma Heal", 6, 5, 5, "Deal an amount of damage equal to the amount you have healed this game to the enemy."));

    } //end WHITE deck  =============================================================


    //this will be the BLUE deck of cards. Its made up of 12 cards that specialize in chaos and more disruptive play syles. Draw tons of cards
    public static List<Card1> blueCardList = new List<Card1>();

    public void populateBlueList()
    { //colour 4 is for blue cards

        //colour,  id,  name,  cost, pow,  hp,  description
        blueCardList.Add(new Card1(4, 0, "Damage Dealer", 2, 1, 1, "Deal damage equal to the amount of cards you've drawn this turn to one enemy minion."));
        blueCardList.Add(new Card1(4, 1, "Damage Dealer", 2, 1, 1, "Deal damage equal to the amount of cards you've drawn this turn to one enemy minion."));

        blueCardList.Add(new Card1(4, 2, "Minion Hunter", 4, 2, 2, "Destroy one enemy minion."));
        blueCardList.Add(new Card1(4, 3, "Minion Hunter", 4, 2, 2, "Destroy one enemy minion."));

        blueCardList.Add(new Card1(4, 4, "Firesale Shuffle", 2, 1, 2, "Draw a new card, and reduce its cost by 1."));
        blueCardList.Add(new Card1(4, 5, "Firesale Shuffle", 2, 1, 2, "Draw a new card, and reduce its cost by 1."));

        blueCardList.Add(new Card1(4, 6, "Balance the Scales", 5, 3, 5, "Draw new cards until you have the same amount of cards in your hand as your opponent."));
        blueCardList.Add(new Card1(4, 7, "Balance the Scales", 5, 3, 5, "Draw new cards until you have the same amount of cards in your hand as your opponent."));

        blueCardList.Add(new Card1(4, 8, "Clean Slate", 4, 3, 2, "You and your opponent both discard your entire hands. You then draw the same amount that you discarded plus 1 extra card."));
        blueCardList.Add(new Card1(4, 9, "Clean Slate", 4, 3, 2, "You and your opponent both discard your entire hands. You then draw the same amount that you discarded plus 1 extra card."));

        blueCardList.Add(new Card1(4, 10, "The House Always Wins", 6, 2, 2, "Deal 2 damage to a random enemy for every card you have drawn this game."));
        blueCardList.Add(new Card1(4, 11, "The House Always Wins", 6, 2, 2, "Deal 2 damage to a random enemy for every card you have drawn this game."));

    } //end BLUE deck  =============================================================

    //add 40 cards from the cardDatabase using 2 neutral decks plus two colours (which gets selected by player) -- ONLY SERVER CAN DO THIS
    [Server]
    void populateTempCombinedDeck(int selectedcombo)
    {
        //every deck gets 16 neutral cards added to it first (there is only 8 cards per deck regularly, but this is the combination of 2)
        populateNeutralList();
        Debug.Log("Attempting to add neutral cards...");
        for (int i = 0; i < 2; i++)
        {
            for (int k = 0; k < 8; k++)
            {

                //int offset = 8 * i;
                //Debug.Log("offset = " + offset + " and i = " + i + " and k = " + k); //should all be zero to start
                //combinedDeck[k + offset]
                //Debug.Log("combinedDeck equals = " + combinedDeck[(k + offset)]);
                temp_combinedDeck.Add(neutralCardList[k]);
            } //ADD 8 Neutral cards first iteration, then 2nd 8 cards in the 2nd iteration

        }//16 neutral cards should now be in 1st 16 slots of deck list
         //====================================================== ADD COLOURS =======================================================
         //int j = 16;//start adding the other 24 cards from each deck at index 16 because of neutral cards

        //===================================================== BLACK COMBOS =======================================================

        if (selectedcombo == 11)      //== 2 BLACK DECKS *********************************************************
        {
            populateBlackList();
            Debug.Log("Attempting to add black deck twice...");
            for (int i = 0; i < 2; i++) //need to loop through 12 cards of black deck twice
            {
                for (int k = 0; k < 12; k++)                //ADD first 12 BLACK cards starting at index 16
                {
                    //int offset = 1 * i;
                    temp_combinedDeck.Add(blackCardList[k]);
                    //j++;
                }
                //j will hit index 16+12 = 28 after first iteration , then 28+12 =40 for second
            }
            //j = 16; //reset j to 16 after loop is done, to make sure other decks don't get indexed incorrectly in future
        }
        else if (selectedcombo == 12)      //== 1 BLACK 1 RED DECK *********************************************************
        {
            Debug.Log("Attempting to add black and red cards...");
            populateBlackList();
            for (int i = 0; i < 12; i++)                //ADD 12 BLACK cards after first 16 neutral
            {
                temp_combinedDeck.Add(blackCardList[i]);
                //j++; //j should go from 16-28
            }
            populateRedList();
            for (int i = 0; i < 12; i++)                //ADD 12 RED cards after 12 black
            {
                temp_combinedDeck.Add(redCardList[i]);
                //j++; //j should go from 28-40
            }
            //j = 16; //reset j to 16 after loop is done, to make sure other decks don't get indexed incorrectly in future
        }
        else if (selectedcombo == 13)      //== 1 BLACK 1 WHITE DECK *********************************************************
        {
            populateBlackList();
            Debug.Log("Attempting to add black and white cards...");
            for (int i = 0; i < 12; i++)                //ADD 12 BLACK cards after first 16 neutral
            {
                temp_combinedDeck.Add(blackCardList[i]);
                //j++; //j should go from 16-28
            }
            populateWhiteList();
            for (int i = 0; i < 12; i++)                //ADD 12 WHITE cards after 12 black
            {
                temp_combinedDeck.Add(whiteCardList[i]);
                //j++; //j should go from 28-40
            }
            //j = 16; //reset j to 16 after loop is done, to make sure other decks don't get indexed incorrectly in future
        }
        else if (selectedcombo == 14)      //== 1 BLACK 1 BLUE DECK *********************************************************
        {
            populateBlackList();
            Debug.Log("Attempting to add black and blue cards...");
            for (int i = 0; i < 12; i++)                //ADD 12 BLACK cards after first 16 neutral
            {
                temp_combinedDeck.Add(blackCardList[i]);
                //j++; //j should go from 16-28
            }
            populateBlueList();
            for (int i = 0; i < 12; i++)                //ADD 12 BLUE cards after 12 black
            {
                temp_combinedDeck.Add(blueCardList[i]);
                //j++; //j should go from 28-40
            }
            //j = 16; //reset j to 16 after loop is done, to make sure other decks don't get indexed incorrectly in future
        }
        //===================================================== RED COMBOS =======================================================
        else if (selectedcombo == 22)      //== 2 RED DECKS *********************************************************
        {
            populateRedList();
            Debug.Log("Attempting to add red deck twice...");
            for (int i = 0; i < 2; i++) //need to loop through 12 cards of red deck twice
            {
                for (int k = 0; k < 12; k++)                //ADD first 12 RED cards starting at index 16
                {
                    //int offset = 1 * i;
                    temp_combinedDeck.Add(redCardList[k]);
                    //j++;
                }
                //j will hit index 16+12 = 28 after first iteration , then 28+12 =40 for second
            }
            //j = 16; //reset j to 16 after loop is done, to make sure other decks don't get indexed incorrectly in future
        }
        else if (selectedcombo == 23)      //== 1 RED 1 WHITE DECK *********************************************************
        {
            populateRedList();
            Debug.Log("Attempting to add red and white cards...");
            for (int i = 0; i < 12; i++)                //ADD 12 RED cards after first 16 neutral
            {
                temp_combinedDeck.Add(redCardList[i]);
                //j++; //j should go from 16-28
            }
            populateWhiteList();
            for (int i = 0; i < 12; i++)                //ADD 12 WHITE cards after 12 red
            {
                temp_combinedDeck.Add(whiteCardList[i]);
                //j++; //j should go from 28-40
            }
            //j = 16; //reset j to 16 after loop is done, to make sure other decks don't get indexed incorrectly in future
        }
        else if (selectedcombo == 24)      //== 1 RED 1 BLUE DECK *********************************************************
        {
            populateRedList();
            Debug.Log("Attempting to add red and blue cards...");
            for (int i = 0; i < 12; i++)                //ADD 12 RED cards after first 16 neutral
            {
                temp_combinedDeck.Add(redCardList[i]);
                //j++; //j should go from 16-28
            }
            populateBlueList();
            for (int i = 0; i < 12; i++)                //ADD 12 BLUE cards after 12 red
            {
                temp_combinedDeck.Add(blueCardList[i]);
                //j++; //j should go from 28-40
            }
            //j = 16; //reset j to 16 after loop is done, to make sure other decks don't get indexed incorrectly in future
        }
        //===================================================== WHITE COMBOS =======================================================
        else if (selectedcombo == 33)      //== 2 WHITE DECKS *********************************************************
        {
            populateWhiteList();
            Debug.Log("Attempting to add white deck twice...");
            for (int i = 0; i < 2; i++) //need to loop through 12 cards of white deck twice
            {
                for (int k = 0; k < 12; k++)                //ADD first 12 WHITE cards starting at index 16
                {
                    //int offset = 1 * i;
                    temp_combinedDeck.Add(whiteCardList[k]);
                    //j++;
                }
                //j will hit index 16+12 = 28 after first iteration , then 28+12 =40 for second
            }
            //j = 16; //reset j to 16 after loop is done, to make sure other decks don't get indexed incorrectly in future
        }
        else if (selectedcombo == 34)      //== 1 WHITE 1 BLUE DECK *********************************************************
        {
            populateWhiteList();
            Debug.Log("Attempting to add white and blue cards...");
            for (int i = 0; i < 12; i++)                //ADD 12 WHITE cards after first 16 neutral
            {
                temp_combinedDeck.Add(whiteCardList[i]);
                //j++; //j should go from 16-28
            }
            populateBlueList();
            for (int i = 0; i < 12; i++)                //ADD 12 BLUE cards after 12 white
            {
                temp_combinedDeck.Add(blueCardList[i]);
                //j++; //j should go from 28-40
            }
            //j = 16; //reset j to 16 after loop is done, to make sure other decks don't get indexed incorrectly in future
        }
        //===================================================== BLUE COMBOS =======================================================
        else if (selectedcombo == 44)      //== 2 BLUE DECKS *********************************************************
        {
            populateBlueList();
            Debug.Log("Attempting to add blue deck twice...");
            for (int i = 0; i < 2; i++) //need to loop through 12 cards of blue deck twice
            {
                for (int k = 0; k < 12; k++)                //ADD first 12 BLUE cards starting at index 16
                {
                    //int offset = 1 * i; //second round will add indexes 28-40
                    temp_combinedDeck.Add(blueCardList[k]);
                    //j++;
                }
                //j will hit index 16+12 = 28 after first iteration , then 28+12 =40 for second
            }
            //j = 16; //reset j to 16 after loop is done, to make sure other decks don't get indexed incorrectly in future
        }

    }//end populateCombinedDeck

    [Server]
    public void shuffleTempCombinedDeck() //takes whatever deck is, and shuffles it using random indexing
    {
        for (int i = 0; i < deckSize; i++)
        {
            container.Add(temp_combinedDeck[i]); //always store current combinedDeck value in first slot of container's list
            int randomIndex = Random.Range(i, deckSize);
            temp_combinedDeck[i] = temp_combinedDeck[randomIndex];
            temp_combinedDeck[randomIndex] = container[0];
            container.RemoveAt(0); //delete container's first value after use, so next card is shuffled properly
        }
        //now that deck is shuffled, change SYNC VAR bool variable to reflect this
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
        return temp_combinedDeck[0].ToString();
    }


    void Update(){
        //update any public data
        p1HP = p1Health; 
        p2HP = p2Health;
        staticTurn = whosTurn;
    }
}
