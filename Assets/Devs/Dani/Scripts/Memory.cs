using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Memory : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private MemoryUse _memoryUse;
    [SerializeField] private GameObject _memoryIcon;
    [Header("Settings")]
    [SerializeField] private string _memoryText;
    [Tooltip("Up to what abilities this memory unlocks")][SerializeField] private int _unlocksAbility;

    public void InRange()
    {
        _memoryIcon.SetActive(true);
        _memoryUse.memoryList.Add(this);
    }

    public void OutOfRange()
    {
        _memoryIcon.SetActive(false);
        _memoryUse.memoryList.Remove(this);
    }

    public string UseMemory()
    {
        _memoryIcon.SetActive(false);
        //Unlock abilities
        if (_unlocksAbility > LevelManager.instance.abilitiesUnlocked)
        {
            LevelManager.instance.abilitiesUnlocked = _unlocksAbility;
            Debug.Log("Abilities unlocked: " + _unlocksAbility);
        }
        Destroy(gameObject);
        return _memoryText;
    }
}
