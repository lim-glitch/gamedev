using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public GameObject gasPlayer;
    public GameObject liquidPlayer;
    public GameObject solidPlayer;

    public GameObject currentPlayer;

    void Start()
    {
        SpawnPlayer(gasPlayer);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnPlayer(gasPlayer);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpawnPlayer(liquidPlayer);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SpawnPlayer(solidPlayer);
        }
    }

    void SpawnPlayer(GameObject selectedPlayer)
    {
        Vector3 spawnPosition = Vector3.zero;

        if (currentPlayer != null)
        {
            spawnPosition = currentPlayer.transform.position;
            Destroy(currentPlayer);
        }

        currentPlayer = Instantiate(
            selectedPlayer,
            spawnPosition,
            Quaternion.identity
        );

        CameraFollow cameraFollow =
            Camera.main.GetComponent<CameraFollow>();

        if (cameraFollow != null)
        {
            cameraFollow.target =
                currentPlayer.transform;
        }
    }

    public PlayerMovement GetCurrentPlayerMovement()
    {
        if (currentPlayer == null)
        {
            return null;
        }

        return currentPlayer.GetComponent<PlayerMovement>();
    }
}