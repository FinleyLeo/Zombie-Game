using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private GameObject player;
    public GameObject zombie;
    public GameObject waveTimer;
    private WaveTimer timerScript;

    public GameObject entranceBlock;

    public TextMeshProUGUI waveCounter;

    public float xRange;
    public float minYRange;
    public float maxYRange;

    public int zombieCount;
    public int wave = 2;
    private int waveCount;

    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();

        timerScript = waveTimer.GetComponent<WaveTimer>();

        waveCount = wave - 2;
        waveCounter.text = waveCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        zombieCount = FindObjectsOfType<EnemyScript>().Length;

        if (zombieCount == 0 && playerController.onGround)
        {
            if (waveCount % 5 != 0 || waveCount == 0)
            {
                entranceBlock.SetActive(true);
                wave++;
                waveCount++;
                SpawnWave(wave);
            }

            if (waveCount % 5 == 0 && waveCount != 0)
            {
                entranceBlock.SetActive(false);
                waveTimer.SetActive(true);

                if (timerScript.timer <= 0)
                {
                    entranceBlock.SetActive(true);
                    wave++;
                    waveCount++;
                    waveTimer.SetActive(false);
                    SpawnWave(wave);
                }
            }
            
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
        Vector3 spawnPos = new Vector3(Random.Range(-xRange, xRange), Random.Range(minYRange, maxYRange), 35.4f);

        return spawnPos;
    }
}
