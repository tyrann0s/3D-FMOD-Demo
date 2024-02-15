using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionNode : Node
{
    private readonly Func<bool> condition;

    public ConditionNode(Func<bool> condition)
    {
        this.condition = condition;
    }

    public override bool Execute()
    {
        return condition();
    }
}
