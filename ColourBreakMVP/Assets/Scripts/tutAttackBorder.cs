using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutAttackBorder : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject AttackBorder;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (tutDbDisplay.staticAttackBorder == true)
        {
            //if staticcardback is true we see back of the card
            AttackBorder.SetActive(true);

        }
        else
        {
            //else we see the front
            AttackBorder.SetActive(false);
        }
    }
}