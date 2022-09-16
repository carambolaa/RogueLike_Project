using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IBurnable
{
    private float baseHp = 200;
    private float currentHp;
    private bool isBurning;
    private float goldHolding;
    private List<Coroutine> c = new List<Coroutine>();
    [SerializeField] private float burningLength = 15;
    [Range(0.1f, 10f)] [SerializeField] private float burningPeriodLength = 0.2f;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Transform canvas;
    private Transform mainCamera;
    private Transform player;
    private bool isDead = false;

    private void Awake()
    {
        goldHolding = Random.Range(50, 150);
        currentHp = baseHp;
        mainCamera = Camera.main.transform;
        player = GameObject.Find("Player").transform;
    }

    private void Start()
    {
        UpdateHealthBar();
    }

    private void LateUpdate()
    {

    }

    public float GetHealPercentage()
    {
        return currentHp / baseHp;
    }

    public void RevieveDamage(float dmg)
    {
        //Debug.Log(dmg);
        currentHp -= dmg;
        UpdateHealthBar();
        if (currentHp <= 0)
        {
            Die();
        }
        //Debug.Log(currentHp);
    }

    private void UpdateHealthBar()
    {
        healthBar.value = currentHp / baseHp;
    }




    public void Die()
    {
        if(!isDead)
        {
            isDead = true;
            CharacterManager.Instance.AddGold(goldHolding);
            Destroy(this.gameObject);
            CharacterManager.Instance.EnemyKilled(transform.position);
        }
    }

    public void StartBurning(float damage)
    {
        c.Add(StartCoroutine(Burn(damage)));
    }

    public void StopBurning()
    {
        if(c.Count > 0)
        {
            StopCoroutine(c[0]);
            c.RemoveAt(0);
        }
    }

    public IEnumerator Burn(float damage)
    {
        float length = burningLength;
        if(burningPeriodLength <= 0)
        {
            burningPeriodLength = 0.2f;
        }
        if (length <= 0)
        {
            length = 15;
        }
        while (length >= 0)
        {
            RevieveDamage(damage);
            yield return new WaitForSeconds(burningPeriodLength);
            length--;
        }
        StopBurning();
    }
}
