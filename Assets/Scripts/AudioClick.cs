using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClick : MonoBehaviour
{
    [SerializeField] AudioSource sound;

    public void Click(){
        sound.pitch = 1 + Random.Range(-0.3f, 0.3f);
        sound.Play();
    }
}
