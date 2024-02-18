using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingToSound : Node
{
    private Enemy enemy;

    public MovingToSound(Enemy _enemy)
    {
        enemy = _enemy;
    }

    public override NodeState Evaluate()
    {
        enemy.MoveToSound();

        if (enemy.IsDestinationReached())
        {
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.RUNNING;
        return state;
    }
}
