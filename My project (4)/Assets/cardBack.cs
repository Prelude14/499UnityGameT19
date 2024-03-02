using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardBack : MonoBehaviour
{
    public GameObject CardBack;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (dbDisplay.staticCardBack == true)
        {
            //if staticcardback is true we see back of the card
            CardBack.SetActive(true);
        }
        else
        {
            //else we see the front
            CardBack.SetActive(false);
        }
    }
}
