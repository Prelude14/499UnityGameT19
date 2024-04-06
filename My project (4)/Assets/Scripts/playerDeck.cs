using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class playerDeck : MonoBehaviour
{
    public List<Card1> container = new List<Card1>();
    public static int deckSize;
    public int handSize = 0;
    public List<Card1> deck = new List<Card1>();
    public static List<Card1> staticDeck = new List<Card1>();
    public int x;
    public static int staticAmount;
    public static bool drawStatic = false;
    public GameObject cardInDeck1;
    public GameObject cardInDeck2;
    public GameObject cardInDeck3;
    public int burnDamage = 1;
    public GameObject cardInDeck4;
    public GameObject cardInDeck5;
    public GameObject cardInDeck6;

    public GameObject[] clones;
    public GameObject hand;
    public GameObject cardInHand;
    public Text deckCountText;

    public static int playerColour;

    // Start is called before the first frame update
    void Start()
    {

        x = 0;
        deckSize = 20;

        //populate card list depending on decks selected. Done inside the function
        populateDeck();

        shuffle();
        StartCoroutine(StartGame());

    }
    // Update is called once per frame
    void Update()
    {
        //reduce number of "visible" cards on the -deck- stack
        if (drawStatic == true)
        {
            Debug.Log("DRAW TRIGGER: " + staticAmount);
            StartCoroutine(Draw(staticAmount));
            drawStatic = false;
        }
        staticDeck = deck;

        changeSize();

        //display deck card count;
        deckCountText.text = "" + deckSize;
        if (turnScript.turnStart == true)
        {
            StartCoroutine(Draw(1));
            turnScript.turnStart = false;
        }
    }
    IEnumerator StartGame()
    {
        //coroutine: way to count down
        //initial draw
        for (int i = 0; i < 7; i++)
        { // number of starting hand
            yield return new WaitForSeconds(.15f);
            //each second it draws a card
            //spawns new object using instantiate duplicating it as a clone of cardInHand
            GameObject card = Instantiate(cardInHand, new Vector2(0, 0), Quaternion.identity);

            //renderCardColour(card);
        }
        Debug.Log(turnScript.currentMana);

    }

    public void changeSize()
    {
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
    }
    public void shuffle()
    {
        for (int i = 0; i < deckSize; i++)
        {
            container[0] = deck[i];
            int randomIndex = Random.Range(i, deckSize);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = container[0];
        }
        //container should show 
        // string result = "";
        // for (int i = 0; i < deck.Count; i++) {
        //     result = result + deck[i].print() + ",";
        // }
        // Debug.Log(result);
    }

    IEnumerator Draw(int drawSize)
    {
        if (deckSize > 0) //check if there's other cards. do not draw if no more cards do smth else;
        {
            //draw cards repeat until drawSize
            for (int x = 0; x < drawSize; x++)
            {
                //slow down code so we don't draw too fast
                yield return new WaitForSeconds(.15F);
                //Instantiate(cardInHand, transform.position, transform.rotation);
                GameObject card = Instantiate(cardInHand, new Vector2(0, 0), Quaternion.identity);

                //renderCardColour(card);
            }
        }
        else
        {
            //lose game
            //TODO: Add a lose health

            playerHealth.HPStatic -= burnDamage;
            burnDamage++;
        }
        drawStatic = false;
    }

    //add 20 cards from the cardDatabase using a neutral deck plus one colour (which gets selected by player)
    public void populateDeck()
    {
        //every deck gets the same 8 neutral cards added to it first
        for (int i = 0; i < 8; i++)
        {
            deck[i] = cardDatabase.neutralCardList[i];
        } //ADD 8 Neutral cards first

        int j = 8;//start adding the other 12 cards from each deck at index 8 because of neutral cards
        if (playerColour == 1)          // 1 == BLACK DECK
        {
            for (int i = 0; i < 12; i++)                //ADD 12 BLACK cards after first 8
            {
                deck[j] = cardDatabase.blackCardList[i];
                j++;
            }
            j = 8; //reset j to 8 after loop is done
        }
        else if (playerColour == 2)          // 2 == RED DECK
        {
            for (int i = 0; i < 12; i++)                //ADD 12 RED cards after first 8
            {
                deck[j] = cardDatabase.redCardList[i];
                j++;
            }
            //reset j to 8 after loop is done
        }
        else if (playerColour == 3)          // 3 == WHITE DECK
        {
            for (int i = 0; i < 12; i++)                //ADD 12 WHITE cards after first 8
            {
                deck[j] = cardDatabase.whiteCardList[i];
                j++;
            }
            //reset j to 8 after loop is done
        }
        else if (playerColour == 4)          // 4 == BLUE DECK
        {
            for (int i = 0; i < 12; i++)                //ADD 12 BLUE cards after first 8
            {
                deck[j] = cardDatabase.blueCardList[i];
                j++;
            }
            //reset j to 8 after loop is done
        }

    }


    public int getDeckSize()
    {
        return deckSize;
    }
    public string getDeckTop()
    {
        return deck[0].ToString();
    }

}
