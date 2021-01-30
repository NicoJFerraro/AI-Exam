using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFleeNPCs : NPCstate{

    public StateFleeNPCs(StateMachine sm, NPCs b) : base(sm, b)
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
        _npc.dir = _npc.vectAvoidance + _npc.GetNode(_npc.GetPathToFlee);

        _npc.transform.forward = Vector3.Slerp(_npc.transform.forward, _npc.dir.normalized, _npc.rotationSpeed * Time.deltaTime);
        _npc.transform.position += _npc.transform.forward * Time.deltaTime * _npc.speed * 1.2f;
    }
    public override void Sleep()
    {
        base.Sleep();
        _npc.ModifieHP(-_npc.heal);
    }
}
