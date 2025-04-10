using System.Collections;
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
        HideAllUI();
    }

    
    public void SetMainMenu() {
        StartCoroutine(LoadingScreen());
        HideAllUI();
        MainMenu.SetActive(true);
    }

    void HideAllUI() {
        Welcome.SetActive(false);
        MainMenu.SetActive(false);
        Loading.SetActive(false);
    }


    IEnumerator LoadingScreen() {
        HideAllUI();
        Loading.SetActive(true);
        yield return new WaitForSeconds(2f);
    }
}