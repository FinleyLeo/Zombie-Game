using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform transf;
    private Vector3 startPos;

    public float duration = 0f;
    public float shakeMagnitude = 0.7f;
    public float dampingSpeed = 1.0f;

    void Awake()
    {
        if (transf == null)
        {
            transf = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        startPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (duration > 0)
        {
            transform.localPosition = startPos + Random.insideUnitSphere * shakeMagnitude;

            duration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            duration = 0f;
            transform.localPosition = startPos;
        }
    }

    public void TriggerShake()
    {
        duration = 0.1f;
    }
}
