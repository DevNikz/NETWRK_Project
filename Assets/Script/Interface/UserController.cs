using UnityEngine;

public class UserController : MonoBehaviour {
    public static UserController instance;

    public int currentState = -1; // -1 = Welcome | 0 = Main Menu

    void Awake() {
        if(instance == null) {
            instance = this;
        }
        else Destroy(this);
    }

    public void Update()
    {
        if(Input.anyKeyDown) {
            currentState = 0;
        }  
    }


    public int GetState() {
        return currentState;
    }
}