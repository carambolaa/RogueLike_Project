using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestAbility : Abilities
{
    protected override void Awake()
    {
        base.Awake();
        m_PlayerAction.performed += Cast;
    }

    private void Cast(InputAction.CallbackContext context)
    {
        GetComponent<PlayerMovement>().BroadcastMessage("UpdateHP", 10);
        Debug.Log(GetComponent<PlayerMovement>().GetHP());
    }
}
