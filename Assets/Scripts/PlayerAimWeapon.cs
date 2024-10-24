using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Cinemachine;
using TMPro;

public class PlayerAimWeapon : MonoBehaviour
{
    private Transform aim;
    private PlayerController playerC;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private DONOTTOUCH cheat;

    public GameObject bullet;
    public GameObject rocket;
    public GameObject rocketPrefab;
    private GameObject flash;
    private GameObject firePoint;
    public GameObject cam;
    public GameObject[] guns;
    public GameObject[] gunIcons;

    public Camera _cam;

    public AudioSource shoot;
    public AudioClip rocketShoot;
    public AudioClip empty;
    public AudioClip reload;
    public AudioClip rpgReload;

    private float coolDown;
    private float startCoolDown = 0.25f;
    public float maxAmmo = 10, ammo;
    public float recoil;
    public float damage = 3;
    private float shootDir;

    private bool isReloading;

    public Sprite lightPlayer;
    public Sprite heavyPlayer;

    public TextMeshProUGUI ammoText;

    public Texture2D cursor;

    private void OnEnable()
    {
        CinemachineCore.CameraUpdatedEvent.RemoveListener(OnCameraUpdated);
        CinemachineCore.CameraUpdatedEvent.AddListener(OnCameraUpdated);
    }

    private void OnDisable()
    {
        CinemachineCore.CameraUpdatedEvent.RemoveListener(OnCameraUpdated);
    }

    private void Awake()
    {
        playerC = GetComponent<PlayerController>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        cheat = GetComponent<DONOTTOUCH>();

        flash = GameObject.Find("Flash");
        flash.SetActive(false);
        gunIcons[0].SetActive(true);

        firePoint = GameObject.Find("FirePoint");

        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);

