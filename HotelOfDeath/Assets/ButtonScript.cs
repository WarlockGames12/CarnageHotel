using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{

    public void GoToScene(string buttonString)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(buttonString);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
