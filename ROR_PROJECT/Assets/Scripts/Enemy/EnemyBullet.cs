using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private float damage;

    private void Awake()
    {
        damage = Random.Range(10, 20);
        Invoke("DestroyBullet", 7);
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "Player")
        {
            CharacterManager.Instance.RecieveDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
