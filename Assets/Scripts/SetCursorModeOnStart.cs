using UnityEngine;

public class SetCursorModeOnStart : MonoBehaviour
{
    [SerializeField] private bool visible = true;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = visible;        
    }
}
