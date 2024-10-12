using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    public GameObject loading;
    public GameObject title;
    public GameObject load;

    public Camera cam;

    public AudioSource audioS;
    public AudioClip VHSload;
    public AudioClip VHSstartup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            audioS.PlayOneShot(VHSload);
            loading.SetActive(true);
            StartCoroutine(startupTime());
        }
    }

    IEnumerator startupTime()
    {
        yield return new WaitForSeconds(VHSload.length);

        loading.SetActive(false);
        title.SetActive(false);
        load.SetActive(true);
        cam.backgroundColor = Color.black;
        audioS.PlayOneShot(VHSstartup);

        StartCoroutine(SwitchScene());
    }

    IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(VHSstartup.length);

        SceneManager.LoadScene("Main Game");
    }
}
