using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearForPlayer : Node
{
    private Enemy enemy;

    public HearForPlayer(Enemy _enemy)
    {
        enemy = _enemy;
    }

    public override NodeState Evaluate()
    {
        if (enemy.IsHearingSomething)
        {
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}
