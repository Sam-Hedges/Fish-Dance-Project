using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BuoyancyPoint : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private WaterVolume WaterVolume;
    [SerializeField] private BuoyancyScriptableObject settings;

    private void FixedUpdate()
    {
        rb.AddForceAtPosition(Physics.gravity / settings.numberOfPoints, transform.position, ForceMode.Acceleration);
        float waveHeight = WaterVolume.GetHeight(transform.position) ?? transform.position.y;
    
        if (transform.position.y < waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / settings.depthBeforeSubmerged) * settings.displacementAmount;
            rb.AddForceAtPosition( new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
            rb.AddForce(-rb.velocity * (displacementMultiplier * settings.waterDrag * Time.fixedDeltaTime), ForceMode.VelocityChange);
            rb.AddTorque(-rb.angularVelocity * (displacementMultiplier * settings.waterAngularDrag * Time.fixedDeltaTime), ForceMode.VelocityChange);
        }
    }
}
