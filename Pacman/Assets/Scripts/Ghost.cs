using UnityEngine;

[DefaultExecutionOrder(-10)]
[RequireComponent(typeof(Movement))]
public class Ghost : MonoBehaviour
{
    public Movement movement {get; set;}
    public GhostHome home {get; private set;}
    public GhostScatter scatter {get; private set;}
    public GhostChase chase {get; private set;}
    public GhostFrightened frightened {get; private set;}
    public GhostBehavior initialBehavior;
    public Transform target;
    public int points = 50;
    private void Awake() {
        this.movement = GetComponent<Movement>();
        this.home = GetComponent<GhostHome>();
        this.scatter = GetComponent<GhostScatter>();
        this.chase = GetComponent<GhostChase>();
        this.frightened = GetComponent<GhostFrightened>();
    }
    private void Start() {
        ResetState();
    }
    
    public void ResetState(){
        this.gameObject.SetActive(true);
        this.movement.ResetState();
        this.frightened.Disable();
        this.chase.Disable();
        this.scatter.Enabled();
        home.checkGhostHome = 0;

        if(this.home != this.initialBehavior){
            this.home.Disable();
        }
        if(this.initialBehavior != null){
            this.initialBehavior.Enabled();
        }
    }
    public void SetPosition(Vector3 position)
    {
        position.z = transform.position.z;
        transform.position = position;
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Pacman")){
            if(this.frightened.enabled){
                FindObjectOfType<GameManager>().GhostEaten(this);
            }else{
                FindObjectOfType<GameManager>().PacmanEaten();
            }
        }
    }
}
