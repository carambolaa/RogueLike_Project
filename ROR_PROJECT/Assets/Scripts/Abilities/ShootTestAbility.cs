using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootTestAbility : Ability
{
    [Header("References")]
    private Transform cam;
    private Transform attackPoint;
    [SerializeField]
    private GameObject throwPrefab;

    [Header("Settings")]
    [SerializeField]
    private int totalThrows;
    [SerializeField]
    private float throwCooldown;

    [Header("Throwing")]
    [SerializeField]
    private float throwForce;
    [SerializeField]
    private float throwUpwardForce;
    [SerializeField]
    private bool readyToThrow;

    protected override void Awake()
    {
        base.Awake();
        m_PlayerAction.performed += Throw;
        cam = GameObject.Find("Main Camera").transform;
        attackPoint = GameObject.Find("AttackPoint").transform;
    }

    private void Start()
    {
        readyToThrow = true;
    }

    private void Update()
    {
        CooldownTimer();
        Ability_UI();
    }

    private void Throw(InputAction.CallbackContext context)
    {
        if(readyToThrow && totalThrows > 0)
        {
            isCooling = true;
            readyToThrow = false;
            //instantiate
            GameObject projectile = Instantiate(throwPrefab, attackPoint.position, cam.rotation);
            //assign shooter
            if (projectile.GetComponent<Bullet>())
            {
                projectile.GetComponent<Bullet>().SetShooter(transform);
            }
            if(projectile.GetComponent<PenetrateBullet>())
            {
                Debug.Log("ok");
                projectile.GetComponent<PenetrateBullet>().SetShooter(transform);
            }
            //get rigidbody
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            //add force
            Vector3 forceToAdd = cam.transform.forward * throwForce + transform.up * throwUpwardForce;

            projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
            totalThrows--;

            Invoke(nameof(ResetThrow), throwCooldown);
        }
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }
}
