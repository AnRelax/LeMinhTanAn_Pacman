using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveLoadGame : MonoBehaviour
{
    public Transform pacman;
    public Transform pellets;
    public Ghost[] ghosts;
    public Movement[] movement;
    public GhostHome[] ghostHomes;
    public Text score;

    public static bool checkLoad = false;
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){
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
        PlayerPrefs.SetFloat("SpeedGhost",Movement.speed);

        //ghost
        for (int i = 0; i < ghosts.Length; i++) {
            //save vitri
            Transform ghost = ghosts[i].transform;
            PlayerPrefs.SetFloat("Ghost_" + i + "_x", ghost.position.x);
            PlayerPrefs.SetFloat("Ghost_" + i + "_y", ghost.position.y);
            //save huong di chuyen
            Vector2 direction = movement[i].directionSaveLoad;
            PlayerPrefs.SetFloat("Ghost_" + i + "_direction_x", direction.x);
            PlayerPrefs.SetFloat("Ghost_" + i + "_direction_y", direction.y);
            //save kiem tra trong home
            PlayerPrefs.SetInt("checkGhostHome"+i,ghostHomes[i].checkGhostHome);
            
        }

        PlayerPrefs.SetInt("Score", int.Parse(score.text));
        PlayerPrefs.SetInt("lives",FindObjectOfType<GameManager>().lives);
        PlayerPrefs.SetString("CheckButton","Start");
        PlayerPrefs.SetInt("checkPowerPellet",GameManager.checkPowerPellet);
        SceneManager.LoadScene(0);
        checkLoad = true;
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
        Movement.speed = PlayerPrefs.GetFloat("SpeedGhost");

        //ghost
        for (int i = 0; i < ghosts.Length; i++) {
            float x = PlayerPrefs.GetFloat("Ghost_" + i + "_x");
            float y = PlayerPrefs.GetFloat("Ghost_" + i + "_y");
            ghosts[i].transform.position = new Vector3(x, y, -1.0f);
            float directionX = PlayerPrefs.GetFloat("Ghost_" + i + "_direction_x");
            float directionY = PlayerPrefs.GetFloat("Ghost_" + i + "_direction_y");
            Vector2 direction = new Vector2(directionX, directionY);

            //10 tieng fix loi huong di chuyen
            movement[i].SetDirection(direction,true);

            //1 tieng fix kiem tra ghost trong home hay da ra khoi home
            ghostHomes[i].checkGhostHome = PlayerPrefs.GetInt("checkGhostHome"+i);
            int checkGhostPowerPelletEaten = PlayerPrefs.GetInt("checkGhostPowerPelletEaten");
            //1.5 tieng fix ghost so hai
            if( checkGhostPowerPelletEaten == 1){
                ghosts[i].frightened.Enabled(PlayerPrefs.GetFloat("durationGhostBehavior"+i));
            }
            
        }

        FindObjectOfType<GameManager>().SetScore(PlayerPrefs.GetInt("Score"));
        score.text = PlayerPrefs.GetInt("Score").ToString();
        FindObjectOfType<GameManager>().SetLives(PlayerPrefs.GetInt("lives"));

        GameManager.checkPowerPellet = PlayerPrefs.GetInt("checkPowerPellet");
    }
}
