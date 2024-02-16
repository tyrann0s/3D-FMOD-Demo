using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : MonoBehaviour
{
    private Node rootNode;
    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponent<Enemy>();

        rootNode = new SelectorNode(
            new SequenceNode(
                new ConditionNode(() => TestCondition()),
                new ActionNode(() => TestAction_1())
                ),
            new ActionNode(() => TestAction_2())
            );
    }
    private void Update()
    {
        rootNode.Execute();
    }

    public bool TestCondition()
    {
        return enemy.IsPlayerInSight;
    }

    private void TestAction_1()
    {
        Debug.Log("Test Action 1");
    }

    private void TestAction_2()
    {
        Debug.Log("Test Action 2");
    }
}
