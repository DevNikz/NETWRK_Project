using UnityEngine;
using UnityEngine.UI;

public class ScrollingBackground : MonoBehaviour
{
    public float speed;
    [SerializeField] private RawImage bg;
    void Update() {
        bg.uvRect = new Rect(bg.uvRect.position + new Vector2(0, speed) * Time.deltaTime, bg.uvRect.size);
    }
}
