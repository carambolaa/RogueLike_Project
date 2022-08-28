using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float countDown;
    private float Hp = 100;
    private float currentHp;
    private bool isBurning;

    private void Awake()
    {
        currentHp = Hp;
    }

    private void Start()
    {

    }

    public float GetHealPercentage()
    {
        return currentHp / Hp;
    }

    public bool GetIsBurning()
    {
        return isBurning;
    }

    public void SetIsBurning(bool bo)
    {
        isBurning = bo;
    }

    public void RevieveDamage(float dmg)
    {
        currentHp -= dmg;
        Debug.Log(currentHp);
    }
}
