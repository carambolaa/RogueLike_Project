using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicItem : Item
{
    private float burningPeriod = 1f;
    private float burningDamage = 1;
    private float burningDamageMultiplier;
    private Coroutine currentCoroutine;

    protected override void Start()
    {
        base.Start();
        CharacterManager.Instance.OnRecieveDamage += Cast;
        CharacterManager.Instance.OnDealDamage += FeedBack;
    }

    private void Cast()
    {
        Debug.Log("Player got hit, heal!");
    }

    private void FeedBack(Transform enemy, float damage)
    {
        var target = enemy.GetComponent<Enemy>();
        if (currentCoroutine != null)
        {
            target.StopCoroutine(currentCoroutine);
        }
        burningDamageMultiplier = CharacterManager.Instance.GetBurningDamageMultiplier();
        var currentBurningDamage = burningDamage;
        if (burningDamageMultiplier != 0)
        {
            currentBurningDamage *= burningDamageMultiplier;
        }
        currentCoroutine = target.StartCoroutine(Burning(enemy, burningPeriod, currentBurningDamage));
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