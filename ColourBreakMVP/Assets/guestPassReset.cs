using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class guestPassReset : MonoBehaviour
{
    public InputField email;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void CallResetToken()
    {
        StartCoroutine(sendResetToken());
    }

    IEnumerator sendResetToken()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", email.text);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/resetPass.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            // print error message
        }
        else 
        {
            Debug.Log("Form upload complete!");
        }
    }
}
