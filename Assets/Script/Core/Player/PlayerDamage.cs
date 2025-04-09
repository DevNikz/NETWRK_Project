using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerDamage : NetworkBehaviour {

    [Range(1, 100)] public int maxHealth;
    [SerializeReference] private int health;
    public float invulnPeriod = 0;
    float invulnTimer = 0;
    //int correctLayer;
    [SerializeReference] private LayerMask defaultLayer;
    [SerializeReference] public LayerMask deadLayer;
    [SerializeReference] public Animator animator;
    [SerializeReference] public ParticleSystem hitParticle;
    [SerializeReference] public ParticleSystem deathFX;


    void Start()
    {
        if(GetComponent<Animator>() != null) {
            animator = GetComponent<Animator>();
            hitParticle = transform.Find("HitEffect").GetComponent<ParticleSystem>();
            deathFX = transform.Find("DeathFX").GetComponent<ParticleSystem>();
        }
        defaultLayer = gameObject.layer;
        deadLayer = LayerMask.NameToLayer("Dead");
    }

    void OnEnable() {
        health = maxHealth; 
    }

    void OnDisable() {
        GetComponent<SpriteRenderer>().enabled = true;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        health--;
        invulnTimer = invulnPeriod;

        if(gameObject.activeInHierarchy) StartCoroutine(Hit());
    }


    IEnumerator Hit() {
        if(health > 0) {
            if(animator != null) animator.Play("Hit");
            if(hitParticle != null) hitParticle.Play();
        }

        //Dead
        else{
            if(deathFX != null) {
                GetComponent<SpriteRenderer>().enabled = false;
                deathFX.Play();
                yield return new WaitForSeconds(0.2f);
            }

            gameObject.layer = deadLayer;
            gameObject.SetActive(false);
        }
        yield return default;
    }

    void Update()
    {
        InvulnTimer();
    }

    void InvulnTimer() {
        invulnTimer -= Time.deltaTime;
        if(invulnTimer <= 0) {
            gameObject.layer = defaultLayer;
        }
    }
}