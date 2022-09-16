using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : BaseState
{
    private MovementSM _sm;

    private Vector3 direction;
    private Quaternion lookRotation;
    private float RotationSpeed = 7;

    public Attack(MovementSM stateMachine) : base("Attack", stateMachine)
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

        if (Vector3.Distance(_sm.player.position, _sm.transform.position) > 13)
        {
            stateMachine.ChangeState(_sm.chaseState);
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        direction = (_sm.player.position - _sm.transform.position).normalized;

        //create the rotation we need to be in to look at the target
        lookRotation = Quaternion.LookRotation(direction);

        //rotate us over time according to speed until we are in the required rotation
        _sm.transform.rotation = Quaternion.Slerp(_sm.transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
    }
}
