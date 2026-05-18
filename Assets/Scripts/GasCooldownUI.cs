using UnityEngine;
using UnityEngine.UI;

public class GasCooldownUI : MonoBehaviour
{
    public CharacterSelector characterSelector;
    public Image cooldownFill;

    void Update()
    {
        if (characterSelector == null || cooldownFill == null)
            return;

        PlayerMovement player =
            characterSelector.GetCurrentPlayerMovement();

        if (player == null)
        {
            cooldownFill.fillAmount = 0f;
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