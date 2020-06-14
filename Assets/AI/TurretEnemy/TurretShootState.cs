using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateEnemy;

public class TurretShootState : State<TurrentEnemy>
{
    private static TurretShootState _instance;
    Vector3 direction = Vector3.zero;
    private float timer;

    private TurretShootState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }
    public static TurretShootState Instance
    {
        get
        {
            if (_instance == null)
            {
                new TurretShootState();
            }
            return _instance;
        }
    }

    public override void EnterState(TurrentEnemy _owner)
    {
        Debug.Log("entering shoot State");
    }

    public override void ExitState(TurrentEnemy _owner)
    {
        Debug.Log("Exiting shoot State");
    }

    public override void UpdateState(TurrentEnemy _owner)
    {
        //checking if player is in radius
        if (_owner.Player == null)
        {
            //if no go to idle state
            _owner.stateMachine.ChangeState(TurretIdleEnemy.Instance);
        }
        else
        {
            //if yes rotate to look at player
            direction = _owner.Player.transform.position - _owner.transform.position;
            _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation, Quaternion.LookRotation(direction), 4f * Time.deltaTime);
        }
        //shoot every x seconds
        if(timer <= Time.time)
        {
            _owner.ShootBullet();
            timer = Time.time + _owner.secondsBetweenShots;
            
        }
    }
}
