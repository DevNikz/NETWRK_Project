using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;

public class DamageHandler : NetworkBehaviour
{
    [PropertyRange(1, 100)] public int maxHealth;
    [SerializeReference, ReadOnly] private int health;

    [SerializeReference] private LayerMask defaultLayer;
    [SerializeReference, ReadOnly] public LayerMask deadLayer;
    [SerializeReference, ReadOnly] public ParticleSystem hitParticle;
    [SerializeReference, ReadOnly] public ParticleSystem deathFX;
    bool isDead;

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

    void OnEnable() {
        health = maxHealth; 
    }

    void OnDisable() {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bullet")) {
            CheckHealth();

            //invulnTimer = invulnPeriod;
            if(gameObject.activeInHierarchy) StartCoroutine(HitCoroutine());
        }
    }


    void CheckHealth() {
        health--;
        if(health <= 0) isDead = true;
        else isDead = false;
    }


    IEnumerator HitCoroutine() {
        if(health > 0) {
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
            if(IsServer) InstanceDespawnServerRpc();
            else InstanceDespawnClientRpc();
            InstanceDespawnClientRpc();
        }

        yield return default;
    }

    public bool GetDead() {
        return isDead;
    }

    [ServerRpc]
    void InstanceDespawnServerRpc() {
        GetComponent<NetworkObject>().Despawn();
    }

    [ClientRpc]
    void InstanceDespawnClientRpc() {
        gameObject.SetActive(false);
    }
}
