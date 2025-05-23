using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BreakableWall : MonoBehaviour
{
    public UnityEvent onBreak;

    public void Break()
    {
        onBreak.Invoke();
        BreakWall();
    }

    private IEnumerator BreakWall()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
