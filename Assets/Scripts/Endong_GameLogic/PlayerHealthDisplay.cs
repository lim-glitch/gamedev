using UnityEngine;
using TMPro;

public class PlayerHealthDisplay : MonoBehaviour
{
    public Health playerHealth;
    public TMP_Text healthText;

    void Start()
    {
        if (playerHealth == null)
        {
            playerHealth = GetComponentInParent<Health>();
        }

        if (healthText == null)
        {
            healthText = GetComponent<TMP_Text>();
        }
    }

    void Update()
    {
        if (playerHealth != null && healthText != null)
        {
            healthText.text = "HP: " + playerHealth.currentHP;
        }
    }
}