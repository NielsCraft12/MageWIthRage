using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MemoryUse : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private MemoryText _memoryText;
    public List<Memory> memoryList;

    [SerializeField]
    PlayerInput _playerInput;

    private void Awake()
    {
        memoryList = new List<Memory>();

        _playerInput = GetComponent<PlayerInput>();
        // _playerInput.actions["Interact"].performed += ctx => UseNewestMemory();
    }

    public void UseNewestMemory(InputAction.CallbackContext context)
    {
        if (memoryList.Count > 0)
        {
            Memory memory = memoryList[0];
            string text = memory.UseMemory();
            _memoryText.Display(text);
            memoryList.RemoveAt(0);
        }
    }
}
