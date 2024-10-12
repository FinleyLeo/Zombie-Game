using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveTimer : MonoBehaviour
{
    public float timer = 15;
    public float roundedTimer;

    public bool waveReady;

    public GameObject player;
    private PlayerController playerC;

    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        playerC = player.GetComponent<PlayerController>();

        timer = 15;
        waveReady = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0 )
        {
            waveReady = true;
        }

        if (timer > 0 && !playerC.onFloor)
        {
            timer -= Time.deltaTime;
            roundedTimer = Mathf.RoundToInt(timer);
            text.text = roundedTimer.ToString();
        }
    }
}
