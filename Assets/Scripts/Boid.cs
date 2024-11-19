using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private Vector3 velocity;
    [SerializeField] private float slowdownMultiplier = 0.01f;
    public void SetVelocity(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        velocity += v1 + v2 + v3 + v4;
        velocity *= slowdownMultiplier;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public void MoveBoid()
    {
        transform.position += velocity;
    }
}
