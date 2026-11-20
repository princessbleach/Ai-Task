using UnityEngine;

public class WanderNode : BTNode
{
    private Rigidbody2D rb;
    private float speed;
    private Vector2 direction;
    private float timer;
    private float changeTime = 2f;

    public WanderNode(Rigidbody2D rb, float speed)
    {
        this.rb = rb;
        this.speed = speed;
        PickDirection();
    }

    public override NodeState Tick()
    {
        timer += Time.deltaTime;

        if (timer >= changeTime)
            PickDirection();

        rb.linearVelocity = direction * speed;

        return NodeState.Running;
    }

    void PickDirection()
    {
        direction = Random.insideUnitCircle.normalized;
        timer = 0f;
    }
}
