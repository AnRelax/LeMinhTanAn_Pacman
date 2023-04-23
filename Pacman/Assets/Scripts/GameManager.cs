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
    public AudioClip nenAudio1;
    public AudioClip nenAudio2;
    public AudioClip nenAudio3;
    public AudioClip nenAudio4;
    public AudioClip nenAudio5;
    public AudioClip eatenPelletAudio;
    public AudioClip eatenPowerPelletAudio;
    public AudioClip eatenGhostAudio;
    public AudioClip eatenPacmanAudio;
    public AudioClip pacmanWinAudio;
    public AudioClip pacmanLoseAudio;

    private bool checkPacmanEatenAudio;
    private bool checkPlayPacmanEatenAudio;
    private string checkSaveLoad;
    public static int checkPowerPellet;
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
            StartCoroutine(LoadGame());
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
            audioSource.volume = PlayerPrefs.GetFloat("checkVolume");
            
        } else{
            NewGame();
            Time.timeScale = 0;
            checkPowerPellet = 0;
            up1.SetActive(true);
            up2.SetActive(true);
            up3.SetActive(true);

            audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(startAudio);
            StartCoroutine(PlayNenAudioAfterStartAudio());

            checkPacmanEatenAudio = false;
            checkPlayPacmanEatenAudio = false;
            audioSource.volume = PlayerPrefs.GetFloat("checkVolume");
        }
        
    }
    IEnumerator PlayNenAudioAfterStartAudio()
    {
        yield return new WaitForSeconds(startAudio.length);
        audioSource.clip = nenAudio1;
        audioSource.playOnAwake = true;
        audioSource.loop = true;
        audioSource.Play();
        
    }
    private IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(0.01f);// chú ý
        FindObjectOfType<SaveLoadGame>().tai();
        Time.timeScale = 0;
    }
    private void Update() {
        if(score > int.Parse(highScoreText.text)){
            highScoreText.text = score.ToString();
            PlayerPrefs.SetInt("highScore", int.Parse(highScoreText.text));
        }
        if(this.lives<=0 && Input.anyKeyDown){
            NewGame();
        }
        if (!HasRemainingPellets() && Input.anyKeyDown)
        {
            NewRound();
        }
        if(Input.anyKeyDown){
            Time.timeScale = 1;
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
        checkPowerPellet++;
        StartCoroutine(checkPlayPowerPelletAudio());

        Movement.speed +=0.3f;

        
        for(int i=0; i < this.ghosts.Length; i++){
            this.ghosts[i].frightened.Enabled(pellet.duration);
            PlayerPrefs.SetFloat("durationGhostBehavior"+i,pellet.duration);
        }
        PlayerPrefs.SetInt("checkGhostPowerPelletEaten",1);

        PelletEaten(pellet);
        CancelInvoke();
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }
    private IEnumerator checkPlayPowerPelletAudio(){
        yield return new WaitForSeconds(8.0f);
        PlayerPrefs.SetInt("checkGhostPowerPelletEaten",0);
        if(HasRemainingPellets())
        {
            if(checkPowerPellet == 0){
                audioSource.clip = nenAudio1;
                audioSource.playOnAwake= true;
                audioSource.loop = true;
                audioSource.Play();
            }else if(checkPowerPellet == 1){
                audioSource.clip = nenAudio2;
                audioSource.playOnAwake= true;
                audioSource.loop = true;
                audioSource.Play();
            }else if(checkPowerPellet == 2){
                audioSource.clip = nenAudio3;
                audioSource.playOnAwake= true;
                audioSource.loop = true;
                audioSource.Play();
            }else if(checkPowerPellet == 3){
                audioSource.clip = nenAudio4;
                audioSource.playOnAwake= true;
                audioSource.loop = true;
                audioSource.Play();
            }
            else if(checkPowerPellet >=4){
                audioSource.clip = nenAudio5;
                audioSource.playOnAwake= true;
                audioSource.loop = true;
                audioSource.Play();
            }
            
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
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("checkVolume");
    }
}
