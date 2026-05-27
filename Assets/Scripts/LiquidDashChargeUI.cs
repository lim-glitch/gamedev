using UnityEngine;
using UnityEngine.UI;

public class LiquidDashChargeUI : MonoBehaviour
{
    public PlayerSpawner playerSpawner;
    public Image chargeFill;

    void Update()
    {
        if (playerSpawner == null || chargeFill == null)
            return;

        if (playerSpawner.currentPlayer == null)
            return;

        PlayerMovement player =
            playerSpawner.currentPlayer.GetComponent<PlayerMovement>();

        if (player == null)
        {
            //chargeFill.fillAmount = 0f;
            return;
        }

        if (player.avatarType != PlayerMovement.AvatarType.Liquid)
        {
            chargeFill.fillAmount = 0f;
            return;
        }

        if (!player.poweredUp)
        {
            chargeFill.fillAmount = 0f;
            return;
        }

        chargeFill.fillAmount =
            player.dashCharge / player.maxDashCharge;
    }
}