using System.Collections;
using System.Collections.Generic;
using DilmerGames.Core.Singletons;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;

public class EnemyManager : NetworkSingleton<EnemyManager>
{
    [System.Serializable]
    public struct enemyData {
        public GameObject enemy;
        public int count;
    }

    [SerializeField] private List<enemyData> enemyList;
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
        inactiveLocation = spawnLocation;

        NetworkManager.Singleton.OnServerStarted += () => { 
            NetworkObjectPool.Instance.InitializePool();
        };
    }

    void FixedUpdate()
    {
        if(!IsServer) return;
        if(SessionController.instance.inGame) {
            SpawnTimer();
        }
    }

    void SpawnTimer() {
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
        SpawnBotIndex();
        // int count = Random.Range(1, 10); 
        // for(int i = 0; i < count; i++) {
        //    SpawnBotIndex();
        // }
        // ReAddLocation();

        yield return new WaitForSeconds(0.1f);
    }

    void SpawnBotIndex() {
        //int index = Random.Range(0);
        int index = 0;
        SpawnBot(enemyList[index].enemy, RandomPosition(), Quaternion.identity, enemyList[index].count);
    }

    void SpawnBot(GameObject enemy, Vector2 pos, Quaternion rot, int count) {
        int temp = Random.Range(1, count);
        for (int i = 0; i < temp; i++) {
            GameObject go = NetworkObjectPool.Instance.GetNetworkObject(enemy).gameObject;
            go.transform.position = pos;
            go.transform.rotation = rot;
            go.GetComponent<NetworkObject>().Spawn();
        }
        ReAddLocation();
    }

    void ReAddLocation() {
        foreach(var obj in activeLocation) {
            inactiveLocation.Add(obj);
        }
        activeLocation.Clear();
    }
}
