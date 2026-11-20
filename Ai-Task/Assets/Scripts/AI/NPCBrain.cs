using UnityEngine;
using System.Diagnostics;

public class NPCBrain : MonoBehaviour
{
    [Header("Personality")]
    public NPCPersonality personality = NPCPersonality.Friendly;

    [Header("Movement")]
    public float wanderSpeed = 2f;
    public float fleeSpeed = 4f;
    public float approachSpeed = 2.5f;
    public float crazySpeed = 3.5f;

    [Header("Detection")]
    public float detectionRadius = 3f;
    public float stoppingDistance = 1.5f;

    [Header("Separation")]
    public float separationRadius = 1.2f;
    public float separationSpeed = 2f;

    [Header("Telemetry")]
    public string currentBehaviour = "Starting";
    public float lastEvaluationTimeMs;
    public float averageEvaluationTimeMs;
    public int evaluationCount;

    private Rigidbody2D rb;
    private Transform player;
    private BTSelector root;
    private double totalEvaluationTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject == null)
        {
            UnityEngine.Debug.LogError("No Player tagged object found!");
            return;
        }

        player = playerObject.transform;

        BuildTree();
    }

    void BuildTree()
    {
        root = new BTSelector();

        root.AddChild(new SeparationNode(
            transform, rb, separationRadius, separationSpeed));

        switch (personality)
        {
            case NPCPersonality.Friendly:

                root.AddChild(new ApproachPlayerNode(
                    transform, player, rb,
                    detectionRadius, approachSpeed, stoppingDistance));

                root.AddChild(new WanderNode(rb, wanderSpeed));
                break;

            case NPCPersonality.Antisocial:

                root.AddChild(new AvoidPlayerNode(
                    transform, player, rb,
                    detectionRadius, fleeSpeed));

                root.AddChild(new WanderNode(rb, wanderSpeed));
                break;

            case NPCPersonality.Crazy:

                root.AddChild(new CrazyWanderNode(rb, crazySpeed));
                break;
        }
    }

    void Update()
    {
        if (root == null)
            return;

        Stopwatch stopwatch = Stopwatch.StartNew();

        root.Tick();

        stopwatch.Stop();

        lastEvaluationTimeMs = (float)stopwatch.Elapsed.TotalMilliseconds;

        totalEvaluationTime += lastEvaluationTimeMs;
        evaluationCount++;

        averageEvaluationTimeMs = (float)(totalEvaluationTime / evaluationCount);

        UpdateBehaviourLabel();
    }

    void UpdateBehaviourLabel()
    {
        Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, separationRadius);

        foreach (Collider2D collider in nearby)
        {
            if (collider.transform != transform &&
                collider.GetComponent<NPCBrain>() != null)
            {
                currentBehaviour = "Separation";
                return;
            }
        }

        float distance = Vector2.Distance(transform.position, player.position);

        switch (personality)
        {
            case NPCPersonality.Friendly:
                currentBehaviour = distance <= detectionRadius
                    ? "Approaching Player"
                    : "Wandering";
                break;

            case NPCPersonality.Antisocial:
                currentBehaviour = distance <= detectionRadius
                    ? "Avoiding Player"
                    : "Wandering";
                break;

            case NPCPersonality.Crazy:
                currentBehaviour = "Erratic Wandering";
                break;
        }
    }
}