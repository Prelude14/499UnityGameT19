using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; //important --> use this for db method;
using Mirror; //need this script to be outside of script folder in order for it to use mirror for some reason (EACH CARD SPAWNED has copy of this script)
using TMPro;

public class dbDisplay : NetworkBehaviour
{
    public List<Card1> displayList = new List<Card1>();
    public int displayId;
    public static bool attackDragging;
    public int colour; //for each card's colour
    public static int staticColour;
    public int id;
    public static int staticID;
    public int hp;
    public static int staticHP;
    public int pow;
    public static int staticPow;
    public int cost;
    public static int staticCost;
    public string cardName;
    public static string staticName;
    public string txt;
    public static string staticTxt;

    public TMPro.TMP_Text nameText;
    public TMPro.TMP_Text descriptionText;
    public Image artworkImage;

    public TMPro.TMP_Text costText;
    public TMPro.TMP_Text powText;
    public TMPro.TMP_Text hpText;


    public bool cardBack;
    public static bool staticCardBack;

    public GameObject cardInHand;
    public GameObject hand;
    public GameObject oppHand;
    public GameObject playZone;
    public GameObject oppPlayPanel;
    public GameObject currentZone;
    public static int deckCount;

    public bool canBeSummoned;
    public bool isSummoned;

    public bool canAttack;
    public bool cantAttack;
    public static bool hasAttacked;

    public bool targeting;
    public bool targetingEnemy;
    public static bool staticTargeting;
    public static bool staticTargetingEnemy;
    public bool onlyThisCardAttack;
    public GameObject Target;
    public GameObject Enemy;
    public static bool staticSummoned;
    public bool currentlyDraggable = false;
    public bool attackBorder;
    public static bool staticAttackBorder;
    public GameObject Image;
    public GameObject playableBorder;
    public GameObject unplayableBorder;
    public static GameObject currentLoc;
    public static GameObject pz;

    public int attackCount = 0;

    //testing zoom in dbdisplay
    public GameObject Canvas;
    public GameObject ZoomCard;

    private GameObject zoomCard;
    private Sprite zoomSprite;

    //turn chekcing
    public int lastTurn;
    public int currentTurn;

    //need access to player manager script that is unique to each client
    public PlayerManager PlayerManager;

