using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class resetText : MonoBehaviour
{
    public Text myText;

    // Start is called before the first frame update
    void Start()
    {
         myText.text = "A link to reset your password will be sent to the following email:\n\n" + DBManager.username;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
