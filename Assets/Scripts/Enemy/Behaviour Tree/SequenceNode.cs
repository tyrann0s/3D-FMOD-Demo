using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : Node
{
    private readonly List<Node> children;

    public SequenceNode(params Node[] nodes)
    {
        children = new List<Node>(nodes);
    }

    public override bool Execute()
    {
        foreach (Node node in children)
        {
            if (!node.Execute())
            {
                return false;
            }
        }

        return true;
    }
}
