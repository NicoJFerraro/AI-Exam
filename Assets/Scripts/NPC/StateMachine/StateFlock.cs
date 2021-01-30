using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateFlock : NPCstate
{

    public StateFlock(StateMachine sm, FlockingNPCs b) : base(sm, b)
    {

    }
    public override void Awake()
    {
        base.Awake();
    }
    public override void Execute()
    {
        base.Execute();
        FlockingNPCs sa = _npc.GetComponent<FlockingNPCs>();
        sa.GetBoidEnemiesAndObstacles();
        sa.Flock();

    }
    public override void Sleep()
    {
        base.Sleep();
    }
}

