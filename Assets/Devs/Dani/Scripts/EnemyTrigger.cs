using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>() != null)
            other.GetComponent<Enemy>().alertState = AlertState.ToPlayer;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Enemy>() != null)
            other.GetComponent<Enemy>().alertState = AlertState.ToOrigin;
    }
}
