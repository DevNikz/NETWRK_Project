using UnityEngine;

public class UIController : MonoBehaviour {
    Transform content;
    GameObject Welcome; //WELCOME
    GameObject MainMenu; //MAIN MENU
    GameObject Loading; //LOADING

    void Awake()
    {
        content = transform.Find("Canvas");
        Welcome = content.Find("WELCOME").gameObject;
        MainMenu = content.Find("MAIN MENU").gameObject;
        Loading = content.Find("LOADING").gameObject;
    }

}