using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Deprecated : MonoBehaviour {

    // private NetworkVariable<int> score = new NetworkVariable<int>();
    // private NetworkVariable<bool> isReady = new NetworkVariable<bool>();
    // [SerializeReference] public TextMeshProUGUI readyText;
    // [SerializeReference] public TextMeshProUGUI scoreText;

    // public delegate void ReadyChanged(bool isActive);
    // public event ReadyChanged OnReadyChanged;

    // void Awake()
    // {
    //     isReady.OnValueChanged += OnValueChanged1;
    // }

    // private void OnValueChanged1(bool wasActive, bool isActive)
    // {
    //     if(isActive) Debug.Log($"Player{GetComponent<PlayerController>()._id} is ready");
    //     else Debug.Log($"Player{GetComponent<PlayerController>()._id} is not ready");
    // }


    // [ServerRpc(RequireOwnership = true)]
    // private void CommitNetworkIsReadyServerRpc(bool isActive) {
    //     CommitNetworkIsReadyClientRpc(isActive);
    // }

    // [ClientRpc]
    // private void CommitNetworkIsReadyClientRpc(bool isActive) {
    //     isReady.Value = isActive;
    //     OnReadyChanged?.Invoke(isActive);
    // }

    // public void AddReady() {
    //     readyText.enabled = true;
    //     CommitNetworkIsReadyServerRpc(true);
    // }

    // public void RemoveReady() {
    //     readyText.enabled = false;
    //     CommitNetworkIsReadyServerRpc(false);
    // }

    // public void AddScore() {
    //     //score++;
    //     scoreText.text = $"{score}";
    // }
}