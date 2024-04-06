using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutCardBack : MonoBehaviour
{
    public GameObject tutcardBack;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (tutDbDisplay.staticCardBack == true)
        {
            //if staticcardback is true we see back of the card
            tutcardBack.SetActive(true);
        }
        else
        {
            //else we see the front
            tutcardBack.SetActive(false);
        }
    }
}