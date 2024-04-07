using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class updateEmailLabel : MonoBehaviour
{
    public Text emailLabel;
    public Text username;
    // Start is called before the first frame update
    void Start()
    {
        emailLabel.text = username.text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
