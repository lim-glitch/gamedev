using UnityEngine;
using UnityEngine.UI;

public class LiquidDashChargeUI : MonoBehaviour
{
    public CharacterSelector characterSelector;
    public Image chargeFill;

    void Update()
    {
        if (characterSelector == null || chargeFill == null)
            return;

        PlayerMovement player =
            characterSelector.GetCurrentPlayerMovement();

        if (player == null)
        {
            chargeFill.fillAmount = 0f;
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