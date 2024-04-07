using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cardDisplay : MonoBehaviour
{
    public Card card;

    public Text nameText;
    public Text descriptionText;
    public Image artworkImage;

    public Text costText;
    public Text powText;
    public Text hpText;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(card.print());
        nameText.text = card.cardName;
        descriptionText.text = card.txt;

        artworkImage.sprite = card.artwork;

        costText.text = card.cost.ToString();
        powText.text = card.pow.ToString();
        hpText.text = card.hp.ToString();
    }

    
}
