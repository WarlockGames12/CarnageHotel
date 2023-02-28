using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class LevelLoader : MonoBehaviour
    {
        [Header("Transition Settings: ")]
        public Animator transition;
        public int transitionTime = 2;

        public void LoadNextLevel()
        {
            StartCoroutine(LoadLevel(1));
        }

        private IEnumerator LoadLevel(int levelIndex)
        {
            transition.SetTrigger("Start");
            yield return new WaitForSeconds(transitionTime);
            SceneManager.LoadScene(levelIndex);
        }
    }
}
