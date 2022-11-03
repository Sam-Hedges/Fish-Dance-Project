using PortfolioProject.Bitgem.StylisedWater.URP.Scripts.Bitgem.VFX.StylisedWater;
using UnityEngine;

namespace Project.Runtime._Scripts
{
    public class BuoyancyPoint : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float depthBeforeSubmerged = 1f;
        [SerializeField] private float displacementAmount = 3f;
        [SerializeField] private int numberOfPoints = 4;
        [SerializeField] private float waterDrag = 0.99f;
        [SerializeField] private float waterAngularDrag = 0.5f;

        [SerializeField] public WaterVolumeHelper WaterVolumeHelper;

        private void FixedUpdate()
        {
            rb.AddForceAtPosition(Physics.gravity / numberOfPoints, transform.position, ForceMode.Acceleration);
        
            var instance = WaterVolumeHelper ? WaterVolumeHelper : WaterVolumeHelper.Instance;
            if (!instance)
            {
                return;
            }
        
            var waveHeight = instance.GetHeight(transform.position) ?? transform.position.y;
        
            if (transform.position.y < waveHeight)
            {
                float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
                rb.AddForceAtPosition( new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
                rb.AddForce(displacementMultiplier * -rb.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
                rb.AddTorque(displacementMultiplier * -rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            }
        }
    }
}