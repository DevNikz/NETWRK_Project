using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class Bullet : MonoBehaviour {
    
    [PropertyRange(0.1f, 25f)] public float Speed = 10f;
    [SerializeReference, ReadOnly] private int health = 1;
    [SerializeReference, ReadOnly] public LayerMask deadLayer;
    bool shoot = false;
    bool isColliding = false;
    public GameObject parentObj;

    void OnEnable()
    {
        deadLayer = LayerMask.NameToLayer("Dead");
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
            if(isColliding) return;
            isColliding = true;
            health--;
            if(gameObject.activeInHierarchy) StartCoroutine(Hit());
        }

        if(collision.CompareTag("DeleteBullet")) {
            Destroy(gameObject);
        }      
    }

    IEnumerator Hit() {
        gameObject.layer = deadLayer;
        gameObject.SetActive(false);
        parentObj.GetComponent<PlayerUI>().InstanceAddScore();
        yield return new WaitForSeconds(0.15f);
        Destroy(gameObject);

        yield return default;
    }
}
