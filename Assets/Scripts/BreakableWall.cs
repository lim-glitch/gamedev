using UnityEngine;

public class BreakableWall : MonoBehaviour
{

    public void BreakWall(PlayerMovement player)
    {
        Debug.Log("Wall detected");

        if (player.avatarType ==
            PlayerMovement.AvatarType.Solid)
        {
            Debug.Log("Wall destroyed");
            Destroy(gameObject);
        }
    }
}