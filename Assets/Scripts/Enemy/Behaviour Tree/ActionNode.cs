using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : Node
{
    private readonly Action action;

    public ActionNode(Action action)
    {
        this.action = action;
    }

    public override bool Execute()
    {
        action();
        return true;
    }
}
