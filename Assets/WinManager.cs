using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    [SerializeField] GameObject winCanvas;

    void Start()
    {
        if (winCanvas != null)
            winCanvas.SetActive(false);
    }

    void Update()
    {
        if (DoorArrive.win)
        {
            if (winCanvas != null)
                winCanvas.SetActive(true);
            Time.timeScale = 0f; // pause game (optional)
        }
    }

    // Public method for UI Restart button (no parameters)
    public void Restart()
    {
        Time.timeScale = 1f; // resume time
        if (winCanvas != null)
            winCanvas.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
