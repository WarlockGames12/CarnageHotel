using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class VolumeScripts : MonoBehaviour
    {

        // Audio Settings: 
        private readonly List<AudioSource> _sliderAudio = new List<AudioSource>();
        private Slider _audioSlider;

        // Start is called before the first frame update
        private void Awake()
        {
            // Get every AudioSource from scene
            var allAudio = FindObjectsOfType<AudioSource>();
            foreach (var audioSources in allAudio)
                _sliderAudio.Add(audioSources);

            _audioSlider = GetComponent<Slider>();

            var savedValue = PlayerPrefs.GetFloat("AudioVolume", 1f);
            _audioSlider.value = savedValue;

            foreach (var audioSource in _sliderAudio)
                audioSource.volume = _audioSlider.value;
        }

        public void OnSliderValueChanged()
        {
            foreach (var audioSource in _sliderAudio)
                audioSource.volume = _audioSlider.value;
            
            
            PlayerPrefs.SetFloat("AudioVolume", _audioSlider.value);
        }
    }
}
