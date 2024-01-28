using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Card1
{

    public string cardName;

    public Sprite artwork;

    public int colour;
    public int id;
    public int cost;
    public int pow;
    public int hp;
    public string txt;


    public Card1()
    {

    }
    public Card1(int colour, int id, string cardName, int cost, int pow, int hp, string txt)
    {

        this.cardName = cardName;
        this.colour = colour;
        this.id = id;
        this.cost = cost;
        this.pow = pow;
        this.hp = hp;
        this.txt = txt;

    }

    public string print()
    {
        return (cardName + " " + cost + " " + pow + " " + hp + " " + txt);
    }

}

