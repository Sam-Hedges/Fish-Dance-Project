using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WaterVolume : GenerateMesh
{
    private MeshRenderer meshRenderer;

    //private Vector3Int Dimensions = Vector3Int.one;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
    }

    private void OnDrawGizmos()
    {
        var _WaveFrequency = GetComponent<Renderer>().sharedMaterial.GetFloat("_WaveFrequency");
        var _WaveScale = GetComponent<Renderer>().sharedMaterial.GetFloat("_WaveAmplitude");
        var _WaveSpeed = GetComponent<Renderer>().sharedMaterial.GetFloat("_WaveSpeed");

        for (int x = 0; x < dimensions.x; x++)
        {
            for (int z = 0; z < dimensions.z; z++)
            {
                // replicate the shader logic, using parameters pulled from the specific material, to return the height at the specified position
                var waterHeight = 0f;
                var time = Time.time * _WaveSpeed;
                var shaderOffset = (Mathf.Sin(x + transform.position.x * _WaveFrequency + time) + Mathf.Cos(z + transform.position.z * _WaveFrequency + time)) * _WaveScale;
                var final = waterHeight + shaderOffset;
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(new Vector3(x + transform.position.x, final, z + transform.position.z), 0.1f);
            }        
        }
    }

    public float GetHeight(Vector3 _position)
    {

        // ensure a material
        var renderer = meshRenderer;
        if (!meshRenderer.sharedMaterial) { return 0f; }

        // replicate the shader logic, using parameters pulled from the specific material, to return the height at the specified position
        var waterHeight = 0f;
        var _WaveFrequency = renderer.sharedMaterial.GetFloat("_WaveFrequency");
        var _WaveScale = renderer.sharedMaterial.GetFloat("_WaveAmplitude");
        var _WaveSpeed = renderer.sharedMaterial.GetFloat("_WaveSpeed");
        var time = Time.time * _WaveSpeed;
        var shaderOffset = (Mathf.Sin(_position.x * _WaveFrequency + time) + Mathf.Cos(_position.z * _WaveFrequency + time)) * _WaveScale;
        return waterHeight + shaderOffset;
    }

}
