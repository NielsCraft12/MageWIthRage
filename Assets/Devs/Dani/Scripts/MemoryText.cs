using System.Collections;
using TMPro;
using UnityEngine;

public class MemoryText : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private GameObject _background;

    [SerializeField]
    private TextMeshProUGUI _textField;
    private Coroutine _fadeCoroutine;

    public void Display(string memoryText)
    {
        _background.SetActive(true);
        _textField.text = memoryText;
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(2f);
        _background.SetActive(false);
    }
}
