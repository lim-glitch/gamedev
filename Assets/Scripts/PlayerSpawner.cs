using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject gasPlayer;
    public GameObject liquidPlayer;
    public GameObject solidPlayer;

    public Transform spawnPoint;
    public GameObject currentPlayer;

    void Start()
    {
        SpawnSelectedPlayer();
    }

    void SpawnSelectedPlayer()
    {
        GameObject playerToSpawn = gasPlayer;

        switch (CharacterSelect.selectedCharacter)
        {
            case 0:
                playerToSpawn = gasPlayer;
                break;

            case 1:
                playerToSpawn = liquidPlayer;
                break;

            case 2:
                playerToSpawn = solidPlayer;
                break;
        }

        Vector3 safeSpawn =
            spawnPoint != null
            ? spawnPoint.position
            : Vector3.zero;

        currentPlayer = Instantiate(
            playerToSpawn,
            safeSpawn,
            Quaternion.identity
        );

        CameraFollow cameraFollow =
            Camera.main.GetComponent<CameraFollow>();

        if (cameraFollow != null)
        {
            cameraFollow.target = currentPlayer.transform;
        }

        Debug.Log("Spawned: " + playerToSpawn.name);
    }

    /*void SpawnSelectedPlayer()
    {
        GameObject playerToSpawn = gasPlayer;

        Vector3 safeSpawn = spawnPoint != null
       ? spawnPoint.position
       : Vector3.zero;

       currentPlayer = Instantiate(playerToSpawn, safeSpawn, Quaternion.identity);

        switch (CharacterSelect.selectedCharacter)
        {
            case 0:
                playerToSpawn = gasPlayer;
                break;

            case 1:
                playerToSpawn = liquidPlayer;
                break;

            case 2:
                playerToSpawn = solidPlayer;
                break;
        }

        /*GameObject player = Instantiate(
            playerToSpawn,
            spawnPoint.position,
            Quaternion.identity
        );

        CameraFollow cameraFollow =
            Camera.main.GetComponent<CameraFollow>();

        if (cameraFollow != null)
        {
            cameraFollow.target = currentPlayer.transform;
        }

    }*/
}
