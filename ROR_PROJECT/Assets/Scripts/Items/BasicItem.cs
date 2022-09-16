using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicItem : Item
{
    private float burningDamage = 3f;
    [SerializeField]
    private float burningRadius = 6f;
    private float burningDamageMultiplier;

    protected override void Start()
    {
        base.Start();
        CharacterManager.Instance.OnRecieveDamage += Cast;
        CharacterManager.Instance.OnEnemyKill += Burn;
    }

    private void Cast()
    {
        Debug.Log("Player got hit, heal!");
        CharacterManager.Instance.RecieveHealing(5);
    }

    private void Burn(Vector3 vec3)
    {
        Collider[] hitColliders = Physics.OverlapSphere(vec3, burningRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.transform.tag == "Enemy")
            {
                hitCollider.transform.GetComponent<Enemy>().StartBurning(burningDamage * CharacterManager.Instance.GetBurningDamageMultiplier());
            }
        }
    }
}