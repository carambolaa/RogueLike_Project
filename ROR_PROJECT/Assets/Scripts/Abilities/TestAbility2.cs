using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestAbility2 : Ability
{
    [SerializeField] private float dashForce;
    [SerializeField] private Transform orientation;

    protected override void Awake()
    {
        base.Awake();
        m_PlayerAction.performed += Cast;
    }

    private void Start()
    {
        isCooling = false;
    }

    private void Update()
    {
        CooldownTimer();
        Ability_UI();
    }

    private void ResetDash()
    {
        CharacterManager.Instance.SetIsDashing(false);
    }

    private void Cast(InputAction.CallbackContext context)
    {
        if(!isCooling)
        {
            CharacterManager.Instance.SetIsDashing(true);
            isCooling = true;
            elapsed = 0;
            //dash
            Vector3 dashDirection = orientation.transform.forward;
            var temp = GetComponent<Rigidbody>();
            temp.AddForce(dashDirection * dashForce, ForceMode.Impulse);
            Invoke("ResetDash", 0.1f);
        }
    }
}
