using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCollection : MonoBehaviour
{
    public List<Selectable> buttons;

    public void InteractableToggle()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].interactable = !buttons[i].interactable;
        }
    }
}
