using UnityEngine;

public class MemoryTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Memory>() != null)
            other.GetComponent<Memory>().InRange();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Memory>() != null)
            other.GetComponent<Memory>().OutOfRange();
    }
}
