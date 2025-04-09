using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    
    [System.Serializable]
    public class Pool {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    
    public static ObjectPoolManager instance;

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public GameObject BulletContainer;
    public GameObject EnemyContainer;

    void Awake()
    {
        if(instance == null) {
            instance = this;
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach(var pool in pools) {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for(int i = 0; i < pool.size; i++) {
                GameObject obj = Instantiate(pool.prefab);
                InitParent(obj, obj.tag);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool); 
        }   
    }

    public GameObject SpawnFromPool(string tag, Vector2 position, Quaternion rotation, GameObject parentObj = null) {

        if(!poolDictionary.ContainsKey(tag)) return null;

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        InitCompParent(objectToSpawn, parentObj, objectToSpawn.tag);

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;


        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();

        if(pooledObj != null) {
            pooledObj.OnObjectSpawn();
        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    void InitCompParent(GameObject obj, GameObject parentObj, string tag) {
        switch(tag) {
            case "Bullet":
                obj.GetComponent<Bullet>().parentObj = parentObj;
                break;
        }
    }

    void InitParent(GameObject obj, string tag) {
        switch(tag) {
            case "Bullet":
                obj.transform.parent = BulletContainer.transform;
                break;
            case "Enemy":
                obj.transform.parent = EnemyContainer.transform;
                break;
        }
    }
}
