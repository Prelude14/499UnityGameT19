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

    public GameObject cardInDeck1;
    public GameObject cardInDeck2;
    public GameObject cardInDeck3;

    public GameObject cardInDeck4;
    public GameObject cardInDeck5;
    public GameObject cardInDeck6;

    public GameObject[] clones;
    public GameObject hand;
    public GameObject cardInHand;
    public Text deckCountText;

    // Start is called before the first frame update
    void Start()
    {

        x = 0;
        deckSize = 20;
        //populate card list
        populateDeck();
        shuffle();
        StartCoroutine(StartGame());

    }

    // Update is called once per frame
    void Update()
    {
        //reduce number of "visible" cards on the -deck- stack
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
        for (int i = 0; i < 2; i++)
        { // number of starting hand
            yield return new WaitForSeconds(.15f);
            //each second it draws a card
            //spawns new object using instantiate duplicating it as a clone of cardInHand
            GameObject card = Instantiate(cardInHand, new Vector2(0, 0), Quaternion.identity);

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
            }
        }
        else
        {
            //lose game
            //TODO: Add a lose game

        }
    }
    public void populateDeck()
    {
        for (int i = 0; i < 20; i++)
        {
            deck[i] = cardDatabase.cardList[i];
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
