using System.Collections;
using UnityEngine;

public class SpamEffectRight : MonoBehaviour
{
    
    public GameObject objectToSpawn;
    public float speed = 10.0f;
    public float spawnInterval = 5.0f;

    IEnumerator SpawnObjectCoroutine() {
        while (true) {
            yield return new WaitForSeconds(spawnInterval);
            objectToSpawn.SetActive(true);
            GameObject newObj = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            Rigidbody2D rigidBody = newObj.GetComponent<Rigidbody2D>();
            rigidBody.velocity = Vector2.left * speed;
            spawnInterval = 23.0f;

        }
    }

    void Start() {
        StartCoroutine(SpawnObjectCoroutine());
    }
}
