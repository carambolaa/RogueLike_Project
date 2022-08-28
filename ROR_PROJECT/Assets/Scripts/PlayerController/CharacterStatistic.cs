using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class CharacterStatistic : ScriptableObject
{
    public float walkSpeed;
    public float runSpeed;
    public float damage;
    public float Hp;
}
