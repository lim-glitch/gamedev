using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    public static int selectedCharacter = -1;

    [System.Serializable]
    public class CharacterUI
    {
        public Image buttonBackground;

        public Sprite selectedSprite;
        public Sprite unselectedSprite;
    }

    [Header("Character UI")]
    public CharacterUI[] characters;

    [Header("Character Data")]
    public string[] agentNames;
    public string[] agentTypes;
    public Sprite[] agentSprites;

    private void Start()
    {
        UpdateSelectionUI();
    }

    public void SelectCharacter(int id)
    {
        selectedCharacter = id;

        // SAVE DATA TO GAMEDATA
        GameData.selectedAgentIndex = id;
        GameData.agentName = agentNames[id];
        GameData.agentType = agentTypes[id];
        GameData.agentSprite = agentSprites[id];

        UpdateSelectionUI();

        Debug.Log("Selected: " + id);
    }

    void UpdateSelectionUI()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            bool selected = (i == selectedCharacter);

            characters[i].buttonBackground.sprite =
                selected
                ? characters[i].selectedSprite
                : characters[i].unselectedSprite;
        }
    }

    public void ConfirmSelection()
    {
        if (selectedCharacter == -1)
        {
            Debug.Log("No character selected!");
            return;
        }

        SceneManager.LoadScene("Briefing");
    }
}