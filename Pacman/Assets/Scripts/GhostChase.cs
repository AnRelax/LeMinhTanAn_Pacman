using UnityEngine;

public class GhostChase : GhostBehavior
{
    private void OnDisable() {
        this.ghost.scatter.Enabled();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        Node node = other.GetComponent<Node>();
        if(node != null && this.enabled && !this.ghost.frightened.enabled){
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;
            foreach(Vector2 availableDirection in node.availableDirection){
                Vector3 newPosition = this.transform.position + new Vector3(availableDirection.x, availableDirection.y, 0.0f);
                float distance = (this.ghost.target.position - newPosition).magnitude;
                if(distance < minDistance){
                    direction = availableDirection;
                    minDistance = distance;
                }
            }
            this.ghost.movement.SetDirection(direction);
        }
    }
}