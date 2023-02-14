using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flickering : MonoBehaviour
{

    private float minInterval = 0.1f;
    private float maxInterval = 0.9f;

    [SerializeField] private GameObject flickerText;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        while (true)
        {
            flickerText.SetActive(false);
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
            flickerText.SetActive(true);
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
        }
        // ReSharper disable once IteratorNeverReturns
    }
}
