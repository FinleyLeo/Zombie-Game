using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private GameObject player;
    public GameObject zombie, zomboss;
    public GameObject waveTimer;
    private WaveTimer timerScript;

    public GameObject entranceBlock;
    private GameObject boss;

    public TextMeshProUGUI waveCounter;

    public float xRange;
    public float minYRange;
    public float maxYRange;
    public float bossHealth;

    public int zombieCount;
    public int wave = 2;
    private int waveCount;
    public int breakCheck;
    public int bossCheck;

    public bool bossSpawned;

    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();

        timerScript = waveTimer.GetComponent<WaveTimer>();

        waveCount = wave - 2;
        breakCheck = waveCount;
        bossCheck = waveCount;
        waveCounter.text = waveCount.ToString();

        bossHealth = 400;
    }

    // Update is called once per frame
    void Update()
    {
        zombieCount = FindObjectsOfType<EnemyScript>().Length;

        if (boss != null)
        {
            bossHealth = boss.GetComponent<EnemyScript>().health;
        }

        if (zombieCount == 0 && playerController.onGround)
        {
            waveCheck();
        }
    }

    void waveCheck()
    {
        // wave break
        if (breakCheck == 5 && bossCheck != 10) 
        {
            waveBreak();
        }

        // normal wave
        else if (breakCheck != 5 && bossCheck != 10 && !bossSpawned)
        {
            entranceBlock.SetActive(true);
            wave++;
            waveCount++;
            breakCheck++;
            bossCheck++;
            SpawnWave(wave);
        }

        // boss wave
        else if (bossCheck == 10 && !bossSpawned)
        {
            bossSpawned = true;
            entranceBlock.SetActive(true);
            boss = Instantiate(zomboss, GenerateSpawnPos(), Quaternion.identity);
            bossCheck = 0;
            breakCheck = 0;
        }

        if (bossHealth <= 0 && zombieCount == 0 && bossSpawned)
        {
            waveBreak();
        }
    }

    void waveBreak()
    {
        entranceBlock.SetActive(false);
        waveTimer.SetActive(true);

        if (timerScript.timer <= 0)
        {
            entranceBlock.SetActive(true);
            wave++;
            waveCount++;
            timerScript.timer = 15;
            waveTimer.SetActive(false);
            bossSpawned = false;
            bossHealth = 400;
            SpawnWave(wave);
            breakCheck = 0;
            breakCheck++;
            bossCheck++;
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
