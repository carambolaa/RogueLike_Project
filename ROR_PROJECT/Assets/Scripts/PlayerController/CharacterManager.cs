using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }
    private Dictionary<string, int> m_Inventory = new Dictionary<string, int>();
    private Image healthBar;
    private TextMeshProUGUI GoldHolding;

    //properties
    private string charater;
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
    private bool isDashing;

    //Events
    public event Action OnRecieveDamage;
    public event Action OnRecieveHealing;
    public event Action<Transform, float> OnDealDamage;
    public event Action<Vector3> OnEnemyKill;
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

        GoldHolding = GameObject.Find("Gold").GetComponent<TextMeshProUGUI>();
        healthBar = GameObject.Find("Health").GetComponent<Image>();
        charater = GameManager.instance?.GetCharacter();
        SetAbilities();
    }

    private void Start()
    {
        ResetBasicValues();
    }

    public void SetAbilities()
    {
        if(charater == "Commander")
        {
            Debug.Log("I'm commander");
        }
        else if(charater == "Ashe")
        {
            Debug.Log("I'm Ashe");
        }
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

    public void EnemyKilled(Vector3 vec3)
    {
        OnEnemyKill?.Invoke(vec3);
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
        healthBar.fillAmount = currentHp / baseHp;
        OnRecieveDamage?.Invoke();
    }

    public void RecieveHealing(float amount)
    {
        this.currentHp += amount;
        healthBar.fillAmount = currentHp / baseHp;
        OnRecieveHealing?.Invoke();
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
        if(burningDamageMultiplier <= 0)
        {
            burningDamageMultiplier = 1;
        }
        return burningDamageMultiplier;
    }

    public float GetPlayerCurrentDamage()
    {
        return playerCurrentDamage;
    }

    public float GetGold()
    {
        return Gold;
    }

    public void AddGold(float amount)
    {
        Gold += amount;
        GoldHolding.text = "Gold : " + Gold;
    }

    public void MinusGold(float amount)
    {
        Gold -= amount;
        GoldHolding.text = "Gold : " + Gold;
    }

    public void SetIsDashing(bool bo)
    {
        isDashing = bo;
    }

    public bool GetIsDashing()
    {
        return isDashing;
    }
}
