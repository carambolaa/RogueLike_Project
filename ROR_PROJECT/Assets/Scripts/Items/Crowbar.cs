using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowbar : Item
{
    //item's damage multiplier
    private const float extraDamageMultiplier = 0.75f;

    protected override void ItemNumberUpdate()
    {
        base.ItemNumberUpdate();
        CharacterManager.Instance.SetExtraDamage(extraDamageMultiplier * itemNumber);
    }
}
