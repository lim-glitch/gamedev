using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    public Image[] hearts;

    public Sprite fullHeart;
    public Sprite emptyHeart;

    private Health playerHealth;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerHealth = player.GetComponent<Health>();
        }

        Debug.Log("Player found: " + player);
        Debug.Log("Health found: " + playerHealth);
    }
    void Update()
    {
        if (playerHealth == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                playerHealth = player.GetComponent<Health>();

                if (playerHealth == null)
                    playerHealth = player.GetComponentInChildren<Health>();
            }

            return;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite =
                (i < playerHealth.currentHP) ? fullHeart : emptyHeart;
        }
    }
}