using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Buoyancy : MonoBehaviour
{
    [SerializeField] private WaterVolume WaterVolume;
    [SerializeField] private Transform[] buoyancyPoints;
    private Rigidbody rb;
    //[SerializeField] private BuoyancyScriptableObject settings;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    private int numberOfPoints = 0;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        if (WaterVolume == null) { throw new Exception("No WaterVolume assigned to Buoyancy"); }
        
        numberOfPoints = buoyancyPoints.Length;
        if (numberOfPoints == 0) { throw new Exception("No buoyancy points assigned to Buoyancy"); }
        
        /*
        depthBeforeSubmerged = settings.depthBeforeSubmerged;
        displacementAmount = settings.displacementAmount;
        numberOfPoints = settings.numberOfPoints;
        waterDrag = settings.waterDrag;
        waterAngularDrag = settings.waterAngularDrag;
        */
    }


    private void OnDrawGizmos()
    {
        foreach (Transform t in buoyancyPoints)
        {
            // Introduced a variable to store the current point's position to reduce the number of calls to the transform component
            Vector3 position = t.position;
            
            // Calculates the water level at the current point
            float waveHeight = WaterVolume.GetHeight(position);
            
            // Draws a line from the current point to the water level
            if (position.y < waveHeight)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.green;
            }
            Gizmos.DrawLine(position, new Vector3(position.x, waveHeight, position.z));
        }
    }

    private void Update()
    {
        foreach (Transform t in buoyancyPoints)
        {
            // Introduced a variable to store the current point's position to reduce the number of calls to the transform component
            Vector3 position = t.position;
            
            // Calculates the water level at the current point
            float waveHeight = WaterVolume.GetHeight(position);

            Color color;
            
            // Draws a line from the current point to the water level
            if (position.y < waveHeight)
            {
                color = Color.red;
            }
            else
            {   
                color = Color.green;
            }
            Debug.DrawLine(position, new Vector3(position.x, waveHeight, position.z), color);
        }
        
        return;
        foreach (Transform t in buoyancyPoints)
        {
            // Introduced a variable to store the current point's position to reduce the number of calls to the transform component
            Vector3 position = t.position;
            
            // Applies gravity to the object
            rb.AddForceAtPosition(Physics.gravity / numberOfPoints, position, ForceMode.Acceleration);
            
            // Calculates the water level at the current point
            float waveHeight = WaterVolume.GetHeight(position);
            
            // If the point is below the water level apply buoyancy forces to push the object back to the surface
            if (position.y < waveHeight)
            {

                float displacementMultiplier = Mathf.Clamp01((waveHeight - position.y) / depthBeforeSubmerged) * displacementAmount;
            
                rb.AddForceAtPosition( new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), position, ForceMode.Acceleration);
            
                rb.AddForce(-rb.velocity * (displacementMultiplier * waterDrag * Time.fixedDeltaTime), ForceMode.VelocityChange);
            
                rb.AddTorque(-rb.angularVelocity * (displacementMultiplier * waterAngularDrag * Time.fixedDeltaTime), ForceMode.VelocityChange);
            }
        }
    }
    
    /*
    private Rigidbody rb;
    [SerializeField] private float depthBeforeSubmerged = 1f;
    [SerializeField] private float displacementAmount = 3f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float waveHeight = WaveManager.Instance.GetWaveHeight(transform.position.x);
        
        if (transform.position.y < waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01((waveHeight -transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            rb.AddForce( new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);
        }
    }
    */
}
