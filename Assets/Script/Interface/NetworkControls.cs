using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class NetworkControls : NetworkBehaviour
{
    public bool hasServerStarted;

    void Start()
    {
        NetworkManager.Singleton.OnServerStarted += () => { 
            hasServerStarted = true;
        };
    }

    public void StartHost() {
        EnterLobby();
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(UserController.instance.GetIP(), (ushort)UserController.instance.GetPort());
        NetworkManager.Singleton.StartHost();
    }
    
    public void StartClient() {
        EnterLobby();
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(UserController.instance.GetIP(), (ushort)UserController.instance.GetPort());
        NetworkManager.Singleton.StartClient();
    }

    public void EnterLobby() {
        UserController.instance.SetState(1);
    }
}
