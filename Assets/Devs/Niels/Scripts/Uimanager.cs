using UnityEngine;
using UnityEngine.UI;

public class Uimanager : MonoBehaviour
{
    [SerializeField]
    GameObject invertory1;

    [SerializeField]
    GameObject invertory2;

    [SerializeField]
    GameObject invertory3;

    [SerializeField]
    Slider healthSlider;

    [SerializeField]
    PlayerActionsnput playerActionsnput;

    [SerializeField]
    Health playerHealth;

    void Update()
    {
        if (LevelManager.instance != null && LevelManager.instance.abilitiesUnlocked < 3)
        {
            invertory1.SetActive(true);
            invertory2.SetActive(false);
            invertory3.SetActive(false);
        }
        else if (LevelManager.instance != null && LevelManager.instance.abilitiesUnlocked >= 3)
        {
            if (playerActionsnput.attackActive == 0)
            {
                invertory1.SetActive(false);
                invertory2.SetActive(true);
                invertory3.SetActive(false);
            }
            else if (playerActionsnput.attackActive == 1)
            {
                invertory1.SetActive(false);
                invertory2.SetActive(false);
                invertory3.SetActive(true);
            }
        }
        healthSlider.value = playerHealth.CurrentHealth / playerHealth.MaxHealth;
    }
}
