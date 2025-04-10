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
        public GameObject prefab;
        public int count;
    }

    [SerializeField] private List<enemyData> enemyList;
    [PropertyRange(1, 5)] public float minDelay = 1;
    [PropertyRange(6, 25f)] public float maxDelay = 10;
    [ReadOnly] public float cooldownTimer = 0;
    [ReadOnly] public int enemyCountSpawned;
    [ReadOnly] public bool HasSpawned;

    [SerializeField] bool enableNetworkPool;
    [SerializeField] GameObject enemyContainer;
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

        if(enableNetworkPool) {
            NetworkManager.Singleton.OnServerStarted += () => { 
                NetworkObjectPool.Instance.InitializePool();
            };
        }
    }

    void Update()
    {
        if(!IsOwner) return;

        if(SessionController.instance.inGame) {
            TestServerRpc();
        }
    }

    [ServerRpc]
    void TestServerRpc() {
        //Debug.Log($"TestServerRpc accessed by Player{OwnerClientId}");
        int index, temp;

        cooldownTimer -= Time.deltaTime;
        if(cooldownTimer <= 0) {
            cooldownTimer = 0;

            //Spawn
            //Debug.Log("Spawning Enemies...");
            index = Random.Range(0, enemyList.Count-1);
            temp = Random.Range(1, enemyList[index].count);
            for (int i = 0; i < temp; i++) {
                enemyCountSpawned++;
                enemyData enemyRef = enemyList[index];
                Transform enemyTemp = Instantiate(enemyRef.prefab.transform, RandomPosition(), Quaternion.identity, enemyContainer.transform);
                enemyTemp.name = $"Enemy{enemyCountSpawned}";

                NetworkObject enemyNetworkObject = enemyTemp.GetComponent<NetworkObject>();
                enemyNetworkObject.Spawn(true);
            }
            ReAddLocation();
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

    void ReAddLocation() {
        foreach(var obj in activeLocation) {
            inactiveLocation.Add(obj);
        }
        activeLocation.Clear();
    }
}
