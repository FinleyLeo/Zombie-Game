using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject inputText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(textDelay());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene("Main Game");
        }
    }

    IEnumerator textDelay()
    {
        yield return new WaitForSeconds(1);

        inputText.SetActive(true);
    }
}
