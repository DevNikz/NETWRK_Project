using Sirenix.OdinInspector;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{

    [TitleGroup("STATES", Alignment = TitleAlignments.Centered)]
    [SerializeField] public int _id;

    [TitleGroup("PROPERTIES", Alignment = TitleAlignments.Centered)]
    [SerializeField, PropertyRange(0.1f, 100f)] public float _acceleration = 1f;
    [SerializeField, PropertyRange(0.1f, 10f)] public float _maxVelocity = 1f;
    [SerializeReference, ReadOnly] private Vector2 _input;
    

    [TitleGroup("BOUNDS", Alignment = TitleAlignments.Centered)]
    [SerializeReference, ReadOnly] private Vector2 screenBounds;
    [SerializeReference, ReadOnly] private float objectWidth;
    [SerializeReference, ReadOnly] private float objectHeight;

    [TitleGroup("REFERENCES")]
    [SerializeReference] public Rigidbody2D _rb;
    [SerializeReference] public GameObject leftBullet;
    [SerializeReference] public GameObject rightBullet;
    

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        leftBullet.GetComponent<SpriteRenderer>().enabled = false;
        rightBullet.GetComponent<SpriteRenderer>().enabled = false;
        InitBounds();   
    }
    void InitBounds() {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
    }

    void Update()
    {
        _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void FixedUpdate()
    {
        MovePlayer();
        UpdateBounds();
    }

    void MovePlayer() {
        _rb.velocity += _input.normalized * (_acceleration * Time.deltaTime);
        _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, _maxVelocity);
    }

    void UpdateBounds() {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(transform.position.x, -screenBounds.x + objectWidth, screenBounds.x - objectWidth);
        viewPos.y = Mathf.Clamp(transform.position.y, -screenBounds.y + objectHeight, screenBounds.y - objectHeight);
        transform.position = viewPos;
    }
}
