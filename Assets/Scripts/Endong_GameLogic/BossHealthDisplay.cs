using UnityEngine;
using TMPro;

public class BossHealthDisplay : MonoBehaviour
{
    public BossController bossController;
    public TMP_Text healthText;

    public Vector3 offset = new Vector3(0, 3f, 0);

    void Start()
    {
        if (healthText == null)
        {
            healthText = GetComponent<TMP_Text>();
        }
    }

    void LateUpdate()
    {
        if (bossController == null || healthText == null) return;

        transform.position = bossController.transform.position + offset;

        healthText.text = "Boss HP: " + bossController.currentHP + " / " + bossController.maxHP;
    }
}