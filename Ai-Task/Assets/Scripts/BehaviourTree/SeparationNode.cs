using UnityEngine;

public class SeparationNode : BTNode
{
    private Transform npc;
    private Rigidbody2D rb;
    private float radius;
    private float speed;

    public SeparationNode(Transform npc, Rigidbody2D rb, float radius, float speed)
    {
        this.npc = npc;
        this.rb = rb;
        this.radius = radius;
        this.speed = speed;
    }

    public override NodeState Tick()
    {
        Collider2D[] nearby = Physics2D.OverlapCircleAll(npc.position, radius);

        Vector2 separation = Vector2.zero;
        int neighbours = 0;

        foreach (Collider2D collider in nearby)
        {
            if (collider.transform == npc || 
                collider.CompareTag("Player"))
                continue;

            if (collider.GetComponent<NPCBrain>() == null)
                continue;

            Vector2 away = (Vector2)npc.position - 
                           (Vector2)collider.transform.position;

            if (away.sqrMagnitude > 0.001f)
            {
                separation += away.normalized;
                neighbours++;
            }
        }

        if (neighbours == 0)
            return NodeState.Failure;

        separation /= neighbours;
        rb.linearVelocity = separation.normalized * speed;

        return NodeState.Running;
    }
}