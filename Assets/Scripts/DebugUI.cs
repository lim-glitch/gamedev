using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    public Text text;

    public void Log(string msg)
    {
        text.text += msg + "\n";
        Debug.Log(msg);
    }
}