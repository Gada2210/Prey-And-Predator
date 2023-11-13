using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    public Vector3 position;
    public Vector3 heading;
    public float visionRange;
    public float acceleration;
    public float turningSpeed;

    // Implement in child classes
    public abstract void Move();

    // Implement in child classes
    public abstract void CheckForPredatorsOrPrey();
}