using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class passReset : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Button resetPassButton;

       public void CallResetToken()
    {
        StartCoroutine(sendResetToken());
    }

    IEnumerator sendResetToken()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", DBManager.username);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/resetPass.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else 
        {
            Debug.Log("Form upload complete!");
        }
    }

    // public void VerifyInputL() //button won't be clickable until input field has at least 10 characters
    // {
    //     resetPassButton.interactable = (DBManager.username.text.Length >= 10);
    // }

}
