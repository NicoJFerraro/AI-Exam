using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCstate : State
{
    protected NPCs _npc;

    public NPCstate(StateMachine sm, NPCs n) : base(sm)
    {
        _npc = n;
    }
}
