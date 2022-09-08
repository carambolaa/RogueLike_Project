using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private float baseHp = 200;
    private float currentHp;
    private bool isBurning;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Transform canvas;
    private Transform mainCamera;
    private Transform player;

    private void Awake()
    {
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
        canvas.LookAt(transform.position + -mainCamera.forward);
        var currentSize = 0.005f * Vector3.Distance(player.position, this.transform.position)/6f;
        healthBar.transform.localScale = new Vector3(currentSize, currentSize * 1.5f, currentSize);
    }

    public float GetHealPercentage()
    {
        return currentHp / baseHp;
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
        CharacterManager.Instance.EnemyKilled(transform.position);
        Destroy(this.gameObject);
    }
}
