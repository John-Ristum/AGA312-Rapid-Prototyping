using UnityEngine;
using UnityEngine.UI;

public class TitleButton : MonoBehaviour
{
    public Image splashImage;
    public Sprite mySplashImage;

    public void OnMouseEnter()
    {
        splashImage.sprite = mySplashImage;
        splashImage.enabled = true;
    }

    public void OnMouseExit()
    {
        splashImage.enabled = false;
    }
}
