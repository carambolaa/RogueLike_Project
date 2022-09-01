using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningDamageBuffer : Item
{
    private float burningDamageMultiplier = 3f;

    protected override void ItemNumberUpdate()
    {
        base.ItemNumberUpdate();
        CharacterManager.Instance.SetBurningDamageMultiplier(burningDamageMultiplier * itemNumber);
    }
}
