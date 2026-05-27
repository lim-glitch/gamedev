using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToxicPitDeath : MonoBehaviour
{
    public float drownTime = 0.8f;
    public float restartDelay = 1.5f;
    public float sinkDistance = 0.4f;

    private bool triggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(DrownAndRestart(other.gameObject));
        }
    }

    IEnumerator DrownAndRestart(GameObject player)
    {
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.enabled = false;
        }

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        Vector3 startPosition = player.transform.position;
        Vector3 endPosition = startPosition + new Vector3(0, -sinkDistance, 0);

        float timer = 0f;

        while (timer < drownTime)
        {
            timer += Time.deltaTime;

            player.transform.position = Vector3.Lerp(
                startPosition,
                endPosition,
                timer / drownTime
            );

            yield return null;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }

        yield return new WaitForSecondsRealtime(restartDelay);

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}