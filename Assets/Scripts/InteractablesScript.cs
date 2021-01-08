using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractablesScript : MonoBehaviour
{
    public void Interact(IInteractable obj) {
        obj.OnInteract();
    }
}