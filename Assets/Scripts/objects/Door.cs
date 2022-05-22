using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private const string _DoorSoundPath = "Sounds/door";

    public bool Open { get; private set; }
    private AudioClip sound;
    private AudioSource source;

    void Start() {
        sound = Utils.LoadObject<AudioClip>(_DoorSoundPath);
        source = GetComponent<AudioSource>();
    }

    public void Interact() {
        Open = !Open;

        GetComponent<Collider>().isTrigger = Open;
        
        if (Open) {
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
