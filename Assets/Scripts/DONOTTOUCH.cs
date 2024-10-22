using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DONOTTOUCH : MonoBehaviour
{
    private List<string> keyStrokeHistory;

    public bool cheatActive;

    // Start is called before the first frame update
    void Start()
    {
        keyStrokeHistory = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        KeyCode keyPressed = DetectKeyPressed();
        AddKeyStrokeToHistory(keyPressed.ToString());
        if (GetKeyStrokeHistory().Equals("UpArrow,UpArrow,DownArrow,DownArrow,LeftArrow,RightArrow,LeftArrow,RightArrow,B,A") && !cheatActive || GetKeyStrokeHistory().Equals("W,W,S,S,A,D,A,D,B,A") && !cheatActive)
        {
            Debug.Log("CHEAT CODE DETECTED");
            cheatActive = true;
        }
    }

    private KeyCode DetectKeyPressed()
    {
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                return key;
            }
        }
        return KeyCode.None;
    }

    private void AddKeyStrokeToHistory(string keyStroke)
    {
        if (!keyStroke.Equals("None"))
        {
            keyStrokeHistory.Add(keyStroke);

            if (keyStrokeHistory.Count > 10)
            {
                keyStrokeHistory.RemoveAt(0);
            }
        }
    }

    private string GetKeyStrokeHistory()
    {
        return string.Join(",", keyStrokeHistory.ToArray());
    }
}
