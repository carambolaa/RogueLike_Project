using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float countDown;
    private float Hp = 100;
    private float currentHp;

    private void Awake()
    {
        currentHp = Hp;
    }

    private void Start()
    {

    }

    public void StartBurning(float periodTime, float dmg)
    {
        StartCoroutine(Burning(periodTime, dmg));
    }

    private void RevieveDamage(float dmg)
    {
        currentHp -= dmg;
        Debug.Log(currentHp);
    }

    private IEnumerator Burning(float periodTime, float dmg)
    {
        countDown = 10f;
        while(countDown >= 0)
        {
            countDown -= periodTime;
            RevieveDamage(dmg);
            yield return new WaitForSeconds(periodTime);
        }
    }
}
