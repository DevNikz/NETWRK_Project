using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;

public class DamageHandler : NetworkBehaviour
{
    [PropertyRange(1, 100)] public int maxHealth;
    [SerializeReference, ReadOnly] private int health;
    public float invulnPeriod = 0;
    float invulnTimer = 0;
    //int correctLayer;
    [SerializeReference] private LayerMask defaultLayer;
    [SerializeReference, ReadOnly] public LayerMask deadLayer;
    [SerializeReference, ReadOnly] public Animator animator;
    [SerializeReference, ReadOnly] public ParticleSystem hitParticle;
    [SerializeReference, ReadOnly] public ParticleSystem deathFX;
    [SerializeReference] public int index;

    void Start()
    {
        if(transform.Find("HitEffect").GetComponent<ParticleSystem>() != null) {
            hitParticle = transform.Find("HitEffect").GetComponent<ParticleSystem>();
        }
        if(transform.Find("DeathFX").GetComponent<ParticleSystem>() != null) {
            deathFX = transform.Find("DeathFX").GetComponent<ParticleSystem>();
        }
        defaultLayer = gameObject.layer;
        deadLayer = LayerMask.NameToLayer("Dead");
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log($"{gameObject.name} has Spawned");
    }

    public override void OnNetworkDespawn()
    {
        Debug.Log($"{gameObject.name} has Despawned");
    }

    void OnEnable() {
        health = maxHealth; 
    }

    void OnDisable() {
        GetComponent<SpriteRenderer>().enabled = true;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bullet")) {
            health--;
            invulnTimer = invulnPeriod;
            if(gameObject.activeInHierarchy) StartCoroutine(Hit());
        }
    }


    IEnumerator Hit() {
        if(health > 0) {
            //if(animator != null) animator.Play("Hit");
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
            if(IsOwner) InstanceDespawnServerRpc();
            gameObject.SetActive(false);
        }

        yield return default;
    }

    [ServerRpc]
    void InstanceDespawnServerRpc() {
        GetComponent<NetworkObject>().Despawn();
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
