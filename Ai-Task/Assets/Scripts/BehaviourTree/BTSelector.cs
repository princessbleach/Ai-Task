using System.Collections.Generic;

public class BTSelector : BTNode
{
    private List<BTNode> children = new List<BTNode>();

    public void AddChild(BTNode child)
    {
        children.Add(child);
    }

    public override NodeState Tick()
    {
        foreach (BTNode child in children)
        {
            NodeState result = child.Tick();

            if (result != NodeState.Failure)
                return result;
        }

        return NodeState.Failure;
    }
}