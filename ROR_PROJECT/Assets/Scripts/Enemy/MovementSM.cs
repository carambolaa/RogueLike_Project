using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSM : StateMachine
{
    [HideInInspector]
    public Chase chaseState;
    [HideInInspector]
    public Patrol patrolState;
    [HideInInspector]
    public Attack attackState;

    public Rigidbody rb;
    public Transform player;
    public float moveSpeed = 4;
    public GameObject bulletPrefab;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").transform;
        chaseState = new Chase(this);
        patrolState = new Patrol(this);
        attackState = new Attack(this);
    }

    protected override BaseState GetInitialState()
    {
        return patrolState;
    }

    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 80, ForceMode.Impulse);
    }
}
