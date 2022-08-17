using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicItem : MonoBehaviour
{
    private float burningPeriod = .5f;
    private float damage = 1;

    private void Start()
    {
        PlayerMovement.instance.OnRecieveDamage += Cast;
        PlayerMovement.instance.OnDealDamage += FeedBack;
    }

    private void Cast()
    {
        Debug.Log("Player got hit, heal!");
    }

    private void FeedBack(Transform enemy)
    {
        Debug.Log(enemy + "got hit");
        enemy.GetComponent<Enemy>().StartBurning(burningPeriod, damage);
    }
}