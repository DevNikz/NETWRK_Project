using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerUI : NetworkBehaviour {

    [SerializeReference] public int score;
    [SerializeReference] public bool isReady;
    [SerializeReference] public TextMeshProUGUI readyText;
    [SerializeReference] public TextMeshProUGUI scoreText;

    void Update()
    {
        if(!IsOwner) return;

        if(Input.GetKeyDown(KeyCode.Space)) {
            RequestReadyServerRpc();
            switch(isReady) {
                case false:
                    AddReady();
                    break;
                default:
                    RemoveReady();
                    break;
            }
        }
    }

    public void InstanceAddScore() {
        RequestScoreServerRpc();
        AddScore();
    }

    [ServerRpc]
    void RequestScoreServerRpc(){
        ScoreClientRpc();
    }
    
    [ServerRpc]
    void RequestReadyServerRpc() {
        ReadyClientRpc();
    }
   

    [ClientRpc]
    void ScoreClientRpc() {
        if(!IsOwner) AddScore();
    }
 
    [ClientRpc]
    void ReadyClientRpc() {
        if(!IsOwner) {
            switch(isReady) {
                case false:
                    AddReady();
                    break;
                default:
                    RemoveReady();
                    break;
            }
        }
    }

    public void AddReady() {
        isReady = true;
        readyText.enabled = true;
        SessionController.instance.SetReady(GetComponent<PlayerController>()._id, true);
    }

    public void RemoveReady() {
        isReady = false;
        readyText.enabled = false;
        SessionController.instance.SetReady(GetComponent<PlayerController>()._id, false);
    }

    public void AddScore() {
        score++;
        scoreText.text = $"{score}";
    }

}