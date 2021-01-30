using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDie : NPCstate
{

    public StateDie(StateMachine sm, NPCs b) : base(sm, b)
    {

    }
    public override void Awake()
    {
        base.Awake();
    }
    public override void Execute()
    {
        base.Execute();
        _npc.Die();
    }
    public override void Sleep()
    {
        base.Sleep();
    }
}
