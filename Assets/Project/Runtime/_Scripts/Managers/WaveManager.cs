using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    
    [SerializeField] private float amplitude = 1f;
    [SerializeField] private float length = 2f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float offset = 0f;

    private void Awake()
    {
        if (Instance == null) // Set this to be the current instance
        {
            Instance = this;
        }
        else if (Instance != this) // Destroy this if an instance already exists
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        offset += Time.deltaTime * speed;
    }
    
    public float GetWaveHeight(float x)
    {
        return amplitude * Mathf.Sin(x / length + offset);
    }
}
