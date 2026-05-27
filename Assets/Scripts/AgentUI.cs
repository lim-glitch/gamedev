using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgentUI : MonoBehaviour
{
    public Image agentSprite;
    public Text agentName;
    public Text agentType;

    public void SetAgent(string name, string type, Sprite sprite)
    {
        agentName.text = name;
        agentType.text = type;
        agentSprite.sprite = sprite;
    }
}