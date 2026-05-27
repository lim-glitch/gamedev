using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class MultiplayerManager : NetworkBehaviour
{

    public NetworkVariable<bool> player1Ready = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> player2Ready = new NetworkVariable<bool>(false);
    public DebugUI debugUI;
   
    public void CreateRoom()
    {
        Debug.Log(Application.persistentDataPath);

        NetworkManager.Singleton.StartHost();
        Debug.Log("Host started");
    }

    public void JoinRoom()
    {
        var transport = NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>();

        transport.ConnectionData.Address = "127.0.0.1";

        NetworkManager.Singleton.StartClient();
        debugUI.Log("Client joined");
    }

    public void Ready()
    {
        Debug.Log("Ready button clicked");
        SubmitReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    void SubmitReadyServerRpc(ServerRpcParams rpcParams = default)
    {
        Debug.Log("ServerRpc received");

        ulong clientId = rpcParams.Receive.SenderClientId;

        if(clientId == NetworkManager.Singleton.LocalClientId)
        {
            player1Ready.Value = true;
            Debug.Log("Player 1 Ready");
        }
        else
        {
            player2Ready.Value = true;
            Debug.Log("Player 2 Ready");
        }

        checkStartGame();
    }


    public void checkStartGame()
    {
        if (player1Ready.Value && player2Ready.Value)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("Level1", LoadSceneMode.Single);
        }
    }
}
