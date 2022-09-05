using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenetrateBullet : MonoBehaviour
{
    [SerializeField]
    private Transform shooter;
    [SerializeField]
    private Transform enemy;
    private float damageMultiplierBase = 2f;
    private float damageMultiplier = 1.4f;

    private void Awake()
    {
        Invoke("DestroyBullet", 12);
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

    public void SetShooter(Transform player)
    {
        shooter = player;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemy = other.transform;
            shooter.GetComponent<CharacterManager>()?.DealDamage(enemy, damageMultiplierBase);
            damageMultiplierBase *= damageMultiplier;
        }
    }
}
