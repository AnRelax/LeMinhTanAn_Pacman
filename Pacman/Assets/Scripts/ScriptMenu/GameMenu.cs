using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject Notification;
    public GameObject MenuGame;
    public GameObject OptionGame;
    private AudioSource audioSource;
    public AudioClip audioClip;
    private void Start() {
        MenuGame.SetActive(true);
        Notification.SetActive(false);
        OptionGame.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.playOnAwake = true;
        audioSource.loop = true;
        audioSource.Play();
        audioSource.volume = 0.5f;
        PlayerPrefs.SetFloat("checkVolume",audioSource.volume);
    }
    public void ContinueGame(){
        if(SaveLoadGame.checkLoad){
            PlayerPrefs.SetString("CheckButton","Continue");
            SceneManager.LoadScene(1);
        }else{
            MenuGame.SetActive(false);
            Notification.SetActive(true);
        }
    }
    public void StartGame(){
        PlayerPrefs.SetString("CheckButton","Start");
        SceneManager.LoadScene(1);
    }
    public void OptionGames(){
        MenuGame.SetActive(false);
        OptionGame.SetActive(true);
    }
    public void Quit(){
        Application.Quit();
    }
    public void ClickOkNotification(){
        Notification.SetActive(false);
        MenuGame.SetActive(true);
    }
    public void ClickOkOption(){
        OptionGame.SetActive(false);
        MenuGame.SetActive(true);
    }
}
