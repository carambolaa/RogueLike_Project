using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowbar : IItem
{
    private float damageMultiplierBase = 1.75f;
    private float CalculateExtraDamage(float damage)
    {
        return damageMultiplierBase * damage;
    }
}
