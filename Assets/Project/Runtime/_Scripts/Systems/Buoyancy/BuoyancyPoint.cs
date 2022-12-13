using System;
using UnityEngine;

public class BuoyancyPoint : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private WaterVolume WaterVolume;
    //[SerializeField] private BuoyancyScriptableObject settings;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    public int numberOfPoints = 4;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;


    private void Awake()
    {
        /*
        depthBeforeSubmerged = settings.depthBeforeSubmerged;
        displacementAmount = settings.displacementAmount;
        numberOfPoints = settings.numberOfPoints;
        waterDrag = settings.waterDrag;
        waterAngularDrag = settings.waterAngularDrag;
        */
    }

    private void FixedUpdate()
    {
        rb.AddForceAtPosition(Physics.gravity / numberOfPoints, transform.position, ForceMode.Acceleration);
        
        float waveHeight = WaterVolume.GetHeight(transform.position);

        if (transform.position.y < waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            
            rb.AddForceAtPosition( new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
            
            rb.AddForce(-rb.velocity * (displacementMultiplier * waterDrag * Time.fixedDeltaTime), ForceMode.VelocityChange);
            
            rb.AddTorque(-rb.angularVelocity * (displacementMultiplier * waterAngularDrag * Time.fixedDeltaTime), ForceMode.VelocityChange);
        }
    }
}
