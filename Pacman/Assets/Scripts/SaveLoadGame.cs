using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveLoadGame : MonoBehaviour
{
    public Transform pacman;
    public Transform pellets;
    public Transform []ghosts;
    public Text score;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Q)){
            luu();
        }
        if(Input.GetKeyDown(KeyCode.B)){
            tai();
        }
    }
    public void luu(){
        for (int i = 0; i < pellets.childCount; i++) {
            Transform pellet = pellets.GetChild(i);
            int isActive = pellet.gameObject.activeSelf ? 1 : 0;
            PlayerPrefs.SetInt("Pellet_" + i, isActive);
            PlayerPrefs.SetFloat("Pellet_" + i + "_x", pellet.position.x);
            PlayerPrefs.SetFloat("Pellet_" + i + "_y", pellet.position.y);
        }

        PlayerPrefs.SetFloat("Pacman_x", pacman.position.x);
        PlayerPrefs.SetFloat("Pacman_y", pacman.position.y);

        for (int i = 0; i < ghosts.Length; i++) {
            Transform ghost = ghosts[i];
            PlayerPrefs.SetFloat("Ghost_" + i + "_x", ghost.position.x);
            PlayerPrefs.SetFloat("Ghost_" + i + "_y", ghost.position.y);
        }

        PlayerPrefs.SetInt("Score", int.Parse(score.text));
        PlayerPrefs.SetInt("lives",FindObjectOfType<GameManager>().lives);
        PlayerPrefs.SetString("CheckButton","Start");
        SceneManager.LoadScene(0);
    }
    public void tai(){
        for (int i = 0; i < pellets.childCount; i++) {
            int isActive = PlayerPrefs.GetInt("Pellet_" + i, 1);
            if (isActive == 0) {
            pellets.GetChild(i).gameObject.SetActive(false);
        } else {
            pellets.GetChild(i).gameObject.SetActive(true);
            float x = PlayerPrefs.GetFloat("Pellet_" + i + "_x");
            float y = PlayerPrefs.GetFloat("Pellet_" + i + "_y");
            pellets.GetChild(i).position = new Vector3(x, y, 0);
        }
        }

        float px = PlayerPrefs.GetFloat("Pacman_x");
        float py = PlayerPrefs.GetFloat("Pacman_y");
        pacman.position = new Vector3(px, py, -4);
        
        for (int i = 0; i < ghosts.Length; i++) {
            float x = PlayerPrefs.GetFloat("Ghost_" + i + "_x");
            float y = PlayerPrefs.GetFloat("Ghost_" + i + "_y");
            ghosts[i].position = new Vector3(x, y, -1.0f);
        }
        
        FindObjectOfType<GameManager>().SetScore(PlayerPrefs.GetInt("Score"));
        score.text = PlayerPrefs.GetInt("Score").ToString();
        FindObjectOfType<GameManager>().SetLives(PlayerPrefs.GetInt("lives"));
    }
}