        coolDown = startCoolDown;
    }

    void OnCameraUpdated(CinemachineBrain brain)
    {
        if (!playerC.shopOpen && !playerC.Paused)
        {
            Aiming();
            Shooting();
        }
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

        ammoText.text = ammo + "/" + maxAmmo;

        if (cheat.cheatActive)
        {
            if (damage != 100)
            {
                damage = 100;
            }

            else if (maxAmmo != 1000)
            {
                maxAmmo = 1000;
            }

            else if (playerC.coins != 1000000)
            {
                playerC.coins = 1000000;
            }

            else if (playerC.health != 100)
            {
                playerC.health = 100;
            }

            else if (playerC.speed != 14 && playerC.maxSpeed != 14)
            {
                playerC.speed = 14;
                playerC.maxSpeed = 14;
            }
        }
    }

    void Aiming()
    {
        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 aimDir = (mousePos - transform.position).normalized;
        float zAngle = Mathf.Atan2(aimDir.x, aimDir.y) * Mathf.Rad2Deg;

        shootDir = zAngle;

        transform.eulerAngles = new Vector3(0, 0, -zAngle);
    }

    void Shooting()
    {
        if (playerC.currentGun != CurrentGun.minigun)
        {
            if (Input.GetMouseButtonDown(0) && coolDown <= 0 && ammo > 0 && !isReloading)
            {
                flash.SetActive(true);
                StartCoroutine(FlashTime());
                CameraShake.Instance.ShakeCamera(1f, 0.25f);
                

                if (playerC.currentGun != CurrentGun.RPG && playerC.currentGun != CurrentGun.shotgun)
                {
                    shoot.Play();
                    ammo--;

                    Instantiate(bullet, firePoint.transform.position, transform.rotation);
                }

                else if (playerC.currentGun == CurrentGun.shotgun)
                {
                    shoot.Play();
                    ammo -= 3;

                    Instantiate(bullet, firePoint.transform.position, transform.rotation);
                    Instantiate(bullet, firePoint.transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, -shootDir - 10));
                    Instantiate(bullet, firePoint.transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, -shootDir + 10));
                }
                
                else if (playerC.currentGun == CurrentGun.RPG)
                {
                    shoot.PlayOneShot(rocketShoot);
                    ammo--;

                    rocket.SetActive(false);

                    Instantiate(rocketPrefab, new Vector3(firePoint.transform.position.x, firePoint.transform.position.y, firePoint.transform.position.z - 35), transform.rotation);
                }

                coolDown = startCoolDown;
            }

            else if (Input.GetMouseButtonDown(0) && ammo < 1 && !isReloading)
            {
                shoot.PlayOneShot(empty);
            }
        }

        else if (playerC.currentGun == CurrentGun.minigun)
        {
            if (Input.GetMouseButton(0) && coolDown <= 0 && ammo > 0 && !isReloading)
            {   
                flash.SetActive(true);
                StartCoroutine(FlashTime());
                CameraShake.Instance.ShakeCamera(2f, 0.25f);
                shoot.Play();
                ammo--;

                Instantiate(bullet, new Vector3(firePoint.transform.position.x, firePoint.transform.position.y), Quaternion.Euler(transform.rotation.x, transform.rotation.y, -shootDir + Random.Range(-15f, 15f)));
                rb.AddForce(transform.rotation * Vector2.up * recoil, ForceMode2D.Force);

                coolDown = startCoolDown;
            }
        }
        

        if (Input.GetKeyDown(KeyCode.R) && !isReloading && playerC.currentGun != CurrentGun.RPG)
        {
            shoot.PlayOneShot(reload);
            GetComponent<PlayerController>().speed /= 2;
            isReloading = true;
            StartCoroutine(ReloadTime());
        }

        else if (Input.GetKeyDown(KeyCode.R) && !isReloading && playerC.currentGun == CurrentGun.RPG)
        {
            shoot.PlayOneShot(rpgReload);
            GetComponent<PlayerController>().speed /= 2;
            isReloading = true;
            StartCoroutine(RPGreloadTime());
        }
    }

    public void SetGun()
    {
        if (playerC.currentGun == CurrentGun.pistol)
        {
            guns[0].SetActive(true);
            guns[1].SetActive(false);
            guns[2].SetActive(false);
            guns[3].SetActive(false);
            guns[4].SetActive(false);

            gunIcons[0].SetActive(true);
            gunIcons[1].SetActive(false);
            gunIcons[2].SetActive(false);
            gunIcons[3].SetActive(false);
            gunIcons[4].SetActive(false);

            sr.sprite = lightPlayer;
            startCoolDown = 0.25f;
            damage = 3;
            maxAmmo = 15;
            ammo = maxAmmo;
        }

        else if (playerC.currentGun == CurrentGun.shotgun)
        {
            guns[0].SetActive(false);
            guns[1].SetActive(true);
            guns[2].SetActive(false);
            guns[3].SetActive(false);
            guns[4].SetActive(false);

            gunIcons[0].SetActive(false);
            gunIcons[1].SetActive(true);
            gunIcons[2].SetActive(false);
            gunIcons[3].SetActive(false);
            gunIcons[4].SetActive(false);

            sr.sprite = lightPlayer;
            startCoolDown = 0.5f;
            damage = 3;
            maxAmmo = 9;
            ammo = maxAmmo;
        }

        else if (playerC.currentGun == CurrentGun.sniper)
        {
            guns[0].SetActive(false);
            guns[1].SetActive(false);
            guns[2].SetActive(true);
            guns[3].SetActive(false);
            guns[4].SetActive(false);

            gunIcons[0].SetActive(false);
            gunIcons[1].SetActive(false);
            gunIcons[2].SetActive(true);
            gunIcons[3].SetActive(false);
            gunIcons[4].SetActive(false);

            cam.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 8;

            sr.sprite = heavyPlayer;
            startCoolDown = 1f;
            damage = 15;
            maxAmmo = 1;
            ammo = maxAmmo;
        }

        else if (playerC.currentGun == CurrentGun.minigun)
        {
            guns[0].SetActive(false);
            guns[1].SetActive(false);
            guns[2].SetActive(false);
            guns[3].SetActive(true);
            guns[4].SetActive(false);

            gunIcons[0].SetActive(false);
            gunIcons[1].SetActive(false);
            gunIcons[2].SetActive(false);
            gunIcons[3].SetActive(true);
            gunIcons[4].SetActive(false);

            sr.sprite = heavyPlayer;
            startCoolDown = 0.1f;
            damage = 1;
            maxAmmo = 60;
            ammo = maxAmmo;
        }

        else if (playerC.currentGun == CurrentGun.RPG)
        {
            guns[0].SetActive(false);
            guns[1].SetActive(false);
            guns[2].SetActive(false);
            guns[3].SetActive(false);
            guns[4].SetActive(true);

            gunIcons[0].SetActive(false);
            gunIcons[1].SetActive(false);
            gunIcons[2].SetActive(false);
            gunIcons[3].SetActive(false);
            gunIcons[4].SetActive(true);

            sr.sprite = heavyPlayer;
            startCoolDown = 1.5f;
            maxAmmo = 1;
            ammo = maxAmmo;
        }

        if (playerC.currentGun != CurrentGun.sniper)
        {
            cam.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 6;
        }

        flash.SetActive(true);
        flash = GameObject.Find("Flash");
        flash.SetActive(false);

        firePoint = GameObject.Find("FirePoint");
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
        rocket.SetActive(true);
    }

    IEnumerator RPGreloadTime()
    {
        yield return new WaitForSeconds(rpgReload.length);

        GetComponent<PlayerController>().speed *= 2;
        isReloading = false;
        ammo = maxAmmo;
        rocket.SetActive(true);
    }
}
