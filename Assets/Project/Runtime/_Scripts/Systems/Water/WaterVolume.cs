using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WaterVolume : GenerateMesh
{
    private MeshRenderer meshRenderer;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
    }

    /// <summary>
    /// Returns the height of the water at the given position
    /// </summary>
    /// <param name="_position"></param>
    /// <returns></returns>
    public float GetHeight(Vector3 _position)
    {
        // Ensure the water has a material
        Renderer renderer = meshRenderer;
        if (!meshRenderer.sharedMaterial) { return 0f; }

        // Replicate the shader logic, using parameters pulled from the specific material, to return the height at the specified position
        float _WaveFrequency = renderer.sharedMaterial.GetFloat("_WaveFrequency");
        float _WaveAmplitude = renderer.sharedMaterial.GetFloat("_WaveAmplitude");
        float _WaveSpeed = renderer.sharedMaterial.GetFloat("_WaveSpeed");
        
        // Calculate the waves of the water
        float waveMovement = Time.time * _WaveSpeed;
        float waveoffset = -Mathf.Sin(waveMovement + _WaveFrequency * _position.x) + -Mathf.Cos(waveMovement + _WaveFrequency * _position.z);
        float waveScaled = waveoffset * _WaveAmplitude;
        float finalWavePositon = transform.position.y + dimensions.y + waveScaled;
        
        return finalWavePositon;
    }

}
