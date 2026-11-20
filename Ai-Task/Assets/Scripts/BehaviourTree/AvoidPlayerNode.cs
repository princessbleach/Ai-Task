using UnityEngine;

public class AvoidPlayerNode : BTNode
{
    private Transform npc;
    private Transform player;
    private Rigidbody2D rb;
    private float detectionRadius;
    private float speed;

    public AvoidPlayerNode(Transform npc, Transform player, Rigidbody2D rb,
        float detectionRadius, float speed)
    {
        this.npc = npc;
        this.player = player;
        this.rb = rb;
        this.detectionRadius = detectionRadius;
        this.speed = speed;
    }

    public override NodeState Tick()
    {
        float distance = Vector2.Distance(npc.position, player.position);

        if (distance > detectionRadius)
            return NodeState.Failure;

        Vector2 direction = (npc.position - player.position).normalized;
        rb.linearVelocity = direction * speed;

        return NodeState.Running;
    }
}