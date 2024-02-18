using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class TaskPatrol : Node
{
    private Enemy enemy;

    private bool waiting = false;
    private float waitCounter = 0;
    private float waitTime = 5;

    public TaskPatrol(Enemy _enemy)
    {
        enemy = _enemy;
    }

    public override NodeState Evaluate()
    {
        if (enemy.IsDestinationReached())
        {
            enemy.StartPatrol();
        }

        state = NodeState.RUNNING;
        return state;
    }
}
