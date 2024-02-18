using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class EnemyBT : BehaviourTree.Tree
{
    private Enemy enemy;

    protected override void Start()
    {
        enemy = GetComponent<Enemy>();
        base.Start();
    }

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new WatchForPlayer(enemy),
                new ChasePlayer(enemy)
            }),
            new Sequence(new List<Node>
            {
                new HearForPlayer(enemy),
                new MovingToSound(enemy)
            }),
            new TaskPatrol(enemy)
        });

        return root;
    }
}
