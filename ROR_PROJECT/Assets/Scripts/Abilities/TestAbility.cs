using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestAbility : Ability
{
    protected override void Awake()
    {
        base.Awake();
        m_PlayerAction.performed += Cast;
    }

    private void Cast(InputAction.CallbackContext context)
    {
        Debug.Log("+10");
    }
}
