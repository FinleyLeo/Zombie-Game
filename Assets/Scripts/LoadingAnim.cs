using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingAnim : MonoBehaviour
{
    public TextMeshProUGUI loadingText;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("LoadAnim1", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadAnim1()
    {
        loadingText.text = "INSERTING.";
        Invoke("LoadAnim2", 0.5f);
    }

    void LoadAnim2()
    {
        loadingText.text = "INSERTING..";
        Invoke("LoadAnim3", 0.5f);
    }

    void LoadAnim3()
    {
        loadingText.text = "INSERTING...";
        Invoke("LoadAnim1", 0.75f);
    }
}
