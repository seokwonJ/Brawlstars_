using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JEnemySpawner : MonoBehaviour
{
    public GameObject[] enemies;         // 적 프리팹
    public Transform[] spawnPoints;
    public int spawnTime=5;
    public int enemiesPerWave = 3;  // 한 번에 생성할 적의 수
    public bool start = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // 현재 게임중이 아니라면 함수를 나가자.
        if (GameManager.instance.isPlaying == false) return;
        if (!start)
        {
            StartCoroutine(SpawnEnemies());
            start = true;
        }
        
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {

            for (int i = 0; i < enemiesPerWave; i++)
            {
                int randomPointIndex = Random.Range(0, spawnPoints.Length);
                int randomEnemiesIndex = Random.Range(0, enemies.Length);
                Instantiate(enemies[randomEnemiesIndex], spawnPoints[randomPointIndex].position, Quaternion.identity, spawnPoints[randomPointIndex]);
            }
            yield return new WaitForSeconds(spawnTime);
        }
    }
}
