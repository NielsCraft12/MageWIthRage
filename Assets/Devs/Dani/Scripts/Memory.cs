using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    [Header("Dependencies")]
    // [SerializeField] private Player player;
    [SerializeField] private GameObject memoryIcon;
    [Header("Settings")]
    [SerializeField] private string memoryText;

    public void InRange()
    {
        memoryIcon.SetActive(true);
    }

    public void OutOfRange()
    {
        memoryIcon.SetActive(false);
    }
}
