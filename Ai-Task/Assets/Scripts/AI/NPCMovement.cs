using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float changeDirectionTime = 2f;

    private Rigidbody2D rb;
    private Vector2 direction;
    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PickDirection();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= changeDirectionTime)
        {
            PickDirection();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direction * moveSpeed;
    }

    void PickDirection()
    {
        direction = Random.insideUnitCircle.normalized;
        timer = 0f;
    }
}