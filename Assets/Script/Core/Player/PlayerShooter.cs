using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;

public class PlayerShooter : NetworkBehaviour {
    [PropertyRange(0.1f, 25f)] public float fireDelay = 0.25f;
    [ReadOnly] public float cooldownTimer = 0;
    public GameObject _projectile;
    public Transform _leftBullet, _rightBullet;

    void Update()
    {
        if(!IsOwner) return;

        cooldownTimer -= Time.deltaTime;
        if(cooldownTimer <= 0) {
            cooldownTimer = 0;
        }

        if(Input.GetKey(KeyCode.Space) && cooldownTimer <= 0) {
            cooldownTimer = fireDelay;
            RequestFireServerRpc();
            Shoot();
        }
    }

    [ServerRpc]
    private void RequestFireServerRpc() {
        FireClientRpc();
    }

    [ClientRpc]
    private void FireClientRpc() {
        if(!IsOwner) Shoot();
    }

    void Shoot() {
        var projectile1 = Instantiate(_projectile, _leftBullet.position, Quaternion.identity, SessionController.instance.bulletContainer.transform);
        projectile1.GetComponent<Bullet>().parentObj = this.gameObject;

        var projectile2 = Instantiate(_projectile, _rightBullet.position, Quaternion.identity, SessionController.instance.bulletContainer.transform);
        projectile2.GetComponent<Bullet>().parentObj = this.gameObject;
    }
}