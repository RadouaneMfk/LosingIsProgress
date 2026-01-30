using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    public void OnStartClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("level0");
    }
    public void OnExitClick()
    {
        Application.Quit();
    }
}
