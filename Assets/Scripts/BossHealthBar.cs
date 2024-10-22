using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider slider;
    public GameObject health;
    private EnemyScript script;

    // Start is called before the first frame update
    void Start()
    {
        script = GetComponent<EnemyScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (script.health > 0)
        {
            slider.value = script.health;
        }
        
        else
        {
            slider.value = 0;
            health.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        health.transform.rotation = Quaternion.identity;
    }
}
