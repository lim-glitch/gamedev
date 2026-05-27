using UnityEngine;

public class BossToxicMagic : MonoBehaviour
{
    public float speed = 3f;
    public float lifeTime = 4f;

    private Transform player;

    void Start()
    {
        GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");

        if (foundPlayer != null)
        {
            player = foundPlayer.transform;
        }

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (player == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );
    }
}