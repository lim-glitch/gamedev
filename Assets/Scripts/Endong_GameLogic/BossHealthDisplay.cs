using UnityEngine;
using TMPro;

public class BossHealthDisplay : MonoBehaviour
{
    public BossController bossController;
    public TMP_Text healthText;

    public Vector3 offset = new Vector3(0, 3f, 0);

    void Update()
    {
        if (bossController == null) return;


        healthText.text = "Boss HP: " + bossController.currentHP + " / " + bossController.maxHP;
    }

}