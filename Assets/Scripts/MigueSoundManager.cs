using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MigueSoundManager : MonoBehaviour
{

    public AudioClip[] nurseClip;
    public AudioClip[] patientClip;
    public AudioClip dialogueStart;
    public AudioSource nurseSource;
    public AudioSource patientSource;
    public AudioSource waterSource;
    public AudioClip uiSound;
    // Start is called before the first frame update

    public void SoundTouchUI()
    {
        nurseSource.PlayOneShot(uiSound);
    }

    public void DialogueStart()
    {
        nurseSource.clip = dialogueStart;
        nurseSource.Play();
    }
}
