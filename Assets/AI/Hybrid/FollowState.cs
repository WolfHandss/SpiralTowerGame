using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateEnemy;
using System;

public class FollowState : State<HybridEnemy>
{
    private static FollowState _instance;


    private FollowState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }
    public static FollowState Instance
    {
        get
        {
            if (_instance == null)
            {
                new FollowState();
            }
            return _instance;
        }
    }

    public override void EnterState(HybridEnemy _owner)
    {
        Debug.Log("Entering Follow State");
    }

    public override void ExitState(HybridEnemy _owner)
    {
        Debug.Log("Exiting Follow State");
    }

    public override void UpdateState(HybridEnemy _owner)
    {
        //if player is in proximity
        if (_owner.distance <= _owner.stopDistance && _owner.distance > 1)
        {
            //set target
            Vector3 targetPosition = _owner.PlayerPosition.position;
            targetPosition.y = _owner.transform.position.y;
            //rotate and move towards player
            _owner.transform.LookAt(targetPosition);
            _owner.transform.position += _owner.transform.forward * _owner.speed * Time.deltaTime;
        }
        //if hit player go to cooldown state
        else if(_owner.distance <= 1f)
        {
            _owner.stateMachine.ChangeState(CoolDownState.Instance);
        }
        //if player is out of range go to patrolling
        else if (_owner.distance > _owner.stopDistance)
        {
            _owner.stateMachine.ChangeState(PatrollingState.Instance);
        }
    }
}