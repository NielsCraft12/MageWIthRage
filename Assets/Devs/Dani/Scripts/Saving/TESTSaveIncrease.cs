using UnityEngine;

public class TESTSaveIncrease : MonoBehaviour
{
    public void IncreaseLevel()
    {
        LevelManager.instance.currentLevel++;
    }
}
