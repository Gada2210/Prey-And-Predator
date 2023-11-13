using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public float speed = 2.0f;
    public float minX = -6.5f;
    public float maxX = 6.5f;
    private int direction = 1;

    void Update()
    {
        // Calculate the new position
        float newX = transform.position.x + direction * speed * Time.deltaTime;

        // If the obstacle has reached one of the limits, change the direction
        if (newX < minX)
        {
            newX = minX;
            direction = 1;
        }
        else if (newX > maxX)
        {
            newX = maxX;
            direction = -1;
        }

        // Move the obstacle to the new position
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
