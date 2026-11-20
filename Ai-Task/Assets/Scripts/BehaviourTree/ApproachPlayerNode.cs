using UnityEngine;

public class ApproachPlayerNode : BTNode
{
    private Transform npc;
    private Transform player;
    private Rigidbody2D rb;
    private float detectionRadius;
    private float speed;
    private float stoppingDistance;

    public ApproachPlayerNode(Transform npc, Transform player, Rigidbody2D rb,
        float detectionRadius, float speed, float stoppingDistance)
    {
        this.npc = npc;
        this.player = player;
        this.rb = rb;
        this.detectionRadius = detectionRadius;
        this.speed = speed;
        this.stoppingDistance = stoppingDistance;
    }

    public override NodeState Tick()
    {
        float distance = Vector2.Distance(npc.position, player.position);

        if (distance > detectionRadius)
            return NodeState.Failure;

        if (distance <= stoppingDistance)
        {
            rb.linearVelocity = Vector2.zero;
            return NodeState.Success;
        }

        Vector2 direction = (player.position - npc.position).normalized;
        rb.linearVelocity = direction * speed;

        return NodeState.Running;
    }
}
