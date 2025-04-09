using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerShooter : MonoBehaviour {
    ObjectPoolManager objectPoolManager;

    [PropertyRange(0.1f, 25f)] public float fireDelay = 0.25f;
    [ReadOnly] public float cooldownTimer = 0;

    void Start()
    {
        objectPoolManager = ObjectPoolManager.instance;   
    }

    void FixedUpdate()
    {
        cooldownTimer -= Time.deltaTime;
        if(cooldownTimer <= 0) {
            cooldownTimer = 0;
        }

        if(Input.GetKey(KeyCode.Space) && cooldownTimer <= 0) { 
            Shoot();
            cooldownTimer = fireDelay;
        }
    }

    void Shoot() {
        switch(this.name) {
            case "Player1":
                objectPoolManager.SpawnFromPool("LeftBullet1", GetComponent<PlayerController>().leftBullet.transform.position, Quaternion.identity, this.gameObject);
                objectPoolManager.SpawnFromPool("RightBullet1", GetComponent<PlayerController>().rightBullet.transform.position, Quaternion.identity, this.gameObject);
                break;
        }
    }
}