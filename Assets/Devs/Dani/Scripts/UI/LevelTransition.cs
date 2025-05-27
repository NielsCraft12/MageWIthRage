using UnityEngine;
using UnityEngine.UI;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private Image _image;

    [Range(0, 5)][SerializeField] private float _fadeSpeed;
    [SerializeField] private bool _fadingIn;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void FadeIn()
    {
        _fadingIn = true;
    }
    public void FadeOut()
    {
        _fadingIn = false;
    }

    private void Update()
    {
        if (_fadingIn && _image.color.a < 1)

            _image.color += new Color(0, 0, 0, _fadeSpeed * Time.unscaledDeltaTime);
        else if (_image.color.a > 1)
            _image.color = new Color(0, 0, 0, 1);
        else if (!_fadingIn && _image.color.a > 0)
            _image.color -= new Color(0, 0, 0, _fadeSpeed * Time.unscaledDeltaTime);
        else if (_image.color.a < 0)
            _image.color = new Color(0, 0, 0, 0);
    }
}
