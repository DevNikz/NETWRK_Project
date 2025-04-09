using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [TitleGroup("PROPERTIES", Alignment = TitleAlignments.Centered)]
    [SerializeField, PropertyRange(0.1f, 10f)] public float speed = 1f;
    [SerializeReference, ReadOnly] private float vert;
    [SerializeReference, ReadOnly] private float hor;
    [SerializeReference, ReadOnly] private int score;

    [TitleGroup("BOUNDS", Alignment = TitleAlignments.Centered)]
    [SerializeReference, ReadOnly] private Vector2 screenBounds;
    [SerializeReference, ReadOnly] private float objectWidth;
    [SerializeReference, ReadOnly] private float objectHeight;

    [TitleGroup("REFERENCES")]
    [SerializeReference] public GameObject leftBullet;
    [SerializeReference] public GameObject rightBullet;
    [SerializeReference] public TextMeshProUGUI scoreText;

    void Start()
    {
        leftBullet.GetComponent<SpriteRenderer>().enabled = false;
        rightBullet.GetComponent<SpriteRenderer>().enabled = false;
        InitBounds();   
    }

    void Update()
    {
        vert = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        hor = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        MovePlayer();
        UpdateBounds();
    }

    void MovePlayer() {
        transform.Translate(hor, vert, 0f);
    }

    void InitBounds() {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
    }

    void UpdateBounds() {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(transform.position.x, -screenBounds.x + objectWidth, screenBounds.x - objectWidth);
        viewPos.y = Mathf.Clamp(transform.position.y, -screenBounds.y + objectHeight, screenBounds.y - objectHeight);
        transform.position = viewPos;
    }

    public void AddScore() {
        score++;
        scoreText.text = $"{score}";
    }
}
