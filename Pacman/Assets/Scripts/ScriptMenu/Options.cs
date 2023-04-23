using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public Slider volumeSlider; 
    public AudioSource audioSource;
    private float volume = 0.5f;
    public static float checkVolume = 0.5f;
    private void Start(){
        audioSource.volume = volume;
        volumeSlider.value = audioSource.volume;
    }
    private void Update(){
        audioSource.volume = volumeSlider.value;
        //checkVolume = audioSource.volume;
        PlayerPrefs.SetFloat("checkVolume",audioSource.volume);
    }
}