    // Start is called before the first frame update (Find each card's info to be displayed)
    void Start()
    {
        //Debug.Log("dbDisplay started...");
        deckCount = playerDeck.deckSize;

        
        displayList[0] = playerDeck.staticDeck[displayId]; //get card from playerDeck's static deck PLACEHOLDER********

        this.id = displayList[0].id;

        isSummoned = false;
        Enemy = GameObject.Find("enemyHealth");
        canAttack = false;
        targeting = false;
        targetingEnemy = false;
        if (cost <= 1)
        {
            canBeSummoned = true;
        }
        else
        {
            canBeSummoned = false;
        }
        staticAttackBorder = false;
        attackBorder = false;
        playableBorder.SetActive(false);
        unplayableBorder.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(attackCount == 2){
            NetworkServer.Destroy(this.gameObject);
        }
        currentTurn = SharedVarManager.staticTurn;
        if(currentTurn != lastTurn){
            hasAttacked = false;
            lastTurn = currentTurn;
        }
        staticColour = colour;

        staticAttackBorder = false;
        staticCost = cost;
        staticSummoned = isSummoned;
        //Debug.Log(staticSummoned + " " + cardName);
        displayCard(); //sets the card's information and colour up to be rendered INTO PLACEHOLDER***
        hand = GameObject.Find("hand");
        //if this parent is the same as the hands parent 
        Debug.Log("this.transform.parent = "+ this.transform.parent);
        //if (this.transform.parent.gameObject == hand) //if this script's parent's gameobject is equal to the hand object, then show face of card to us
        //{
        //    cardBack = false;
        //}
        staticCardBack = cardBack; //make sure that the card doesn't show the back
        cloneDraw(); //this updates the card to be the last card in the static deck (which is updated to have one less each time a card is drawn)

        //currentZone = this.transform.parent; //supposed to grab each card's parent game object (hand, oppHand, playPanel, or oppPlayPanel)

        if (this.transform.parent != null)
        {
            currentZone = this.transform.parent.gameObject;// had this from multplay but changed to fix merge conflicts this.transform.parent.gameObject;
        }
        else
        {
            Debug.Log("Parent transform is null for object: " + this.name);
        }


        //check if currZone is oppHand, set cardBack to true to not show opponent's cards
        oppHand = GameObject.Find("oppHand");
        if (currentZone == oppHand)
        {
            cardBack = true;
            //canBeSummoned = false; //can't drag opp's cards
            //dragScript.isDraggable = false; //can't drag opp's cards
        }


        //check if currZone is oppPlayPanel, set cardBack to false in order to now show opponent's card
        oppPlayPanel = GameObject.Find("oppPlayPanel");
        if (currentZone == oppPlayPanel)
        {
            cardBack = false;
            //canBeSummoned = false; //can't drag opp's cards
            //dragScript.isDraggable = false; //can't drag opp's cards
        }

        currentLoc = currentZone;

        playZone = GameObject.Find("playPanel");
        pz = playZone;
        //summoning logic and cost logic

        //Debug.Log(cardName + " Is summoned false");
        if (this.cost <= turnScript.currentMana && isSummoned == false && currentZone == hand ) //added check to stop opponent's cards from being playable if you have enough mana
        {
            canBeSummoned = true;
            //currentlyDraggable = true; //set to be draggable
            //dragScript.isDraggable = true;
            Debug.Log(cardName + " Is now playable");
            if (currentZone == hand)
            {
                //Debug.Log(cardName + " Is now playable");

            }
        }
        else //if can't be played and your card, turn border grey to show unable to play
        {
            canBeSummoned = false;
            //currentlyDraggable = false; //check if draggable (can only drag cards you can afford to play
            playableBorder.SetActive(false);
            unplayableBorder.SetActive(true);
        }

        //if can be played and your card, set border to green and make draggable
        if (canBeSummoned && currentZone == hand ) //extra check to ensure opponent's cards can't be played
        {
            if (turnScript.isMyTurn == true) { //AND its MY TURN
                dragScript.isDraggable = true;
                //currentlyDraggable = dragScript.isDraggable; //check if draggable
                Debug.Log(cardName + " is now green " + currentlyDraggable);
                playableBorder.SetActive(true); //turn green to signify card is playable
                unplayableBorder.SetActive(false);
            }
        }


        //GameObject startParent = transform.parent.gameObject;
        //if placed into playPanel from hand, check to actually place it there and deduct mana cost for playing it
        if (isSummoned == false && currentZone == playZone)
        {
            unplayableBorder.SetActive(false);
            playableBorder.SetActive(false);
            if (this.cost > turnScript.currentMana) //if you don't have enough mana, then send card back to your hand and do nothing
            {
                transform.SetParent(GameObject.Find("hand").transform, true);
                return;
            }
            isSummoned = true; //if you can afford it, then card becomes "summoned", or played
            turnScript.totalSummons++;
            Debug.Log(cardName + " Summoned sucess | Cost: " + this.cost + " | Current zone: " + currentZone + " | play zone: " + playZone + " | Is summoned? " + isSummoned);
            //disable script component when summoned
            GetComponent<dragScript>().enabled = false; //don't let card be dragged once played
            // turnScript.currentMana = turnScript.currentMana - this.cost; //take proper mana cost from player
            // Debug.Log("Mana left: " + turnScript.currentMana);

        }

        currentlyDraggable = dragScript.isDraggable; //check if draggable

        //decide attackers
        if (turnScript.isMyTurn == true && isSummoned == true && hasAttacked == false && currentZone == playZone)
        {
            cantAttack = false;
            //Debug.Log(cardName + " ready to attack");
            unplayableBorder.SetActive(false);
            playableBorder.SetActive(false);
            canAttack = true;
        }
        else
        {
            cantAttack = true;
            canAttack = false;
        }
        targeting = staticTargeting;
        targetingEnemy = staticTargetingEnemy;

        if (targetingEnemy == true)
        {
            Target = Enemy;
        }
        else
        {
            Target = null;
        }

        if (targeting == true && targetingEnemy == true && onlyThisCardAttack == true)
        {
            Attack();
        }

        if (canAttack == true && currentLoc == playZone)
        {
            attackBorder = true;
            staticAttackBorder = attackBorder;
        }
        else
        {
            staticAttackBorder = false;
            attackBorder = false;
        }

    }
    public void Awake()
    {
        Canvas = GameObject.Find("Canvas");
    }

