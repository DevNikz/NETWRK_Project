using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [TitleGroup("REFERENCES", Alignment = TitleAlignments.Centered)]
    [SerializeReference] public int playerCount;

    [TitleGroup("REFERENCES", Alignment = TitleAlignments.Centered)]
    [SerializeReference] public GameObject referenceGrid;
    [SerializeReference] public GameObject referencePlayerScore;
    [SerializeReference] public GameObject referencePlayer;
    [SerializeReference, ReadOnly] public List<GameObject> playerSpawn;
    

    void Awake()
    {
        foreach(Transform child in transform) {
            if(child.name != this.gameObject.name) playerSpawn.Add(child.gameObject);
        }
    }

    void Start() {
        //playerCount = GameObject.FindGameObjectsWithTag("PlayerShip").Length; //replace this for NM
        //AddPlayers();
        AddPlayer();
    }

    void AddPlayers() {
        for(int i = 1; i <= playerCount; i++) {
            
        }
    }

    void AddPlayer() {
        playerCount++;
        GameObject tempPlayer = Instantiate(referencePlayer, RandomLocation(), Quaternion.identity);
        tempPlayer.transform.Find("Canvas").transform.Find("Player").GetComponent<TextMeshProUGUI>().text = $"P{playerCount}";
        tempPlayer.name = $"Player{playerCount}";
        
        GameObject tempScore = Instantiate(referencePlayerScore, referenceGrid.transform);
        tempScore.transform.Find("NAME").GetComponent<TextMeshProUGUI>().text = $"P{playerCount}";
        tempScore.transform.Find("SCORE").GetComponent<TextMeshProUGUI>().text = "0";
        tempPlayer.GetComponent<PlayerController>().scoreText = tempScore.transform.Find("SCORE").GetComponent<TextMeshProUGUI>();
    }

    Vector2 RandomLocation() {
        int temp = Random.Range(0, playerSpawn.Count-1);
        GameObject tempLoc = playerSpawn[temp];

        return tempLoc.transform.position;
    }

    void Update() {

    }
}
