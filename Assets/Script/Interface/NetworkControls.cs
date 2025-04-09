using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkControls : NetworkBehaviour
{
    public GameObject debug;
    public bool hasServerStarted;

    void Start()
    {
        NetworkManager.Singleton.OnServerStarted += () => { 
            hasServerStarted = true;
        };
    }

    public void StartHost() {
        NetworkManager.Singleton.StartHost();
        DisableDebug();
    }

    public void StartClient() {
        NetworkManager.Singleton.StartClient();
        DisableDebug();
    }

    public void DisableDebug() {
        debug.SetActive(false);
    }
}
