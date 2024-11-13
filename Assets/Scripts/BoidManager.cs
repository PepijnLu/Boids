using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    [SerializeField] private float moveToCentreMultiplier = 0.01f;
    [SerializeField] private float moveToPlaceMultiplier = 0.01f;
    [SerializeField] private float matchVelocityMultiplier = 0.125f;
    [SerializeField] private float minimumBoidDistance = 0.1f;
    [SerializeField] private int group1Size;
    [SerializeField] private Boid fishPrefab;
    [SerializeField] private Vector3 currentGoal = new Vector3(15, 15, 15);
    [SerializeField] private Vector3 minSpawnBounds = new Vector3(-5, -5, -5);
    [SerializeField] private Vector3 maxSpawnBounds = new Vector3(5, 5, 5);
    [SerializeField] private Vector3 minMoveBounds = new Vector3(-25, -25, -25);
    [SerializeField] private Vector3 maxMoveBounds = new Vector3(25, 25, 25);

    List<Boid> group = new();
    void Start()
    {
        InstantiateBoids();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        MoveBoids();
    }

    void InstantiateBoids()
    {
        for(int i = 0; i < group1Size; i++)
        {
            float x = Random.Range(minSpawnBounds.x, maxSpawnBounds.x);
            float y = Random.Range(minSpawnBounds.y, maxSpawnBounds.y);
            float z = Random.Range(minSpawnBounds.z, maxSpawnBounds.z);

            Boid boid = Instantiate(fishPrefab, new Vector3(x, y, z), transform.rotation);
            group.Add(boid);
        }
    }

    void MoveBoids()
    {
        Vector3 v1, v2, v3, v4, v5;
        foreach(Boid _boid in group)
        {
            v1 = Rule1(_boid);
            v2 = Rule2(_boid);
            v3 = Rule3(_boid);
            v4 = Rule4(_boid, currentGoal);
            v5 = Rule5(_boid);

            _boid.SetVelocity(v1, v2, v3, v4, v5);
            _boid.MoveBoid();
        }
    }

    Vector3 Rule1(Boid boid)
    {
        //if(group.IndexOf(boid) == 0) return new Vector3(0, 0, 0);

        //Either initialize as 0 or the position of the boid?
        Vector3 percievedCentreOfMass = new Vector3(0, 0, 0);

        foreach(Boid _boid in group)
        {
            if(boid.gameObject != _boid.gameObject)
            {
                percievedCentreOfMass = percievedCentreOfMass + _boid.transform.position;
            }
        }
        percievedCentreOfMass = percievedCentreOfMass / (group.Count - 1);
        Debug.Log("Perceived CoM: " + percievedCentreOfMass + "boid pos: " + boid.transform.position); 
        return (percievedCentreOfMass - boid.transform.position) * moveToCentreMultiplier;
    }
    Vector3 Rule2(Boid boid)
    {
        //if(group.IndexOf(boid) == 0) return new Vector3(0, 0, 0);

        Vector3 displacement = new Vector3(0, 0, 0);
        foreach(Boid _boid in group)
        {
            if(boid.gameObject != _boid.gameObject)
            {
                if((_boid.transform.position - boid.transform.position).magnitude < minimumBoidDistance)
                {
                    displacement = displacement - (_boid.transform.position - boid.transform.position);
                }
            }
        }
        // Debug.Log("Displacement: " + displacement);
        return displacement;
    }
    Vector3 Rule3(Boid boid)
    {
        //if(group.IndexOf(boid) == 0) return new Vector3(0, 0, 0);

        Vector3 percievedVelocity = new Vector3(0, 0, 0);
        foreach(Boid _boid in group)
        {
            if(boid.gameObject != _boid.gameObject)
            {
               percievedVelocity += _boid.GetVelocity();
            }
        }

        percievedVelocity = percievedVelocity / (group.Count - 1);
        Debug.Log("Perceived V: " + percievedVelocity); 
        return percievedVelocity * matchVelocityMultiplier;
    }

    Vector3 Rule4(Boid boid, Vector3 place)
    {
        //if(group.IndexOf(boid) == 0) return new Vector3(0, 0, 0);

        Debug.Log("Move to place multiplier: " + (place - boid.transform.position) * moveToPlaceMultiplier);
        return (place - boid.transform.position) * moveToPlaceMultiplier;
    }

    Vector3 Rule5(Boid boid)
    {
        Vector3 boundsCorrection = new Vector3(0, 0, 0);

        //if(group.IndexOf(boid) == 0) return new Vector3(0, 0, 0);

        if(boid.transform.position.x < minMoveBounds.x) boundsCorrection.x = 10;
		else if (boid.transform.position.x > maxMoveBounds.x) boundsCorrection.x = -10;
		if(boid.transform.position.y < minMoveBounds.y) boundsCorrection.y = 10;
		else if (boid.transform.position.y > maxMoveBounds.y) boundsCorrection.y = -10;
		if (boid.transform.position.z < minMoveBounds.z) boundsCorrection.z = 10;
		else if (boid.transform.position.z > maxSpawnBounds.z) boundsCorrection.z = -10;
		
		return boundsCorrection;
    }
}



