using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public bool open = false;
    public AudioClip sound;
    public AudioSource source;
       
    public void ChangeState() {
        open = !open;

        GetComponent<Collider>().isTrigger = open;
        
        if (open) {
            transform.Translate(new Vector3(0.45f, 0, 0.45f), Space.Self);
            transform.Rotate(new Vector3(0, -90, 0), Space.Self);
        }
        else {
            transform.Rotate(new Vector3(0, 90, 0), Space.Self);
            transform.Translate(new Vector3(-0.45f, 0, -0.45f), Space.Self);
        }

        source.clip = sound;
        source.Play();
    }
}
