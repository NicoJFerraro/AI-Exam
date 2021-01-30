using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSeekNPCs : NPCstate {

    public StateSeekNPCs(StateMachine sm, NPCs n) : base(sm, n)
    {

    }
    public override void Awake()
    {
        base.Awake();
        _npc.donePath = false;
    }
    public override void Execute()
    {
        base.Execute();
        _npc.GetObstacles();
        _npc.closerOb = _npc.GetCloserOb();
        _npc.vectAvoidance = new Vector3(_npc.getObstacleAvoidance().x, 0, _npc.getObstacleAvoidance().z) * _npc.avoidWeight;
        _npc.dir = _npc.vectAvoidance + _npc.GetNode(_npc.GetPathToTarget);


        _npc.transform.forward = Vector3.Slerp(_npc.transform.forward, _npc.dir, _npc.rotationSpeed * Time.deltaTime);
        _npc.transform.position += _npc.transform.forward * Time.deltaTime * _npc.speed;
    }
    public override void Sleep()
    {
        base.Sleep();
    }
}
