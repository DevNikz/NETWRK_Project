using Sirenix.OdinInspector;
using UnityEngine;

public class Bullet : MonoBehaviour, IPooledObject {
    
    [PropertyRange(0.1f, 25f)] public float Speed = 10f;

    bool shoot = false;
    public GameObject parentObj;

    public bool outofBounds;

    public void OnObjectSpawn() {
        shoot = true;
    }

    void OnDisable()
    {
        outofBounds = false;
    }

    void Update()
    {
        if(shoot) {
            Vector3 pos = transform.position;
            Vector3 velocity = new Vector3(0, Speed * Time.deltaTime, 0);
            pos += velocity;
            transform.position = pos;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy")) {
            parentObj.GetComponent<PlayerController>().AddScore();
        }

        if(collision.CompareTag("DeleteBullet")) {
            gameObject.layer = GetComponent<DamageHandler>().deadLayer;
            gameObject.SetActive(false);
        }      
    }
}
