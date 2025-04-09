using Sirenix.OdinInspector;
using UnityEngine;

public class Bullet : MonoBehaviour, IPooledObject {
    
    [PropertyRange(0.1f, 25f)] public float Speed = 10f;

    bool shoot = false;
    public GameObject parentObj;

    public void OnObjectSpawn() {
        shoot = true;
    }

    void OnEnable()
    {
        shoot = true;
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
            //parentObj.GetComponent<PlayerController>().AddScore();
            parentObj.GetComponent<PlayerUI>().InstanceAddScore();
        }

        if(collision.CompareTag("DeleteBullet")) {
            Destroy(gameObject);
        }      
    }
}
