using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    public bool isOpen = false;

    public UnityEvent onOpen;

    public void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            onOpen.Invoke();
        }
    }
}
