using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GeneralScript : MonoBehaviour
{
    [Header("General Events")]
    [SerializeField] float onEnableWait = 0;
    [SerializeField] UnityEvent onEnableEvents;
    [SerializeField] float onDisableWait = 0;
    [SerializeField] public UnityEvent onDisableEvents;


    [Header("Looped Sequence")]
    [SerializeField] bool executeLoopedSequence = false;
    [SerializeField] float sequenceWait = 0.2f;
    [SerializeField] int sequenceCycle = 1;
    [SerializeField] int sequenceStartDelay = 3;
    [SerializeField] public UnityEvent sequenceEvents;
    // Start is called before the first frame update


    private void OnEnable()
    {
        Invoke(nameof(OnEnableEvent), onEnableWait);
    }

    void OnEnableEvent()
    {
        if (onEnableEvents != null)
            onEnableEvents.Invoke();

        if(executeLoopedSequence && sequenceEvents != null)
        {
            StartCoroutine(nameof(StartLoopedsequence));
        }
    }

    void OnDisableEvent()
    {
        if (onDisableEvents != null)
            onDisableEvents.Invoke();
    }

    private void OnDisable()
    {
        Invoke(nameof(OnDisableEvent), onDisableWait);
    }


    IEnumerator StartLoopedsequence()
    {
        yield return null;
        int currentSequence = 0;

        while (this.isActiveAndEnabled)
        {
            while(currentSequence <= sequenceCycle)
            {
                sequenceEvents?.Invoke();
                
                yield return new WaitForSeconds(sequenceWait);

                ++currentSequence;
            }


            yield return new WaitForSeconds(sequenceStartDelay);
            currentSequence = 0;
        }

    }
}
