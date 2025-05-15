using UnityEngine;

public enum AlertState
{
    Idle,
    ToOrigin,
    ToPlayer,
}

public class Enemy : MonoBehaviour
{
    public AlertState alertState;
}
