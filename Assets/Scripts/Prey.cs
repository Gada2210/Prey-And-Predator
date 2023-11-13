using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : Agent
{
    public float detectionRange = 10.0f;
    public float speed = 5.0f;
    public float wallDetectionRange = 5.0f;
    public float fieldOfVisionAngle = 300.0f;
    private Vector3 direction;
    private Rigidbody rb;

    private void Start()
    {
        direction = RandomDirection();
        rb = GetComponent<Rigidbody>();
    }

    public override void Move()
    {
        // Calculate the obstacle avoidance force
        Vector3 obstacleAvoidanceForce = ObstacleAvoidance();

        // Add the obstacle avoidance force to the direction of movement
        direction += obstacleAvoidanceForce;

        // Normalize the direction
        direction = direction.normalized;
        Vector3 newPosition = transform.position + direction * speed * Time.deltaTime;
        rb.MovePosition(newPosition);
    }

    public override void CheckForPredatorsOrPrey()
    {
        // Get all the predators in the scene
        Predator[] predators = FindObjectsOfType<Predator>();

        foreach (Predator predator in predators)
        {
            Vector3 toPredator = predator.transform.position - transform.position;
            // If the predator is within the detection range of the prey
            if (toPredator.magnitude < detectionRange)
            {
                // If the predator is within the field of vision of the prey
                if (Vector3.Angle(transform.forward, toPredator) < fieldOfVisionAngle / 2)
                {
                    // Change the direction of the prey to move away from the predator
                    direction = (transform.position - predator.transform.position).normalized;
                    return;
                }
            }
        }

        // If no predators are in sight, check for walls
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, wallDetectionRange))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                // If a wall is detected, change direction
                direction = Vector3.Reflect(direction, hit.normal).normalized;

                // Add a random offset to the direction
                Vector3 offset = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
                direction += offset;

                // Normalize the direction
                direction = direction.normalized;
            }
        }
    }

    private Vector3 ObstacleAvoidance()
    {
        // The maximum distance at which obstacles are detected
        float maxObstacleDetectionRange = 1.0f;

        // The force that pushes the agent away from obstacles
        Vector3 steeringForce = Vector3.zero;

        // Cast a ray in the direction of movement
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, maxObstacleDetectionRange))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                // Calculate the steering force
                float multiplier = 1.0f + (maxObstacleDetectionRange - hit.distance) / maxObstacleDetectionRange;
                steeringForce = hit.normal * multiplier;
            }
        }

        return steeringForce;
    }

    public Vector3 RandomDirection()
    {
        return new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            direction = Vector3.Reflect(direction, collision.contacts[0].normal).normalized;
        }
    }

    private void Update()
    {
        CheckForPredatorsOrPrey();
        Move();
    }
}
