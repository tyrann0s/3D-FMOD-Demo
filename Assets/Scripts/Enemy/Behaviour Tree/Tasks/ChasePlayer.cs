using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : Node
{
    private Enemy enemy;
    public ChasePlayer(Enemy _enemy)
    {
        enemy = _enemy;
    }

    public override NodeState Evaluate()
    {
        enemy.ChasePlayer();

        if (enemy.IsDestinationReached())
        {
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.RUNNING;
        return state;
    }
}
