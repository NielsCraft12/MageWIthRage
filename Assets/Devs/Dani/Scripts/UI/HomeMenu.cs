using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HomeMenu : MonoBehaviour
{
    public static HomeMenu instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        screens[0].firstButton.Select();
    }

    public Screens[] screens;

    [System.Serializable]
    public class Screens
    {
        public RectTransform rectTransform;
        public ButtonCollection buttonCollection;
        public int ShowPosY;
        public int RestPosY;
        public Selectable firstButton;
    }

    public void LoadScreen(int partOfScreen, int requestedScreen)
    {
        Screens calledScreen = screens[requestedScreen];
        if (calledScreen.rectTransform.position.y != calledScreen.ShowPosY)
        {
            calledScreen.buttonCollection.InteractableToggle();
            calledScreen.rectTransform.transform.DOLocalMoveY(calledScreen.ShowPosY, 0.75f);
            calledScreen.firstButton.Select();
        }

        Screens screen = screens[partOfScreen];
        if (screen.rectTransform.position.y != screen.RestPosY)
        {
            screen.buttonCollection.InteractableToggle();
            screen.rectTransform.transform.DOLocalMoveY(screen.RestPosY, 0.75f);
        }

    }
}


