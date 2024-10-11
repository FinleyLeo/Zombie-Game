using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject zombie;

    public TextMeshProUGUI waveCounter;

    public float xRange;
    public float minYRange;
    public float maxYRange;

    public int zombieCount;
    public int wave = 3;
    private int waveCount;

    // Start is called before the first frame update
    void Start()
    {
        waveCount = wave - 2;
        waveCounter.text = waveCount.ToString();
        SpawnWave(wave);
    }

    // Update is called once per frame
    void Update()
    {
        zombieCount = FindObjectsOfType<EnemyScript>().Length;

        if (zombieCount == 0)
        {
            wave++;
            waveCount++;
            SpawnWave(wave);
        }
    }

    void SpawnWave(int zombiesToSpawn)
    {
        for (int i = 0; i < zombiesToSpawn; i++)
        {
            Instantiate(zombie, GenerateSpawnPos(), Quaternion.identity);
        }

        waveCounter.text = waveCount.ToString();
    }

    Vector3 GenerateSpawnPos()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-xRange, xRange), Random.Range(minYRange, maxYRange), 0);

        return spawnPos;
    }
    
}
