using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class updatePass : MonoBehaviour
{
    public Text password;
    public Text username;
    public GameObject newPasswordInput;
    public GameObject confirmPasswordUpdate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
      public void CallUpdate()
    {
        StartCoroutine(update());
    }

    IEnumerator update()
    {
        //make Form to take the user's input 
        WWWForm form = new WWWForm();
        form.AddField("username", username.text);
        form.AddField("password", password.text);

        //connect to url of our database's php file, PASS FORM TO URL
        WWW www = new WWW("http://localhost/sqlconnect/update.php", form);
        yield return www; //tell Unity to yield running the rest of the game till it gets this info from the url

        //Error check what our PHP file returned, index 0 should be the first character, 0 means everything worked perfectly
        if (www.text[0] == '0')
        {
            Debug.Log("Token Valid");
            confirmPasswordUpdate.SetActive(true);
            newPasswordInput.SetActive(false);
        }
        else
        {
            Debug.Log("Validation FAILED. Error Code: " + www.text);
            password.color = Color.red;
        }
    }
}
