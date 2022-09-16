using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : BaseState
{
    private MovementSM _sm;

    private Vector3 direction;
    private Quaternion lookRotation;
    private float RotationSpeed = 7;

    public Chase(MovementSM stateMachine) : base("Chase", stateMachine)
    {
        _sm = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        _sm.rb.velocity = Vector3.zero;
        _sm.rb.angularVelocity = Vector3.zero;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (Vector3.Distance(_sm.player.position, _sm.transform.position) <= 13)
        {
            stateMachine.ChangeState(_sm.attackState);
        }
        if (Vector3.Distance(_sm.player.position, _sm.transform.position) > 20)
        {
            stateMachine.ChangeState(_sm.patrolState);
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        _sm.rb.velocity = _sm.transform.forward * _sm.moveSpeed;
        //find the vector pointing from our position to the target
        direction = (_sm.player.position - _sm.transform.position).normalized;

        //create the rotation we need to be in to look at the target
        lookRotation = Quaternion.LookRotation(direction);

        //rotate us over time according to speed until we are in the required rotation
        _sm.transform.rotation = Quaternion.Slerp(_sm.transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
    }
}
