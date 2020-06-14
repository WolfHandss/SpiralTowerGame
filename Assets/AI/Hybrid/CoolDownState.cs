using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateEnemy;
using System;

public class CoolDownState : State<HybridEnemy>
{
    private static CoolDownState _instance;

    private CoolDownState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }
    public static CoolDownState Instance
    {
        get
        {
            if (_instance == null)
            {
                new CoolDownState();
            }
            return _instance;
        }
    }

    public override void EnterState(HybridEnemy _owner)
    {
        Debug.Log("Entering CoolDown State");
        _owner.calming();
    }

    public override void ExitState(HybridEnemy _owner)
    {
        Debug.Log("Exiting CoolDown State");
    }

    public override void UpdateState(HybridEnemy _owner)
    {

    }
}
