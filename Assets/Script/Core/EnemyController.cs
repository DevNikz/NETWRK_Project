using Sirenix.OdinInspector;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : NetworkBehaviour
{
    [PropertyRange(0.1f, 25f)] public float Speed = 10f;
    [SerializeField] bool move = false;

    public void OnEnable()
    {
        move = true;   
    }

    void Update() {
        if(move) {
            Move();
        }
    }

    void Move() {
        MoveEnemyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    void MoveEnemyServerRpc() {
        MoveEnemyClientRpc();
    }

    [ClientRpc]
    void MoveEnemyClientRpc() {
        Vector3 pos = transform.position;
        pos = new Vector3(0, -Speed * Time.deltaTime, 0);
        transform.position += pos;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("DeleteBot")) {
            gameObject.layer = GetComponent<DamageHandler>().deadLayer;
            move = false;
            if(IsServer) InstanceDespawnServerRpc();
            else InstanceDespawnClientRpc();
            InstanceDespawnClientRpc();
        }      
    }

    [ServerRpc]
    void InstanceDespawnServerRpc() {
        GetComponent<NetworkObject>().Despawn();
    }

    [ClientRpc]
    void InstanceDespawnClientRpc() {
        gameObject.SetActive(false);
    }
}
