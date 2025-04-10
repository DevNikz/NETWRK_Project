using System.Collections;
using TMPro;
using UnityEngine;

public class BlinkingText : MonoBehaviour
{
    [SerializeReference] TextMeshProUGUI text;
    [SerializeReference] float duration;
    void Start()
    {
        StartCoroutine(Blink());
    }

    IEnumerator Blink() {
        while(UserController.instance.GetState() == -1) {
            if(text.alpha == 1) {
                text.alpha = 0;
                yield return new WaitForSeconds(duration);
            }
            else{
                text.alpha = 1;
                yield return new WaitForSeconds(duration);
            }
        }
    }
}
