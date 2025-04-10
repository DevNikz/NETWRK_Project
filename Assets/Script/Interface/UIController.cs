using System.Collections;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour {
    Transform content;
    GameObject Welcome; //WELCOME
    GameObject MainMenu; //MAIN MENU
    GameObject Loading; //LOADING
    TextMeshProUGUI ipPlaceholder;
    public string ipStringPlaceholder;
    public string ipString;
    TextMeshProUGUI portPlaceholder;
    public string portStringPlaceholder;
    public string portString;
    public TextMeshProUGUI countdown;
    public string countdownText;

    void Awake()
    {
        content = transform.Find("Canvas");
        Welcome = content.Find("WELCOME").gameObject;
        MainMenu = content.Find("MAIN MENU").gameObject;
        //main menu children
            ipPlaceholder = MainMenu.transform.Find("SETTINGS/IP/Area/Placeholder").GetComponent<TextMeshProUGUI>();
            ipStringPlaceholder = ipPlaceholder.text;

            portPlaceholder = MainMenu.transform.Find("SETTINGS/PORT/Area/Placeholder").GetComponent<TextMeshProUGUI>();
            portStringPlaceholder = portPlaceholder.text;

        //End
        Loading = content.Find("LOADING").gameObject;
        countdown = content.Find("COUNTDOWN").GetComponent<TextMeshProUGUI>();

        HideAllUI();
        Welcome.SetActive(true);
    }

    void Update()
    {
        if(SessionController.instance.AllPlayersReady()) UpdateTimer();
    }

    void UpdateTimer() {
        if(SessionController.instance.GetTimer() > 0) countdown.text = $"{SessionController.instance.GetTimer()}";
        else countdown.text = "";
    }

    public void ReadIPInput(string s) {
        ipString = s;
    }

    public void ReadPortInput(string s) {
        portString = s;
    }
    
    public void SetMainMenu() {
        StartCoroutine(SetMainMenuCoroutine());
    }

    public void SetLobby() {
        StartCoroutine(SetLobbyCoroutine());
    }

    void HideAllUI() {
        Welcome.SetActive(false);
        MainMenu.SetActive(false);
        Loading.SetActive(false);
    }

    public string CheckIP() {
        if(MainMenu.gameObject.activeInHierarchy) {
            if(string.IsNullOrEmpty(ipString)) {
                return ipStringPlaceholder;
            }
            else return ipString;
        }
        else return default;
    }

    public int CheckPort() {
        if(MainMenu.gameObject.activeInHierarchy) {
            if(string.IsNullOrEmpty(portString)) {
                return int.Parse(portStringPlaceholder);
            }
            else return int.Parse(portString);
        }
        else return default;
    }


    IEnumerator SetMainMenuCoroutine() {
        HideAllUI();
        Loading.SetActive(true);
        yield return new WaitForSeconds(5f);
        HideAllUI();
        MainMenu.SetActive(true);
    }

    IEnumerator SetLobbyCoroutine() {
        HideAllUI();
        yield break;
    }
}