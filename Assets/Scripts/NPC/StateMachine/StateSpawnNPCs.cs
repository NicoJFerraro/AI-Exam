using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSpawnNPCs : NPCstate
{
    public StateSpawnNPCs(StateMachine sm, Boss b) : base(sm, b)
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
            var _instatiate = GameObject.Instantiate(_npc.GetComponent<Boss>()._spawneado);
            _instatiate.transform.position = _npc.spawnpoint.position;
            _instatiate.target = _npc.GetComponent<Boss>().target;
            _instatiate.leader = _npc.gameObject.GetComponent<Boss>();
            _instatiate.team = _npc.team;
            _instatiate.myRoom = _npc.myRoom;
            _npc.actionTimer = _npc.GetComponent<Boss>().spawnCooldown;
            _npc.GetComponent<Boss>().spawned.Add(_instatiate);
        }
    }
    public override void Sleep()
    {
        base.Sleep();
    }
}