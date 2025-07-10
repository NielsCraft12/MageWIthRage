using UnityEngine;

public class Buttons : MonoBehaviour
{
    [SerializeField]
    Canvas mainMenuCanvas;

    [SerializeField]
    Canvas creditsMenuCanvas;

    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
        Debug.Log(
            this.name
                + " : "
                + this.GetType()
                + " : "
                + System.Reflection.MethodBase.GetCurrentMethod().Name
        );
#endif
#if (UNITY_EDITOR)
        UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE)
        Application.Quit();
#elif (UNITY_WEBGL)
        Application.OpenURL("itch url ");
#endif
    }

    public void openCredits()
    {
        creditsMenuCanvas.enabled = true;
        mainMenuCanvas.enabled = false;
    }

    public void closeCredits()
    {
        creditsMenuCanvas.enabled = false;
        mainMenuCanvas.enabled = true;
    }
}
