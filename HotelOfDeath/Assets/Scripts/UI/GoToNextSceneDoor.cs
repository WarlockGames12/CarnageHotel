using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToNextSceneDoor : MonoBehaviour
{

    [Header("Go to black and then next scene")] 
    [SerializeField] private AudioSource getIn;
    [SerializeField] private GameObject pitchBlack;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && PlayerIsClose.isInRange)
        {
            StartCoroutine(IsGettingIn());
        }
    }
    
    // Start is called before the first frame update
    private IEnumerator IsGettingIn()
    {
        pitchBlack.SetActive(true);
        getIn.Play();
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Scenes/Hotel");
    }
}
