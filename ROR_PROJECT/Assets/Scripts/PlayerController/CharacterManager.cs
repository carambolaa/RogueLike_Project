using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }
    private Dictionary<string, int> m_Inventory = new Dictionary<string, int>();

    //properties
    private float baseHp;
    private float currentHp;
    private float Xp;
    private float Gold;
    private float attackSpeed;
    private float playerBaseDamage;
    private float playerCurrentDamage;
    private float burningDamageMultiplier;
    private float extraDamageMultiplier; //crowbar's extra damage
    private float buffDamageMultiplier; //crystal's buff damage

    //Events
    public event Action OnRecieveDamage;
    public event Action<Transform, float> OnDealDamage;
    public event Action<string> OnAddItem; //add new item
    public event Action<string> OnPlusItem; //add one item number
    public event Action<string> OnMinusItem; // minus one item number

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        ResetBasicValues();
    }

    public void ResetBasicValues()
    {
        baseHp = 200;
        currentHp = baseHp;
        playerBaseDamage = 10;
        playerCurrentDamage = playerBaseDamage;
        Xp = 0;
        Gold = 0;
        attackSpeed = 10;
    }

    public void AddItem(string itemName)
    {
        Type t = Type.GetType(itemName);
        if (t == null)
        {
            Debug.LogWarning("Invalide Item Name");
            return;
        }
        if (m_Inventory.ContainsKey(itemName))
        {
            m_Inventory[itemName]++;
            OnPlusItem?.Invoke(itemName);
        }
        else
        {
            m_Inventory.Add(itemName, 1);
            OnAddItem?.Invoke(itemName);
        }
    }

    public void MinusItem(string itemName)
    {
        if(!m_Inventory.ContainsKey(itemName))
        {
            Debug.LogWarning("Doesn't contain this item, can't remove.");
            return;
        }
        m_Inventory[itemName]--;
        OnMinusItem?.Invoke(itemName);
    }

    public float CalculateItemDamageBuff(Transform target, float currentDamage)
    {
        if(target.GetComponent<Enemy>().GetHealPercentage() >= 0.9f)
        {
            currentDamage += currentDamage * extraDamageMultiplier;
            Debug.Log(currentDamage);
        }
        if(buffDamageMultiplier != 0)
        {
            if (Vector3.Distance(target.position, this.transform.position) < 13)
            {
                currentDamage *= buffDamageMultiplier;
            }
        }
        return currentDamage;
    }

    public void DealDamage(Transform target, float abilityDamageMultiplier)
    {
        //check buff item
        var currentDamage = playerCurrentDamage * abilityDamageMultiplier;
        currentDamage = CalculateItemDamageBuff(target, currentDamage);
        //calculate damage.deal damage
        target.GetComponent<Enemy>().RevieveDamage(currentDamage);
        OnDealDamage?.Invoke(target, currentDamage);
    }

    public void RecieveDamage(float damage)
    {
        this.currentHp -= damage;
        OnRecieveDamage?.Invoke();
    }

    public void SetExtraDamage(float extraDamageMultiplier)
    {
        this.extraDamageMultiplier = extraDamageMultiplier;
    }

    public void SetBurningDamageMultiplier(float burningDamageMultiplier)
    {
        this.burningDamageMultiplier = burningDamageMultiplier;
    }

    public float GetBurningDamageMultiplier()
    {
        return burningDamageMultiplier;
    }
}
