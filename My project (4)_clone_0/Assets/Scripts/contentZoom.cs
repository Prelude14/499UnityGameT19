using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class contentZoom : MonoBehaviour
{

    public int displayId;

    public int colour; //for each card's colour

    public int id;
    public int hp;
    public int pow;
    public int cost;
    public string cardName;
    public string txt;

    public Text nameText;
    public Text descriptionText;
    public Image artworkImage;

    public Text costText;
    public Text powText;
    public Text hpText;




    // Start is called before the first frame update
    void Start()
    {



    }


    // Update is called once per frame
    void Update()
    {
        nameText.text = " " + cardName.ToString();
        descriptionText.text = " " + txt.ToString();
        costText.text = " " + cost.ToString();
        powText.text = " " + pow.ToString();
        hpText.text = " " + hp.ToString();
    }
    public static void setCards(string name, string txt, int pow, int hp, int cost)
    {

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



}