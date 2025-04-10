using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class SessionController : MonoBehaviour
{
    public static SessionController instance;

    [TitleGroup("PROPERTIES", Alignment = TitleAlignments.Centered)]
    [SerializeReference] public int playerCount;
    [SerializeReference] public bool inLobby;
    [SerializeReference] public bool inGame;
    [SerializeReference] public List<bool> isReady;
    [Range(1, 20)] public float countdownTimer = 10;
    public float timer;

    [TitleGroup("REFERENCES", Alignment = TitleAlignments.Centered)]
    [SerializeReference] public List<PlayerUI> playerList;
    [SerializeReference] public GameObject referenceGrid;
    [SerializeReference] public GameObject referencePlayerScore;
    [SerializeReference] public NetworkManager networkManager;
    [SerializeReference] public GameObject bulletContainer;
    [SerializeReference, ReadOnly] public List<GameObject> playerSpawn;


    void Awake()
    {
        if(instance == null) {
            instance = this;
        }
        else Destroy(gameObject);

        foreach(Transform child in transform) {
            if(child.name != this.gameObject.name) playerSpawn.Add(child.gameObject);
        }

        inLobby = true;
        inGame = false;
        timer = countdownTimer;
    }

    void Update() {
        if(playerCount > 0) UpdateLobby();
        // if(!inLobby && inGame) UpdateGame();
    }

    void UpdateLobby() {
        if(AllPlayersReady() == true) {
            timer -= Time.deltaTime;
            if(timer <= 0) {
                Debug.Log("Player Start!");
                timer = 0;
                inLobby = false;
                inGame = true;
            }
        }
        else {
            timer = countdownTimer;
            inLobby = true;
            inGame = false;
            //Debug.Log("Players Not Ready");
        }
    }

    public int GetTimer() {
        return (int)timer;
    }

    public void AddPlayer(GameObject player) {
        playerCount++;

        player.transform.position = RandomLocation();
        player.transform.Find("Canvas").transform.Find("Player").GetComponent<TextMeshProUGUI>().text = $"P{playerCount}";
        player.name = $"Player{playerCount}";
        player.GetComponent<PlayerController>()._id = playerCount;

        GameObject tempScore = Instantiate(referencePlayerScore, referenceGrid.transform);
        tempScore.name = $"{player.name}_Score";

        tempScore.transform.Find("NAME").GetComponent<TextMeshProUGUI>().text = $"P{playerCount}";
        tempScore.transform.Find("SCORE").GetComponent<TextMeshProUGUI>().text = "0";
        tempScore.transform.Find("READY").GetComponent<TextMeshProUGUI>().enabled = false;
        isReady.Add(player.GetComponent<PlayerUI>().isReady);

        player.GetComponent<PlayerUI>().readyText = tempScore.transform.Find("READY").GetComponent<TextMeshProUGUI>();
        player.GetComponent<PlayerUI>().scoreText = tempScore.transform.Find("SCORE").GetComponent<TextMeshProUGUI>();
    }

    public bool AllPlayersReady() {
        if(isReady.All(x => x == true) && isReady.Count > 0) {
            return true;
        }
        else return false;
    }

    public void RemovePlayer() {
        playerCount--;
    }

    Vector2 RandomLocation() {
        int temp = Random.Range(0, playerSpawn.Count-1);
        GameObject tempLoc = playerSpawn[temp];

        return tempLoc.transform.position;
    }

    public void SetReady(int index, bool value) {
        isReady[index-1] = value;
    }
}
