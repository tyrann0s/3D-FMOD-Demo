using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : Node
{
    private readonly List<Node> children;

    public SelectorNode(params Node[] nodes)
    {
        children = new List<Node>(nodes);
    }

    public override bool Execute()
    {
        foreach (Node node in children)
        {
            if (node.Execute())
            {
                return true;
            }
        }

        return false;
    }
}
