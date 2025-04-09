using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;

public class EnemyManager : NetworkBehaviour
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
    [SerializeField] public GameObject enemyContainer;

    void Awake()
    {
        foreach(Transform child in transform) {
            if(child.name != this.gameObject.name) spawnLocation.Add(child.gameObject);
        }

        NetworkManager.Singleton.OnServerStarted += () => {
            NetworkObjectPool.Instance.InitializePool();
        };
    }

    void Start()
    {
        inactiveLocation = spawnLocation;
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
        int count = Random.Range(1, 10); 
        for(int i = 0; i < count; i++) {
           SpawnBotIndex();
        }
        ReAddLocation();

        yield return new WaitForSeconds(0.1f);
    }

    void SpawnBotIndex() {
        int index = Random.Range(0, 3);
        SpawnBot(enemyList[index].enemy, RandomPosition(), Quaternion.identity, enemyList[index].count);
    }

    void SpawnBot(GameObject enemy, Vector2 pos, Quaternion rot, int count) {
        for (int i = 0; i < count; i++) {
            GameObject go = NetworkObjectPool.Instance.GetNetworkObject(enemy).gameObject;
            go.transform.position = pos;
            go.transform.rotation = rot;
            go.transform.parent = enemyContainer.transform;
            go.GetComponent<NetworkObject>().Spawn();
        }
    }

    void ReAddLocation() {
        foreach(var obj in activeLocation) {
            inactiveLocation.Add(obj);
        }
        activeLocation.Clear();
    }
}
