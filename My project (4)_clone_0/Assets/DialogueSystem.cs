using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Threading;

[System.Serializable]
public class Block
{
    public string dialogue;
    public GameObject[] ObjectsToEnable;
    public GameObject[] ObjectsToDisable;

    public TextMeshProUGUI DialogueTxt;
    public UnityEvent OnBlockStartEvent;


}
public class DialogueSystem : MonoBehaviour
{
    public Button ContinueButton;
    public Block[] DialogueBlocks;
    public Coroutine DialogueRoutine;
    public int currentDialogueIndex = 0;
    public UnityEvent OnDialogueEnd;
   


    public void StartDialogue()
    {
        if (!this.isActiveAndEnabled) return;

        if (DialogueRoutine != null)
            StopCoroutine(DialogueRoutine);
        ++currentDialogueIndex;
        if (currentDialogueIndex > DialogueBlocks.Length)
        {
            OnDialogueEnd?.Invoke();
        }
        DialogueRoutine = StartCoroutine(StartDialogueRoutine());
    }

    IEnumerator StartDialogueRoutine()
    {
        string text = "";
        currentDialogueIndex = Mathf.Clamp(currentDialogueIndex,0, DialogueBlocks.Length-1);
        DialogueBlocks[currentDialogueIndex].DialogueTxt.text = "";

        for (int i = 0; i < DialogueBlocks[currentDialogueIndex].ObjectsToEnable.Length; i++)
        {
            DialogueBlocks[currentDialogueIndex].ObjectsToEnable[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < DialogueBlocks[currentDialogueIndex].ObjectsToDisable.Length; i++)
        {
            DialogueBlocks[currentDialogueIndex].ObjectsToDisable[i].gameObject.SetActive(false);
        }

        DialogueBlocks[currentDialogueIndex].OnBlockStartEvent?.Invoke();
        for (int i = 0; i < DialogueBlocks[currentDialogueIndex].dialogue.Length; i++)
        {
            text += DialogueBlocks[currentDialogueIndex].dialogue[i];
            DialogueBlocks[currentDialogueIndex].DialogueTxt.text = text;
           yield return new WaitForSeconds(0.03f);
        }

        yield return null;
    }


    // Start is called before the first frame update
    void Start()
    {
        if(ContinueButton)
        ContinueButton.onClick.AddListener(StartDialogue);
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
