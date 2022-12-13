using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Buoyancy : MonoBehaviour
{
    [SerializeField] private WaterVolume waterVolume;
    [SerializeField] private Transform[] buoyancyPoints;
    [SerializeField] private float depthBeforeSubmerged = 1.5f;
    [SerializeField] private float displacementAmount = 2f;
    [SerializeField] private float waterDrag = 0.1f;
    [SerializeField] private float waterAngularDrag = 1f;
    
    private Rigidbody rb;
    private int numberOfPoints = 0;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        // Null exception checks to ensure the script is set up correctly
        if (waterVolume == null) { throw new Exception("No WaterVolume assigned to Buoyancy"); }
        
        numberOfPoints = buoyancyPoints.Length;
        if (numberOfPoints == 0) { throw new Exception("Not enough buoyancy points assigned to Buoyancy"); }
    }

    /// <summary>
    /// Used to display the cuurent buoyancy points distance from the water surface as a gizmo in editor
    /// </summary>
    private void OnDrawGizmos()
    {
        foreach (Transform t in buoyancyPoints)
        {
            // Introduced a variable to store the current point's position to reduce the number of calls to the transform component
            Vector3 position = t.position;
            
            // Calculates the water level at the current point
            float waveHeight = waterVolume.GetHeight(position);
            
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
        // Loops over every buoyancy point
        foreach (Transform t in buoyancyPoints)
        {
            // Introduced a variable to store the current point's position to reduce the number of calls to the transform component
            Vector3 position = t.position;
            
            // Applies gravity to the object
            rb.AddForceAtPosition(Physics.gravity / numberOfPoints, position, ForceMode.Acceleration);
            
            ApplyBuoyancyForces(position);
        }
    }
    
    /// <summary>
    /// Applies a buoyancy force to the rigidbody at the given position
    /// </summary>
    /// <param name="position"></param>
    private void ApplyBuoyancyForces(Vector3 position) {
        
        // Calculates the water level at the current point
        float waveHeight = waterVolume.GetHeight(position);
            
        // If the point is below the water level apply buoyancy forces to push the object back to the surface
        if (position.y < waveHeight)
        {
            // Calculates the displacement force
            float displacementMultiplier = Mathf.Clamp01((waveHeight - position.y) / depthBeforeSubmerged) * displacementAmount;
                
            // Applies the displacement force at the current points position on the rigidbody
            rb.AddForceAtPosition( new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), position, ForceMode.Acceleration);
                
            // Applies drag to the rigidbody
            rb.AddForce(-rb.velocity * (displacementMultiplier * waterDrag * Time.fixedDeltaTime), ForceMode.VelocityChange);
                
            // Applies angular drag to the rigidbody
            rb.AddTorque(-rb.angularVelocity * (displacementMultiplier * waterAngularDrag * Time.fixedDeltaTime), ForceMode.VelocityChange);
        }
    }
}
