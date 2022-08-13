using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestAbility2 : Abilities
{
    [SerializeField] private float dashForce;
    [SerializeField] private Transform orientation;
    private float elapsed;

    private void Start()
    {
        cooldown = 3;
        isCooling = false;
    }

    protected override void Awake()
    {
        base.Awake();
        m_PlayerAction.performed += Cast;
    }

    private void Update()
    {
        CooldownTimer();
        Ability_UI(elapsed/3);
    }


    //IEnumerator CooldownTimer()
    //{
    //    while(isCooling)
    //    {
    //        yield return new WaitForSeconds(cooldown);
    //        isCooling = false;
    //    }
    //    yield return null;
    //}

    private void CooldownTimer()
    {
        if(isCooling)
        {
            elapsed += Time.deltaTime;
            if (elapsed >= cooldown)
            {
                elapsed = 0;
                isCooling = false;
            }

        }
    }

    private void Cast(InputAction.CallbackContext context)
    {
        if(!isCooling)
        {
            isCooling = true;
            elapsed = 0;
            //StartCoroutine("CooldownTimer");
            //dash
            Vector3 dashDirection = orientation.transform.forward;
            var temp = GetComponent<Rigidbody>();
            temp.AddForce(dashDirection * dashForce, ForceMode.Impulse);
        }
    }
}
