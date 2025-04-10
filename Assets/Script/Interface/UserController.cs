using UnityEngine;

public class UserController : MonoBehaviour {
    public static UserController instance;
    UIController ui;
    public int currentState = -1; // -1 = Welcome | 0 = Main Menu

    void Awake() {
        if(instance == null) {
            instance = this;
        }
        else Destroy(this);
    }

    void Start()
    {
        ui = GameObject.FindGameObjectWithTag("HUD").GetComponent<UIController>();   
    }

    public void Update()
    {
        switch(currentState) {
            case -1:
                if(Input.anyKeyDown) {
                    currentState = 0;
                    ui.SetMainMenu();
                }  
                break;
        }
        
    }

    public int GetState() {
        return currentState;
    }
}