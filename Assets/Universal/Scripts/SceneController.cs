using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadScene(string _sceneName) => SceneManager.LoadScene(_sceneName);

    public void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public void LoadTitle() => LoadScene("Title");

    public void Quit() => Application.Quit();
}
