using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private float wavePeriod;
    private float elapsedTime;
    [SerializeField]
    private GameObject enemyPrefab;
    private Transform player;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        wavePeriod = Random.Range(7, 15);
    }

    private void Update()
    {
        if(elapsedTime < wavePeriod)
        {
            elapsedTime += Time.deltaTime;
        }
        else
        {
            for(int i = 0; i < Random.Range(3,5); i++)
            {
                Spawn();
            }
            
            elapsedTime = 0;
            wavePeriod = Random.Range(7, 15);
        }
    }

    private void Spawn()
    {
        Instantiate(enemyPrefab, GetSpawnPoint(), Quaternion.identity);
    }

    private Vector3 GetSpawnPoint()
    {
        return player.position + new Vector3(Random.Range(-15, 15), Random.Range(0, 5), Random.Range(-15, 15));
    }
}
