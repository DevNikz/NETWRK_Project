using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyController : MonoBehaviour, IPooledObject
{
    [PropertyRange(0.1f, 25f)] public float Speed = 10f;
    bool move = false;

    public void OnObjectSpawn() {
        move = true;
    }

    void Update() {
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
