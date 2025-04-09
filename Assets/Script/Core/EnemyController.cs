using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;

public class EnemyController : NetworkBehaviour
{
    [PropertyRange(0.1f, 25f)] public float Speed = 10f;
    [SerializeField] bool move = false;

    public override void OnNetworkSpawn() {
        move = true;
    }

    public void OnEnable()
    {
        move = true;   
    }

    void Update() {
        // if(!IsServer) return;

        if(move) {
            Vector3 pos = transform.position;
            pos = new Vector3(0, -Speed * Time.deltaTime, 0);
            transform.position += pos;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("DeleteBot")) {
            gameObject.layer = GetComponent<DamageHandler>().deadLayer;
            gameObject.SetActive(false);
        }      
    }
}
