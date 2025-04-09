using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    ObjectPoolManager objectPoolManager;
    [PropertyRange(1, 5)] public float minDelay = 1;
    [PropertyRange(6, 25f)] public float maxDelay = 10;
    [ReadOnly] public float cooldownTimer = 0;

    [SerializeField] public List<GameObject> spawnLocation;
    [SerializeReference, ReadOnly] private List<GameObject> inactiveLocation;
    [SerializeField, ReadOnly] public List<GameObject> activeLocation;

    void Awake()
    {
        foreach(Transform child in transform) {
            if(child.name != this.gameObject.name) spawnLocation.Add(child.gameObject);
        }
    }

    void Start()
    {
        objectPoolManager = ObjectPoolManager.instance;   
        inactiveLocation = spawnLocation;
    }

    void FixedUpdate()
    {
        cooldownTimer -= Time.deltaTime;
        if(cooldownTimer <= 0) {
            StartCoroutine(SpawnBots());
            cooldownTimer = RandomDelay();
        }
    }

    float RandomDelay() {
        return Random.Range(minDelay, maxDelay);
    }

    Vector2 RandomPosition() {
        int temp = Random.Range(0, inactiveLocation.Count-1);
        GameObject tempLoc = inactiveLocation[temp];

        activeLocation.Add(tempLoc);
        inactiveLocation.RemoveAt(temp);

        return tempLoc.transform.position;
    }

    IEnumerator SpawnBots() {
        int count = Random.Range(1, 10); 
        for(int i = 0; i < count; i++) {
           SpawnBotIndex();
        }
        ReAddLocation();

        yield return new WaitForSeconds(0.1f);
    }

    void SpawnBotIndex() {
        int index = Random.Range(0, 3);
        switch(index) {
            case 0:
                objectPoolManager.SpawnFromPool("Enemy1", RandomPosition(), Quaternion.identity);
                break;
            case 1:
                objectPoolManager.SpawnFromPool("Enemy2", RandomPosition(), Quaternion.identity);
                break;
            case 2:
                objectPoolManager.SpawnFromPool("Enemy3", RandomPosition(), Quaternion.identity);
                break;
        }
    }

    void ReAddLocation() {
        foreach(var obj in activeLocation) {
            inactiveLocation.Add(obj);
        }
        activeLocation.Clear();
    }
}
