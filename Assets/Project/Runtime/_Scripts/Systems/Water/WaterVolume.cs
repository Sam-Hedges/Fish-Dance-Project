using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WaterVolume : GenerateWaterVolume
{

    private MeshRenderer meshRenderer;
    public Vector3 Dimensions = Vector3.zero;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// Returns the current height of the water mesh at <paramref name="_position"/>
    /// </summary>
    /// <param name="_position"></param>
    /// <returns></returns>
    public float? GetHeight(Vector3 _position)
    {
        // Ensure there's a material or return height as zero
        Material currentMaterial = meshRenderer.sharedMaterial;
        if (!currentMaterial) { return 0f; }

        // replicate the shader logic, using parameters pulled from the specific material, to return the height at the specified position
        float? waterHeight = GetHeight(_position);
        if (!waterHeight.HasValue)
        {
            return null;
        }
        float _WaveFrequency = currentMaterial.GetFloat("_WaveFrequency");
        float _WaveScale = currentMaterial.GetFloat("_WaveScale");
        float _WaveSpeed = currentMaterial.GetFloat("_WaveSpeed");
        float time = Time.time * _WaveSpeed;
        float shaderOffset = (Mathf.Sin(_position.x * _WaveFrequency + time) + Mathf.Cos(_position.z * _WaveFrequency + time)) * _WaveScale;
        return waterHeight.Value + shaderOffset;
    }

    protected override void GenerateTiles(ref bool[,,] _tiles)
    {
        // calculate volume in tiles
        var maxX = Mathf.Clamp(Mathf.RoundToInt(Dimensions.x / TileSize), 1, MAX_TILES_X);
        var maxY = Mathf.Clamp(Mathf.RoundToInt(Dimensions.y / TileSize), 1, MAX_TILES_Y);
        var maxZ = Mathf.Clamp(Mathf.RoundToInt(Dimensions.z / TileSize), 1, MAX_TILES_Z);

        // populate the tiles with a box volume
        for (var x = 0; x < maxX; x++)
        {
            for (var y = 0; y < maxY; y++)
            {
                for (var z = 0; z < maxZ; z++)
                {
                    _tiles[x, y, z] = true;
                }
            }
        }
    }

    public override void Validate()
    {
        // keep values sensible
        Dimensions.x = Mathf.Clamp(Dimensions.x, 1, MAX_TILES_X);
        Dimensions.y = Mathf.Clamp(Dimensions.y, 1, MAX_TILES_Y);
        Dimensions.z = Mathf.Clamp(Dimensions.z, 1, MAX_TILES_Z);
    }

    
}