    private void Attack()
    {
        if (canAttack == true && isSummoned )
        {

            if (Target != null)
            {

                if (Target == Enemy)
                {
                    //enemyHealth.HPStatic -= pow;
                    attackCount++;
                    attackEnemy(pow); //send pow of card attacking to attack command
                    
                    targeting = false;
                    cantAttack = true;
                    hasAttacked = true;
                    staticAttackBorder = false;
                    attackBorder = false;

                }

                if (Target.name == "cardInHand(Clone)")
                {
                    canAttack = true;
                }
            }
        }
    }
    public void attackEnemy(int damage)
    {
        //locate the PlayerManager in the Client, need to call command on server to update health counts
        NetworkIdentity networkAttackIdentity = NetworkClient.connection.identity;
        PlayerManager = networkAttackIdentity.GetComponent<PlayerManager>();

        PlayerManager.CmdSendAttackInfo(damage, networkAttackIdentity); //call playermanager's cmd that calls server's attackPlayer CMD
        Debug.Log("Attacked opponent...attack sent CmdSendAttackInfo to playermanager...");
        abilityScript.attacked = true;
    }

    public void UntargetEnemy()
    {
        staticTargetingEnemy = false;
    }
    public void TargetEnemy()
    {
        staticTargetingEnemy = true;
    }
    public void StartAttack()
    {
        staticTargeting = true;
        if (currentZone == playZone)
        {
            attackDragging = true;
        }
    }
    public void StopAttack()
    {
        staticTargeting = false;
        if (currentZone == playZone)
        {
            attackDragging = false;
        }

    }
    public void OneCardAttack()
    {
        onlyThisCardAttack = true;
    }
    public void OneCardAttackStop()
    {
        onlyThisCardAttack = false;
    }
    private void displayCard()
    {
        this.id = displayList[0].id;//update ids to match real id of card
        this.cardName = displayList[0].cardName;
        this.colour = displayList[0].colour;//new
        this.pow = displayList[0].pow;
        this.hp = displayList[0].hp;
        this.txt = displayList[0].txt;
        this.cost = displayList[0].cost;

        nameText.text = " " + this.cardName.ToString();
        descriptionText.text = " " + this.txt.ToString();
        costText.text = " " + this.cost.ToString();
        powText.text = " " + this.pow.ToString();
        hpText.text = " " + this.hp.ToString();

        //trying to get the border of the card drwan to change colour to match the card's colour int
        Color border = renderCardColour(colour);//get what colour the border should be
        Image.GetComponent<Image>().color = border; //then render the correct colour


    }
    private void cloneDraw()
    {
        //Debug.Log("Made it to clone draw...");
        //clone cards for draw
        if (this.tag == "clone") //if this dbdisplay's card is a clone, then we draw it from the game deck and update the game deck's information
        {
            //
            displayList[0] = playerDeck.staticDeck[deckCount - 1]; //add last card of playerDeck's staticDeck to the top of the display list
            //
            deckCount -= 1;
            playerDeck.deckSize -= 1;
            cardBack = false;
            this.tag = "Untagged";
        }
    }

    public Color renderCardColour(int cardColour)
    {
        //need to check card1's colour int to see what colour to make the border of the card
        if (cardColour == 1)          // 1 == BLACK DECK
        {
            return Color.black;
        }
        else if (cardColour == 2)          // 2 == RED DECK
        {
            return Color.red;
        }
        else if (cardColour == 3)          // 3 == WHITE DECK
        {
            return Color.white;
        }
        else if (cardColour == 4)          // 4 == BLUE DECK
        {
            return Color.blue;
        }
        else
        {
            return Color.yellow;
        }
    }

    public void OnHoverEnter()
    {
        if(currentZone == oppHand){
            return; //if this is an opponent's card do not show
        }
        // Add logic for zooming in on hover enter
        Debug.Log("Zooming on: " + cardName);
        zoomCard = Instantiate(ZoomCard, new Vector2(Input.mousePosition.x + 100, 250), Quaternion.identity);
        zoomCard.transform.SetParent(Canvas.transform, true);
        RectTransform rect = zoomCard.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(200, 300);
        zoomCard.GetComponent<contentZoom>().cardName = cardName;
        zoomCard.GetComponent<contentZoom>().txt = txt;
        zoomCard.GetComponent<contentZoom>().cost = cost;
        zoomCard.GetComponent<contentZoom>().pow = pow;
        zoomCard.GetComponent<contentZoom>().hp = hp;
    }

    public void OnHoverExit()
    {
        // Add logic for zooming out on hover exit
        Destroy(zoomCard);
    }

}
