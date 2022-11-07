using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WaterVolume : GenerateWaterVolume
{
    
    private static WaterVolume instance = null;
    public static WaterVolume Instance { get { return instance; } }

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    
    public Vector3 Dimensions = Vector3.zero;
    public GenerateWaterVolume _WaterVolume = null;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
    }

    public float? GetHeight(Vector3 _position)
    {
        // ensure a water volume
        if (!_WaterVolume)
        {
            return 0f;
        }

        // ensure a material
        var renderer = GetComponent<MeshRenderer>();
        if (!meshRenderer.sharedMaterial)
        {
            return 0f;
        }

        // replicate the shader logic, using parameters pulled from the specific material, to return the height at the specified position
        var waterHeight = GetHeight(_position);
        if (!waterHeight.HasValue)
        {
            return null;
        }
        var _WaveFrequency = renderer.sharedMaterial.GetFloat("_WaveFrequency");
        var _WaveScale = renderer.sharedMaterial.GetFloat("_WaveScale");
        var _WaveSpeed = renderer.sharedMaterial.GetFloat("_WaveSpeed");
        var time = Time.time * _WaveSpeed;
        var shaderOffset = (Mathf.Sin(_position.x * _WaveFrequency + time) + Mathf.Cos(_position.z * _WaveFrequency + time)) * _WaveScale;
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
