using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardInHand : MonoBehaviour
{
    public GameObject hand;
    public GameObject handCard;

    // Start is called before the first frame update
    void Start()
    {
        hand = GameObject.Find("hand");

        //change parent of card to be under hand panel
        handCard.transform.SetParent(hand.transform);

        handCard.transform.localScale = Vector3.one;
        handCard.transform.position = new Vector3(transform.position.x, transform.position.y, -48);
        handCard.transform.eulerAngles = new Vector3(25, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        //search game object named ahnd4
        handCard.transform.localScale = Vector3.one;

    }
}
