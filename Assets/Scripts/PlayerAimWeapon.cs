using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Cinemachine;

public class PlayerAimWeapon : MonoBehaviour
{
    private Transform aim;

    public GameObject bullet;
    public GameObject flash;
    public GameObject firePoint;
    public GameObject cam;

    public Camera _cam;

    public AudioSource shoot;
    public AudioClip empty;
    public AudioClip reload;

    private float coolDown = 0.25f;
    public float maxAmmo = 10, ammo;

    private bool isReloading;

    private void OnEnable()
    {
        CinemachineCore.CameraUpdatedEvent.RemoveListener(OnCameraUpdated);
        CinemachineCore.CameraUpdatedEvent.AddListener(OnCameraUpdated);
    }

    private void OnDisable()
    {
        CinemachineCore.CameraUpdatedEvent.RemoveListener(OnCameraUpdated);
    }

    void OnCameraUpdated(CinemachineBrain brain)
    {
        Aiming();
        Shooting();
    }

    // Start is called before the first frame update
    void Start()
    {
        ammo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        coolDown -= Time.deltaTime;
    }

    void Aiming()
    {
        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 aimDir = (mousePos - transform.position).normalized;
        float zAngle = Mathf.Atan2(aimDir.x, aimDir.y) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0, 0, -zAngle);
    }

    void Shooting()
    {
        if (Input.GetMouseButtonDown(0) && coolDown <= 0 && ammo > 0 && !isReloading)
        {
            flash.SetActive(true);
            StartCoroutine(FlashTime());
            CameraShake.Instance.ShakeCamera(1f, 0.25f);
            shoot.Play();
            ammo--;

            Instantiate(bullet, firePoint.transform.position, transform.rotation);

            coolDown = 0.25f;
        }

        else if (Input.GetMouseButtonDown(0) && ammo < 1 && !isReloading)
        {
            shoot.PlayOneShot(empty);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            shoot.PlayOneShot(reload);
            GetComponent<PlayerController>().speed /= 2;
            isReloading = true;
            StartCoroutine(ReloadTime());
        }
    }

    IEnumerator FlashTime()
    {
        yield return new WaitForSeconds(0.1f);

        flash.SetActive(false);
    }

    IEnumerator ReloadTime()
    {
        yield return new WaitForSeconds(reload.length);

        GetComponent<PlayerController>().speed *= 2;
        isReloading = false;
        ammo = maxAmmo;
    }
}
