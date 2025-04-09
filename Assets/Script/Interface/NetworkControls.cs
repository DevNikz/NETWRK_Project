using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkControls : MonoBehaviour
{
    public void StartHost() {
        if(!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer) {
            NetworkManager.Singleton.StartHost();
        }
    }

    public void StartClient() {
        if(!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer) {
            NetworkManager.Singleton.StartClient();
        }
    }
}
