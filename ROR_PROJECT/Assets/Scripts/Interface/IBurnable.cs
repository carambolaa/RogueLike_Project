using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBurnable
{
    void StartBurning(float damage);
    void StopBurning();
    IEnumerator Burn(float damage);
}
