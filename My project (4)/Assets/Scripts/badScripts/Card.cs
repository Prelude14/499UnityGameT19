using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Card", menuName = "Card")] //tell unity make it possible to creat this object through create asset menu
public class Card : ScriptableObject
{

    public string cardName;

    public Sprite artwork;

    public int id;
    public int cost;
    public int pow;
    public int hp;
    public string txt;

    public Card() {

    } 
    public Card(int id, string cardName, int cost, int pow, int hp, string txt) {
        
        this.cardName = cardName;
        this.id = id;
        this.cost = cost;
        this.pow = pow;
        this.hp = hp;
        this.txt = txt;

    }

    public string print(){
        return (cardName  + " " + cost + " " + pow + " " + hp + " " + txt);
    }
    
}

