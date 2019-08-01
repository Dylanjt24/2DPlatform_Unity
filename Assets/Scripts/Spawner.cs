using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy[] enemies;
    [SerializeField] private float spawnTimeDelay, startSpawnDelay;
    public bool completed;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(startSpawnDelay);
        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemyInstance = Instantiate(enemies[i], transform.position, Quaternion.identity); // Instantiate next enemy in the list at spawner's location with no rotation
            enemyInstance.Move(transform.right); // Moves the enemy in the spawner's right direction
            completed = i >= enemies.Length - 1;
            yield return new WaitForSeconds(spawnTimeDelay);
        }
    }
}
