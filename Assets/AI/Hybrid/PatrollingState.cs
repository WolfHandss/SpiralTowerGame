
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateEnemy;
using System;

public class PatrollingState : State<HybridEnemy>
{
    private static PatrollingState _instance;
    private GameObject PEnemy;

    private int CurrentWP;

    private bool reverse = false;

    private PatrollingState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }
    public static PatrollingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new PatrollingState();
            }
            return _instance;
        }
    }

    public override void EnterState(HybridEnemy _owner)
    {
        Debug.Log("Patrolling State");
        PEnemy = _owner.gameObject;
        CurrentWP = 0;
    }

    public override void ExitState(HybridEnemy _owner)
    {
        Debug.Log("Exiting Patrolling State");
    }

    public override void UpdateState(HybridEnemy _owner)
    {
        //cast a line to forward of the enemy
        Vector3 lineStart = _owner.transform.position;
        Vector3 vectorToSearch = new Vector3(lineStart.x, lineStart.y, lineStart.z) + (_owner.transform.forward / 1.5f);

        RaycastHit hit;
        Debug.DrawLine(lineStart, vectorToSearch);
        //check if its hitting anything
        if (Physics.Linecast(lineStart, vectorToSearch, out hit))
        {
            //if its hitting an object without the tag of player reverse the rotation of going through waypoints
            if (hit.transform.gameObject.tag != ("Player"))
            {
                reverse = !reverse;
                if (reverse)
                {
                    CurrentWP--;
                    if (CurrentWP < 0)
                    {
                        CurrentWP = _owner.PatrolPoints.Length - 1;
                    }
                }
                else
                {
                    CurrentWP++;
                    if (CurrentWP >= _owner.PatrolPoints.Length)
                    {
                        CurrentWP = 0;
                    }
                }
            }

        }

        if (_owner.PatrolPoints.Length == 0) return;

        //checked if reached the waypoint if yes set the next way point
        if (Vector3.Distance(_owner.PatrolPoints[CurrentWP].transform.position, PEnemy.transform.position) < 1.0f)
        {
            if (!reverse)
                CurrentWP++;
            else
                CurrentWP--;
            if (CurrentWP >= _owner.PatrolPoints.Length)
            {
                CurrentWP = 0;
            }
            if (CurrentWP < 0)
            {
                CurrentWP = _owner.PatrolPoints.Length - 1;
            }
        }

        //rotate towards next point
        var direction = _owner.PatrolPoints[CurrentWP].transform.position - PEnemy.transform.position;
        PEnemy.transform.rotation = Quaternion.Slerp(PEnemy.transform.rotation, Quaternion.LookRotation(direction), 4f * Time.deltaTime);
        //move forward
        PEnemy.transform.Translate(0, 0, Time.deltaTime * 4f);

        //if player in close distance go to follow state
        if (Vector3.Distance(_owner.PlayerPosition.position, _owner.transform.position) < 10.0f)
        {
            _owner.stateMachine.ChangeState(FollowState.Instance);
        }
    }
}
