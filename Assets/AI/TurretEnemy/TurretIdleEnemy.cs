using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateEnemy;

public class TurretIdleEnemy : State<TurrentEnemy>
{
    private static TurretIdleEnemy _instance;
    private Quaternion randomRotation;

    private TurretIdleEnemy()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }
    public static TurretIdleEnemy Instance
    {
        get
        {
            if (_instance == null)
            {
                new TurretIdleEnemy();
            }
            return _instance;
        }
    }

    public override void EnterState(TurrentEnemy _owner)
    {
        Debug.Log("Entering idle State");
        //random y value
        randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

    }

    public override void ExitState(TurrentEnemy _owner)
    {
        Debug.Log("Exiting idle State");
    }

    public override void UpdateState(TurrentEnemy _owner)
    {
        //if player in proximity go to shoot state
        if (_owner.Player != null)
        {
            _owner.stateMachine.ChangeState(TurretShootState.Instance);
        }

        //rotate to new rotation
        _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation, randomRotation, 8f * Time.deltaTime);

        //if finished rotation get new direction
        if(randomRotation == _owner.transform.rotation)
        {
            randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        }
    }
}
