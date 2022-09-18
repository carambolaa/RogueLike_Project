using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet: MonoBehaviour
{
    private Transform shooter;
    private Transform enemy;
    private float damageMultiplierBase = 1.5f;

    private void Awake()
    {
        Invoke("DestroyBullet", 7);
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

    public void SetShooter(Transform player)
    {
        shooter = player;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            enemy = collision.transform;
            shooter.GetComponent<CharacterManager>()?.DealDamage(enemy, damageMultiplierBase);
            Destroy(this.gameObject);
        }
    }
}
