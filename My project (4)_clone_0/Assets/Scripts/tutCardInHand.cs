using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutCardInHand : MonoBehaviour
{
    public GameObject tuthand;
    public GameObject tuthandCard;

    // Start is called before the first frame update
    void Start()
    {
        tuthand = GameObject.Find("tutHand");

        //change parent of card to be under hand panel
        tuthandCard.transform.SetParent(tuthand.transform);
        tuthandCard.transform.localScale = Vector3.one;
        tuthandCard.transform.position = new Vector3(transform.position.x, transform.position.y, -48);
        tuthandCard.transform.eulerAngles = new Vector3(25, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        //search game object named ahnd4
       tuthandCard.transform.localScale = Vector3.one;

    }
}