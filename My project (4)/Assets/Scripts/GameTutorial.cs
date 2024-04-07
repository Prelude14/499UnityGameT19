using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GameTutorial : MonoBehaviour
{
    public GameObject oppTurnBtn;
    public GameObject myTurnBtn;
    public GameObject TutorialStartPanel;
    public GameObject TutSeq1;
    public GameObject TutSeq2;
    public GameObject TutSeq3;
    public UnityEvent OnAttackTutorialEnded;
    public GameObject TutorialEndPanel;
    public int currentSequence = 0;
    public bool IsTutorialStarted = false;
    // Start is called before the first frame update

    public static GameTutorial Instance;

    private void Awake()
    {
        Instance = this;

        if(PlayerPrefs.GetInt("TutorialPlayed") == 0)
        {
            TutorialStartPanel.gameObject.SetActive(true);
        }

    }
    public void StartTutorial()
    {
        IsTutorialStarted= true;
        StartSequence1();
    }
    public void StartSequence1()
    {
        if (!IsTutorialStarted) return;
        ++currentSequence;

        TutorialStartPanel.gameObject.SetActive(false);
        TutSeq1.gameObject.SetActive(true);
    }

    public void TutorialEneded()
    {
        PlayerPrefs.SetInt("TutorialPlayed", 1);
        oppTurnBtn.gameObject.SetActive(true);
        myTurnBtn.gameObject.SetActive(true);
        IsTutorialStarted = false;
    }

    public void AttachTutorialCompleted()
    {

        if (this.isActiveAndEnabled)
        {
           // TutorialEndPanel.gameObject.SetActive(true);
            OnAttackTutorialEnded?.Invoke();
        }
    }

    public void StartSequence2()
    {
        if (!IsTutorialStarted) return;

        ++currentSequence;

        TutSeq1.gameObject.SetActive(false);

        TutSeq2.gameObject.SetActive(true);
    }

    public void StartSequence3()
    {
        if (!IsTutorialStarted) return;

            TutSeq2.gameObject.SetActive(false);

            TutSeq3.gameObject.SetActive(true);
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void CardPick1Tutiral()
    {

    }
}
