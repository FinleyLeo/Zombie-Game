using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ShopManager : MonoBehaviour
{
    public AudioSource buySound;
    public AudioSource cantAfford;

    public GameObject player;
    private PlayerController playerscript;
    private PlayerAimWeapon playerAim;

    private bool gun2Bought, gun3Bought, gun4Bought, gun5Bought;

    public Image[] guns;
    public GameObject[] prices;

    // Start is called before the first frame update
    void Start()
    {
        playerscript = player.GetComponent<PlayerController>();
        playerAim = player.GetComponent<PlayerAimWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyItem(int item)
    {
        // Health
        if (item == 1 && playerscript.coins >= 50)
        {
            buySound.Play();
            playerscript.health = 6f;
            playerscript.coins -= 50;
        }

        // Batteries
        else if (item == 2 && playerscript.coins >= 25)
        {
            buySound.Play();
            playerscript.lightCharge = 60f;
            playerscript.coins -= 25;
        }

        else if (item == 3)
        {
            playerscript.currentGun = CurrentGun.pistol;
            playerAim.SetGun();
        }

        // Shotgun
        else if (item == 4 && playerscript.coins >= 100 && !gun2Bought)
        {
            buySound.Play();
            playerscript.coins -= 100;
            prices[0].SetActive(false);

            Color tmp = guns[0].GetComponent<Image>().color;
            tmp.a = 1f;
            guns[0].GetComponent<Image>().color = tmp;
            
            playerscript.currentGun = CurrentGun.shotgun;
            gun2Bought = true;
            playerAim.SetGun();
        }

        // equips Shotgun
        else if (item == 4 && gun2Bought)
        {
            playerscript.currentGun = CurrentGun.shotgun;
            playerAim.SetGun();
        }

        // Sniper
        else if (item == 5 && playerscript.coins >= 150 && !gun3Bought)
        {
            buySound.Play();
            playerscript.coins -= 150;
            prices[1].SetActive(false);

            Color tmp = guns[1].GetComponent<Image>().color;
            tmp.a = 1f;
            guns[1].GetComponent<Image>().color = tmp;

            playerscript.currentGun = CurrentGun.sniper;
            gun3Bought = true;
            playerAim.SetGun();
        }

        // Equips Sniper
        else if (item == 5 && gun3Bought)
        {
            playerscript.currentGun = CurrentGun.sniper;
            playerAim.SetGun();
        }

        // Minigun
        else if (item == 6 && playerscript.coins >= 200 && !gun4Bought)
        {
            buySound.Play();
            playerscript.coins -= 200;
            prices[2].SetActive(false);

            Color tmp = guns[2].GetComponent<Image>().color;
            tmp.a = 1f;
            guns[2].GetComponent<Image>().color = tmp;

            playerscript.currentGun = CurrentGun.minigun;
            gun4Bought = true;
            playerAim.SetGun();
        }

        // Equips Minigun
        else if (item == 6 && gun4Bought)
        {
            playerscript.currentGun = CurrentGun.minigun;
            playerAim.SetGun();
        }

        // RPG
        else if (item == 7 && playerscript.coins >= 250 && !gun5Bought)
        {
            buySound.Play();
            playerscript.coins -= 250;
            prices[3].SetActive(false);

            Color tmp = guns[3].GetComponent<Image>().color;
            tmp.a = 1f;
            guns[3].GetComponent<Image>().color = tmp;

            playerscript.currentGun = CurrentGun.RPG;
            gun5Bought = true;
            playerAim.SetGun();
        }

        // Equips RPG
        else if (item == 7 && gun5Bought)
        {
            playerscript.currentGun = CurrentGun.RPG;
            playerAim.SetGun();
        }

        // Plays when cant afford item
        else
        {
            cantAfford.Play();
        }

        playerscript.coinsText.text = playerscript.coins.ToString();
    }

}
