using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;

    public GameObject gameOverText;
    public GameObject gameWinText;
    public Text scoreText;
    public Text livesText;
    public Text highScoreText;
    public GameObject up1;
    public GameObject up2;
    public GameObject up3;

    private AudioSource audioSource;
    public AudioClip startAudio;
    public AudioClip nenAudio;
    public AudioClip eatenPelletAudio;
    public AudioClip eatenPowerPelletAudio;
    public AudioClip eatenGhostAudio;
    public AudioClip eatenPacmanAudio;
    public AudioClip pacmanWinAudio;
    public AudioClip pacmanLoseAudio;

    private bool checkPacmanEatenAudio;
    private bool checkPlayPacmanEatenAudio;
    private string checkSaveLoad;
    public int ghostMultiplier{get; private set;} =1;

    public int score{
        get;
        private set;}
    public int lives{
        get;
        private set;}

    private void Start(){
        checkSaveLoad = PlayerPrefs.GetString("CheckButton");
        if(checkSaveLoad.Equals("Continue")){
            NewGame();
            FindObjectOfType<SaveLoadGame>().tai();
            
            if(lives == 3){
                up1.SetActive(true);
                up2.SetActive(true);
                up3.SetActive(true);
            }else if(lives == 2){
                up1.SetActive(true);
                up2.SetActive(true);
                up3.SetActive(false);
            } else if(lives == 1){
                up1.SetActive(true);
                up2.SetActive(false);
                up3.SetActive(false);
            }else if(lives == 0){
                up1.SetActive(false);
                up2.SetActive(false);
                up3.SetActive(false);
            }
            audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(startAudio);
            StartCoroutine(PlayNenAudioAfterStartAudio());

            checkPacmanEatenAudio = false;
            checkPlayPacmanEatenAudio = false;
        } else{
            NewGame();
            up1.SetActive(true);
            up2.SetActive(true);
            up3.SetActive(true);

            audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(startAudio);
            StartCoroutine(PlayNenAudioAfterStartAudio());

            checkPacmanEatenAudio = false;
            checkPlayPacmanEatenAudio = false;
        }
        
    }
    IEnumerator PlayNenAudioAfterStartAudio()
    {
        yield return new WaitForSeconds(startAudio.length);
        audioSource.clip = nenAudio;
        audioSource.playOnAwake = true;
        audioSource.loop = true;
        audioSource.Play();
        
    }
    private void Update() {
        if(this.lives<=0 && Input.anyKeyDown){
            NewGame();
        }
        if (!HasRemainingPellets() && Input.anyKeyDown)
        {
            NewRound();
        }
        
    }
    
    private void NewGame(){
        SetScore(0);
        SetLives(3);
        NewRound();
    }
    private void NewRound(){
        gameOverText.SetActive(false);
        gameWinText.SetActive(false);
        foreach(Transform pellet in this.pellets){
            pellet.gameObject.SetActive(true);
            
        }
        ResetState();
    }
    private void ResetState(){
        ResetGhostMultiplier();
        for(int i = 0; i < this.ghosts.Length; i++){
            this.ghosts[i].ResetState();
        }
        this.pacman.ResetState();
    }
    private void GameOver(){
        audioSource.clip = pacmanLoseAudio;
        audioSource.playOnAwake = true;
        audioSource.loop = false;
        audioSource.Play();

        gameOverText.SetActive(true);
        if(int.Parse(scoreText.text) > int.Parse(highScoreText.text)){
            highScoreText.text = scoreText.text;
            PlayerPrefs.SetInt("highScore", int.Parse(highScoreText.text));
        }
        for(int i = 0; i < this.ghosts.Length; i++){
            this.ghosts[i].gameObject.SetActive(false);
        }
        this.pacman.gameObject.SetActive(false);
    }
    public void SetScore(int score){
        this.score = score;
        scoreText.text = score.ToString().PadLeft(2, '0');
        highScoreText.text = PlayerPrefs.GetInt("highScore").ToString();
    }
    public void SetLives(int lives){
        this.lives = lives;
        livesText.text =lives.ToString() + "UP";

        if(lives == 3){
            up1.SetActive(true);
            up2.SetActive(true);
            up3.SetActive(true);
        }else if(lives == 2){
            up1.SetActive(true);
            up2.SetActive(true);
            up3.SetActive(false);
        } else if(lives == 1){
            up1.SetActive(true);
            up2.SetActive(false);
            up3.SetActive(false);
        }else if(lives == 0){
            up1.SetActive(false);
            up2.SetActive(false);
            up3.SetActive(false);
        }
    }
    public void GhostEaten(Ghost ghost){
        audioSource.PlayOneShot(eatenGhostAudio);

        int points = ghost.points * this.ghostMultiplier;
        SetScore(this.score + points);
        this.ghostMultiplier ++;
    }
    public void PacmanEaten(){
        this.pacman.gameObject.SetActive(false);
        audioSource.PlayOneShot(eatenPacmanAudio);
        SetLives(this.lives - 1);
        if(this.lives > 0){
            Invoke(nameof(ResetState), 3.0f);
        }else{
            GameOver();
        }
    }
    public void PelletEaten(Pellet pellet){
        pellet.gameObject.SetActive(false);
        SetScore(this.score + pellet.points);
        StartCoroutine(checkPlayPacManEatenAudio());

        if(!checkPacmanEatenAudio && checkPlayPacmanEatenAudio){
            StartCoroutine(PlayPacManEatenAudio());
        }
        
        if (!HasRemainingPellets())
        {
            this.pacman.gameObject.SetActive(false);
            audioSource.clip = pacmanWinAudio;
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.Play();
            //StartCoroutine(checkMute());
        }
    }
    private IEnumerator checkMute(){
        yield return new WaitForSeconds(pacmanWinAudio.length);
        audioSource.mute = true;
    }
    private IEnumerator checkPlayPacManEatenAudio(){
        yield return new WaitForSeconds(startAudio.length);
        checkPlayPacmanEatenAudio = true;
    }
    private IEnumerator PlayPacManEatenAudio()
    {
        checkPacmanEatenAudio = true;
        audioSource.PlayOneShot(eatenPelletAudio);
        yield return new WaitForSeconds(0.15f);
        checkPacmanEatenAudio = false;
    }
    private void nhacnen(){
        audioSource.clip = eatenPelletAudio;
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.Play();
    }
    public void PowerPelletEaten(PowerPellet pellet){
        audioSource.clip = eatenPowerPelletAudio;
        audioSource.playOnAwake= true;
        audioSource.loop = true;
        audioSource.Play();
        StartCoroutine(checkPlayPowerPelletAudio());

        for(int i=0; i < this.ghosts.Length; i++){
            this.ghosts[i].frightened.Enabled(pellet.duration);
        }

        PelletEaten(pellet);
        CancelInvoke();
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }
    private IEnumerator checkPlayPowerPelletAudio(){
        yield return new WaitForSeconds(8.0f);
        if(HasRemainingPellets())
        {
            audioSource.clip = nenAudio;
            audioSource.playOnAwake= true;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in this.pellets)
        {
            if (pellet.gameObject.activeSelf) {
                return true;
            }
        }
        for(int i = 0; i < this.ghosts.Length; i++){
            this.ghosts[i].gameObject.SetActive(false);
        }
        if(int.Parse(scoreText.text) > int.Parse(highScoreText.text)){
            highScoreText.text = scoreText.text;
            PlayerPrefs.SetInt("highScore", int.Parse(highScoreText.text));
        }
        Debug.Log("het pellet roi !");
        gameWinText.SetActive(true);

        return false;
    }
    private void ResetGhostMultiplier(){
        this.ghostMultiplier = 1;
    }
}
