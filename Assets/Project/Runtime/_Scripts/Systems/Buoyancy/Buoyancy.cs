using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Buoyancy : MonoBehaviour
{
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
}
