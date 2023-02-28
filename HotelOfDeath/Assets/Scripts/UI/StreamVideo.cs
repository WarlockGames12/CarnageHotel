using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace UI
{
    public class StreamVideo : MonoBehaviour
    {

        [Header("Video Settings: ")] 
        [SerializeField] private RawImage videoImage;
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private LevelLoader fade;
    
        // Start is called before the first frame update
        private void Start()
        {
            StartCoroutine(PlayVideo());
        }

        private IEnumerator PlayVideo()
        {
            videoPlayer.Prepare();
            var waitForSeconds = new WaitForSeconds(1);
            while (!videoPlayer.isPrepared)
            {
                yield return waitForSeconds;
                break;
            }

            videoImage.texture = videoPlayer.texture;
            videoPlayer.Play();
            audioSource.Play();
            while (videoPlayer.isPlaying)
            {
                yield return new WaitForEndOfFrame();
            }
            fade.LoadNextLevel();
        }
    }
}
