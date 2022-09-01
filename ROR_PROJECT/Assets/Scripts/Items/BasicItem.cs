using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicItem : Item
{
    private float burningPeriod = .5f;
    private float burningDamage = 1;

    protected override void Start()
    {
        base.Start();
        CharacterManager.Instance.OnRecieveDamage += Cast;
        CharacterManager.Instance.OnDealDamage += FeedBack;

        if (TryGetComponent(out BurningDamageBuffer burn))
        {
            burningDamage *= burn.GetDamage();
        }
    }

    private void Cast()
    {
        Debug.Log("Player got hit, heal!");
    }

    private void SetBurningDmage(float multiplier)
    {
        burningDamage *= multiplier;
    }

    private void FeedBack(Transform enemy, float damage)
    {
        var target = enemy.GetComponent<Enemy>();
        if (target.GetIsBurning())
        {
            target.SetIsBurning(false);
            return;
        }
        target.StartCoroutine(Burning(enemy, burningPeriod, burningDamage));
    }

    private IEnumerator Burning(Transform target, float periodTime, float dmg)
    {
        var enemy = target.GetComponent<Enemy>();
        enemy.SetIsBurning(true);
        var countDown = 5f;
        while (countDown >= 0 && enemy.GetIsBurning())
        {
            countDown -= periodTime;
            enemy.RevieveDamage(dmg);
            yield return new WaitForSeconds(periodTime);
        }
    }
}