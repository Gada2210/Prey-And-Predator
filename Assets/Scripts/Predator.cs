using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : Agent
{
    // The range within which the predator can detect prey
    public float detectionRange = 15.0f;

    // The range within which the predator can detect walls
    public float wallDetectionRange = 5.0f;

    // The speed at which the predator moves
    public float speed = 7.0f;

    public float fieldOfVisionAngle = 90.0f;

    // The direction in which the predator is currently moving
    private Vector3 direction;

    private Rigidbody rb;

    private void Start()
    {
        // At the start of the game, choose a random direction for the predator to move in
        direction = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized;

        rb = GetComponent<Rigidbody>();
    }

    // Override the Move method
    public override void Move()
    {
        // Calculate the obstacle avoidance force
        Vector3 obstacleAvoidanceForce = ObstacleAvoidance();

        // Add the obstacle avoidance force to the direction of movement
        direction += obstacleAvoidanceForce;

        // Normalize the direction
        direction = direction.normalized;
        
        // Calculate the new position
        Vector3 newPosition = transform.position + direction * speed * Time.deltaTime;

        // Move the predator to the new position
        rb.MovePosition(newPosition);
    }

    // Override the CheckForPredatorsOrPrey method
    public override void CheckForPredatorsOrPrey()
    {
        // Get all the prey in the scene
        Prey[] preys = FindObjectsOfType<Prey>();

        foreach (Prey prey in preys)
        {
            Vector3 toPrey = prey.transform.position - transform.position;
            // If the prey is within the detection range of the predator
            if (toPrey.magnitude < detectionRange)
            {
                // If the prey is within the field of vision of the predator
                if (Vector3.Angle(transform.forward, toPrey) < fieldOfVisionAngle / 2)
                {
                    // Change the direction of the predator to chase the prey
                    direction = toPrey.normalized;
                    return;
                }
            }
        }

        // If no prey are in sight, check for walls
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, wallDetectionRange))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                // If a wall is detected, change direction
                direction = Vector3.Reflect(direction, hit.normal).normalized;
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

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the predator collided with a wall
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Change the direction of the predator
            direction = Vector3.Reflect(direction, collision.contacts[0].normal).normalized;
        }
        // Check if the predator collided with a prey
        else if (collision.gameObject.CompareTag("Prey"))
        {
            // Find the respawn location
            GameObject respawnLocation = GameObject.FindGameObjectWithTag("Start");

            // Move the prey to the respawn location
            collision.gameObject.transform.position = respawnLocation.transform.position;

            // Reset the prey's direction
            Prey prey = collision.gameObject.GetComponent<Prey>();
            prey.RandomDirection();
        }
    }

    private void Update()
    {
        // Check for prey and move the predator
        CheckForPredatorsOrPrey();
        Move();
    }
}
