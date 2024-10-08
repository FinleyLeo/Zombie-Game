using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class PlayerAimWeapon : MonoBehaviour
{
    private Transform aim;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Aiming();
        Shooting();
    }

    void Aiming()
    {
        Vector3 mousePos = UtilsClass.GetMouseWorldPosition();

        Vector3 aimDir = (mousePos - transform.position).normalized;
        float zAngle = Mathf.Atan2(aimDir.x, aimDir.y) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0, 0, -zAngle);
    }

    void Shooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Shoot");
        }
    }
}
