using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node root = null;

        protected virtual void Start()
        {
            root = SetupTree();
        }

        private void Update()
        {
            if (root != null) root.Evaluate();
        }

        protected abstract Node SetupTree();
    }
}


