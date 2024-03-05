using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundControl : MonoBehaviour
{
    AudioSource myAudioSource; //drag into scene in editor
    public bool isSoundMuted;

    public bool muteChanged = false; //false to start

    // Start is called before the first frame update
    void Start()
    {
        //find audiosource gameobject
        myAudioSource = GetComponent<AudioSource>();

        isSoundMuted = false; //get mute bool from source

        //if sound is not muted, keep playing
        /*
        if (!isSoundMuted)
        {
            //do nothing because sound should already be started
        }
        else (isSoundMuted = true) {
            myAudioSource.Stop(); //stop music from playing
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Box toggled button ========================================================         B   4
    public void muteToggled()
    {
        Debug.Log("Toggled...");
        //get mute bools
        muteChanged = true; //just toggled
        //isSoundMuted = false;
        //if sound is not muted, but they mute it, then we need to stop playing the sound
        if (muteChanged && !isSoundMuted)
        {
            myAudioSource.Pause(); //stop music from playing
            isSoundMuted = true;
        }
        else if (muteChanged && isSoundMuted)
        { //if sound IS muted, but they UN mute it, then we need to start playing the sound
            myAudioSource.UnPause(); //start music 
            isSoundMuted = false;
        }


        //swap mutechanged AFTER each time toggled
        muteChanged = !muteChanged;

    }
}
