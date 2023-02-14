using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{

    public void GoToScene(string buttonString)
    {
        Debug.Log("Is This button Working?"); 
        SceneManager.LoadScene(buttonString);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
