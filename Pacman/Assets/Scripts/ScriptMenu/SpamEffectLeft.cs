using System.Collections;
using UnityEngine;

public class SpamEffectLeft : MonoBehaviour
{
    
    public GameObject objectToSpawn;
    public float speed = 10.0f;
    public float spawnInterval = 5.0f;

    IEnumerator SpawnObjectCoroutine() {
        while (true) {
            objectToSpawn.SetActive(true);
            GameObject newObj = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            Rigidbody2D rigidBody = newObj.GetComponent<Rigidbody2D>();
            rigidBody.velocity = Vector2.right * speed;

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void Start() {
        StartCoroutine(SpawnObjectCoroutine());
    }
}
