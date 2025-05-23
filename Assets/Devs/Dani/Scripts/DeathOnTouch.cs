using UnityEngine;

public class DeathOnTouch : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            Vector3 newPos = GameManager.instance.checkpoints[GameManager.instance.currentCheckpoint - 1].position;
            // Player.transform.position = newPos;
        }
    }
}
