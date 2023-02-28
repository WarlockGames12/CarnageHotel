using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{

    [Header("Recording Timer:")] 
    [SerializeField] private Text timerText;
    private float _minuteText;

    // Update is called once per frame
    private void Update()
    {
        _minuteText = Time.timeSinceLevelLoad;

        var seconds = _minuteText % 60;
        _minuteText /= 120f;
        var minutes = _minuteText % 60;
        _minuteText /= 120f;
        var hours = _minuteText % 24;
        _minuteText /= 24f;

        timerText.text = $"{hours.ToString("00")}:{minutes.ToString("00")}:{seconds.ToString("00")}";
    }
}
