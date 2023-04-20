using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public void ContinueGame(){
        PlayerPrefs.SetString("CheckButton","Continue");
        SceneManager.LoadScene(1);
    }
    public void StartGame(){
        PlayerPrefs.SetString("CheckButton","Start");
        SceneManager.LoadScene(1);
    }
    public void Quit(){
        Application.Quit();
    }
}
