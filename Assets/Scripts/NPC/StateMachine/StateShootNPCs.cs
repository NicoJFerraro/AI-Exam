using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateShootNPCs : NPCstate {

    public StateShootNPCs(StateMachine sm, NPCs b) : base(sm, b)
    {

    }
    public override void Awake()
    {
        base.Awake();
        _npc.actionTimer = 0;
    }
    public override void Execute()
    {
        base.Execute();
        if (_npc.actionTimer <= 0)
        {
            BalaEnemiga _instatiate = GameObject.Instantiate(_npc.bullet);
            _instatiate.transform.position = _npc.spawnpoint.position;
            Vector3 dir = _npc.transform.forward;
            if (_npc.target)
            _instatiate.transform.forward = _npc.transform.forward = dir;
            _instatiate.target = _npc.target;
            _npc.actionTimer = _npc.shootCoolDown;
        }
    }
    public override void Sleep()
    {
        base.Sleep();
    }
}
