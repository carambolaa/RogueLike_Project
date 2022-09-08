using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicItem : Item
{
    private float burningPeriod = 1f;
    private float burningDamage;
    private float burningDamageMultiplier;

    protected override void Start()
    {
        base.Start();
        CharacterManager.Instance.OnRecieveDamage += Cast;
        CharacterManager.Instance.OnEnemyKill += FeedBack;
    }

    private void Cast()
    {
        Debug.Log("Player got hit, heal!");
    }

    private void FeedBack(Vector3 vec3)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject go in targets)
        {
            float distance = Vector3.Distance(go.transform.position, vec3);
            if(distance < 12)
            {
                var target = go.GetComponent<Enemy>();
                //target.StopAllCoroutines();
                burningDamageMultiplier = CharacterManager.Instance.GetBurningDamageMultiplier();
                burningDamage = CharacterManager.Instance.GetPlayerCurrentDamage() * 1.5f * itemNumber;
                var currentBurningDamage = burningDamage;
                if (burningDamageMultiplier != 0)
                {
                    currentBurningDamage *= burningDamageMultiplier;
                }
                StartCoroutine(Burning(go.transform, burningPeriod, currentBurningDamage));
            }
        }
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