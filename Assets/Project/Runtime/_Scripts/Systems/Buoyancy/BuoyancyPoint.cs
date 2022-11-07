using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BuoyancyPoint : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private WaterVolume WaterVolume;
    [SerializeField] private BuoyancyScriptableObject settings;
    
    private float depthBeforeSubmerged = 1f;
    private float displacementAmount = 3f;
    private int numberOfPoints = 4;
    private float waterDrag = 0.99f;
    private float waterAngularDrag = 0.5f;

    private void Awake() {
        // Continue this pattern for all the settings you want to be able to change in the inspector
        depthBeforeSubmerged = settings.depthBeforeSubmerged;
        displacementAmount = settings.displacementAmount;
        numberOfPoints = settings.numberOfPoints;
        waterDrag = settings.waterDrag;
        waterAngularDrag = settings.waterAngularDrag;
    }

    private void FixedUpdate()
    {
        rb.AddForceAtPosition(Physics.gravity / numberOfPoints, transform.position, ForceMode.Acceleration);
        float waveHeight = WaterVolume.GetHeight(transform.position) ?? transform.position.y;
    
        if (transform.position.y < waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            rb.AddForceAtPosition( new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
            rb.AddForce(displacementMultiplier * -rb.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rb.AddTorque(displacementMultiplier * -rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}
