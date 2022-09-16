using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : BaseState
{
    private MovementSM _sm;
    private Vector3 currentTarget;
    private Vector3 direction;
    private Quaternion lookRotation;
    private float RotationSpeed = 7;
    private bool isCooling;
    private float cooldown;

    public Patrol(MovementSM stateMachine) : base("Patrol", stateMachine)
    {
        _sm = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        currentTarget = _sm.transform.position;
        _sm.rb.velocity = Vector3.zero;
        _sm.rb.angularVelocity = Vector3.zero;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (Vector3.Distance(_sm.player.position, _sm.transform.position) <= 20)
        {
            stateMachine.ChangeState(_sm.chaseState);
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        //Debug.Log(Vector3.Distance(currentTarget, _sm.transform.position));
        //Debug.Log(cooldown);
        if (Vector3.Distance(currentTarget, _sm.transform.position) < 0.5f && !isCooling)
        {
            cooldown = 0;
            isCooling = true;
            currentTarget = new Vector3(Random.Range(-6, 6), 0, Random.Range(-6, 6)) + currentTarget;
        }
        if (isCooling == true)
        {
            _sm.rb.velocity = Vector3.zero;
            _sm.rb.angularVelocity = Vector3.zero;
            cooldown += Time.deltaTime;
        }
        if (cooldown >= 1.5f)
        {
            isCooling = false;
        }
        if (isCooling == false)
        {
            _sm.rb.velocity = _sm.transform.forward * _sm.moveSpeed;
            //find the vector pointing from our position to the target
            direction = (currentTarget - _sm.transform.position).normalized;

            //create the rotation we need to be in to look at the target
            lookRotation = Quaternion.LookRotation(direction);

            //rotate us over time according to speed until we are in the required rotation
            _sm.transform.rotation = Quaternion.Slerp(_sm.transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
        }
    }
}