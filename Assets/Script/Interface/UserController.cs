using Unity.VisualScripting;
using UnityEngine;

public class UserController : MonoBehaviour {
    public static UserController instance;
    UIController ui;
    public int currentState = -1; // -1 = Welcome | 0 = Main Menu | 1 = Lobby
    public bool isHost = false;
    public bool isClient = false;
    public string ip;
    public int port;
    public bool isLoading;

    void Awake() {
        if(instance == null) {
            instance = this;
        }
        else Destroy(this);
    }

    void Start()
    {
        ui = GameObject.FindGameObjectWithTag("HUD").GetComponent<UIController>();   
    }

    public void Update()
    {
        switch(currentState) {
            case -1:
                if(Input.anyKeyDown) {
                    if(Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))
                        return;
                    else {
                        currentState = 0;
                        ui.SetMainMenu();
                    }
                }
                break;
            case 0:
                ip = ui.CheckIP();
                //port = ui.CheckPort();
                break;  
            case 1:
                ui.SetLobby();
                break;
        }
    }

    public void SetState(int value) {
        currentState = value;
    }
    public int GetState() {
        return currentState;
    }

    public string GetIP() {
        ip = ui.CheckIP();
        Debug.Log($"IP: {ip}");
        return ip;
    }

    public int GetPort() {
        port = ui.CheckPort();
        Debug.Log($"Port: {port}");
        return port;
    }
}