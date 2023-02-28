using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{

    [SerializeField] [CanBeNull] private GameObject normalCanvas;
    [SerializeField] [CanBeNull] private GameObject optionCanvas;

    private void Awake()
    {
        if (normalCanvas != null) normalCanvas.SetActive(true);
        if (optionCanvas != null) optionCanvas.SetActive(false);
    }

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
