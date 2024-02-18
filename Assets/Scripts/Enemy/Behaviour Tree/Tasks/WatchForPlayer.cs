using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;


public class WatchForPlayer : Node
{
    private Enemy enemy;

    public WatchForPlayer(Enemy _enemy)
    {
        enemy = _enemy;
    }

    public override NodeState Evaluate()
    {
        if (enemy.IsPlayerInSight)
        {
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}
