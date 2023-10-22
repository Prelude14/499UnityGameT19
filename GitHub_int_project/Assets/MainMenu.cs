using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void ExitButton() {
        Application.Quit();
        Debug.Log("Game closed by button.");

    }

    public void StartGame() {
        SceneManager.LoadScene("SampleScene");

    }
}
