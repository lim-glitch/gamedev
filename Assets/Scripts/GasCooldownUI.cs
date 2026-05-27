using UnityEngine;
using UnityEngine.UI;

public class GasCooldownUI : MonoBehaviour
{
    public PlayerSpawner playerSpawner;
    public Image cooldownFill;

    void Update()
    {
        if (playerSpawner == null || cooldownFill == null)
            return;

        if (playerSpawner.currentPlayer == null)
            return;

        PlayerMovement player =
             playerSpawner.currentPlayer.GetComponent<PlayerMovement>();

        if (player == null)
        {
            //cooldownFill.fillAmount = 0f;
            return;
        }

        if (player.avatarType != PlayerMovement.AvatarType.Gas)
        {
            cooldownFill.fillAmount = 0f;
            return;
        }

        if (player.gasOnCooldown)
        {
            cooldownFill.fillAmount =
                1f - player.gasCooldownTimer / player.gasCooldown;
        }
        else
        {
            cooldownFill.fillAmount = 0f;
        }
    }
}