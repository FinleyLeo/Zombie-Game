using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTime : MonoBehaviour
{
    public float seconds;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(KillTimer(seconds));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator KillTimer(float secs)
    {
        yield return new WaitForSeconds(secs);

        Destroy(gameObject);
    }
}
