using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Water.Attribute;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GenerateMesh : MonoBehaviour
{
    public GameObject debugPoint;
    public bool debugged;
    
    private MeshFilter meshFilter;
    private bool built = false;
    private Mesh mesh;
    private bool[,,] tiles;
    
    [SerializeField] private Vector3Int dimensions = Vector3Int.one;
    [SerializeField] private float tileScale = 1f;
    [SerializeField] private float uvScale = 1f;
    
    [Flags] // This allows multiple flags to be set within the editor concurrently
    public enum TileFace : int
    {
        NegX = 1,
        PosX = 2,
        NegZ = 4,
        PosZ = 8
    }
    
    [FlagEnum]
    public TileFace includeFaces = TileFace.NegX | TileFace.NegZ | TileFace.PosX | TileFace.PosZ;

    [FlagEnum]
    public TileFace includeFoam = TileFace.NegX | TileFace.NegZ | TileFace.PosX | TileFace.PosZ;
    
    private void Awake() {
        meshFilter = GetComponent<MeshFilter>();
    }

    private void Update() {

        if (!built) {
            BuildMesh();
        }
        
    }

    private void OnValidate() {
        mesh = new Mesh {
            name = "Procedural Mesh"
        };
        
        GetComponent<MeshFilter>().mesh = mesh;
        
        built = false;
    }
    
    private void BuildMesh() {
        
        // Delete any other mesh
        mesh.Clear();
        
        // 
        tiles = new bool[dimensions.x, dimensions.y, dimensions.z];
        for (int x = 0; x < dimensions.x; x++) {
            for (int y = 0; y < dimensions.y; y++) {
                for (int z = 0; z < dimensions.z; z++) {
                    tiles[x, y, z] = true;
                }
            }
        }
        
        // Stores the Vertices of the mesh
        List<Vector3> vertices = new List<Vector3>();
        
        // Refer to the indices of the vertex positions used to denote te edges of the triangles
        List<int> triangles = new List<int>();
        
        // Defines what direction the triangles are facing
        List<Vector3> normals = new List<Vector3>();
        
        // Maps the UV coordinates to the mesh
        List<Vector2> uvs = new List<Vector2>();
        
        // Used to flag vertices as being on the edge of the mesh
        List<Color> colors = new List<Color>(); 
        
        
        for (int x = 0; x < dimensions.x; x++) {
            
            for (int y = 0; y < dimensions.y; y++) {
                
                for (int z = 0; z < dimensions.z; z++) {
                    
                    // Calculate the tile position
                    float x0 = x * tileScale;
                    float x1 = x0 + tileScale;
                    float y0 = y * tileScale;
                    float y1 = y0 + tileScale;
                    float z0 = z * tileScale;
                    float z1 = z0 + tileScale;

                    // UV coordinates for the tile
                    // *UVScale is used to scale the UV coordinates
                    // /TileScale is used to scale the UV coordinates to the tile size
                    Vector3 position = transform.position;
                    float ux0 = x0 * uvScale / tileScale + position.x;
                    float ux1 = x1 * uvScale / tileScale + position.x;
                    float uy0 = y0 * uvScale / tileScale + position.y;
                    float uy1 = y1 * uvScale / tileScale + position.y;
                    float uz0 = z0 * uvScale / tileScale + position.z;
                    float uz1 = z1 * uvScale / tileScale + position.z;
                    
                    // Check to see if current the vertex is an edge
                    // Check if the current vertex is on the negative edge
                    bool negX = x == 0 || !tiles[x - 1, y, z];
                    bool negY = y == 0 || !tiles[x, y - 1, z];
                    bool negZ = z == 0 || !tiles[x, y, z - 1];
                    // Check if the current vertex is on the positive edge
                    bool posX = x == dimensions.x - 1 || !tiles[x + 1, y, z];
                    bool posY = y == dimensions.y - 1 || !tiles[x, y + 1, z];
                    bool posZ = z == dimensions.z - 1 || !tiles[x, y, z + 1];

                    // Check to see if the current vertex is on the edge
                    bool negXnegZ = !negX && !negZ && x > 0 && z > 0;
                    bool negXposZ = !negX && !posZ && x > 0 && z < dimensions.z;
                    bool posXposZ = !posX && !posZ && x < dimensions.x && z < dimensions.z;
                    bool posXnegZ = !posX && !negZ && x < dimensions.x && z > 0;
                    
                    // 
                    bool generateFaceNegX = negX && (includeFaces & TileFace.NegX) == TileFace.NegX;
                    bool generateFacePosX = posX && (includeFaces & TileFace.PosX) == TileFace.PosX;
                    bool generateFaceNegZ = negZ && (includeFaces & TileFace.NegZ) == TileFace.NegZ;
                    bool generateFacePosZ = posZ && (includeFaces & TileFace.PosZ) == TileFace.PosZ;
                    
                    // 
                    bool foamNegX = negX && (includeFoam & TileFace.NegX) == TileFace.NegX;
                    bool foamPosX = posX && (includeFoam & TileFace.PosX) == TileFace.PosX;
                    bool foamNegZ = negZ && (includeFoam & TileFace.NegZ) == TileFace.NegZ;
                    bool foamPosZ = posZ && (includeFoam & TileFace.PosZ) == TileFace.PosZ;
                    
                    //
                    bool foamNegXnegZ = negXnegZ && ((includeFoam & TileFace.NegX) == TileFace.NegX || (includeFoam & TileFace.NegZ) == TileFace.NegZ);
                    bool foamNegXposZ = negXposZ && ((includeFoam & TileFace.PosX) == TileFace.PosX || (includeFoam & TileFace.PosZ) == TileFace.PosZ);
                    bool foamPosXposZ = posXposZ && ((includeFoam & TileFace.NegZ) == TileFace.NegZ || (includeFoam & TileFace.PosZ) == TileFace.PosZ);
                    bool foamPosXnegZ = posXnegZ && ((includeFoam & TileFace.PosZ) == TileFace.PosZ || (includeFoam & TileFace.NegZ) == TileFace.NegZ);

                    
                    // Create the top face
                    if (y == dimensions.y - 1) {
                        
                        if (!debugged) { SpawnDebugPoints(x0, x1, y0, y1, z0, z1); }
                        
                        // Add the vertices
                        // Y1 is used so the vertices are placed on the top of the tile
                        vertices.Add(new Vector3(x0, y1, z0));
                        vertices.Add(new Vector3(x0, y1, z1));
                        vertices.Add(new Vector3(x1, y1, z1));
                        vertices.Add(new Vector3(x1, y1, z0));
                        
                        // Add the normals
                        normals.Add(new Vector3(0, 1, 0));
                        normals.Add(new Vector3(0, 1, 0));
                        normals.Add(new Vector3(0, 1, 0));
                        normals.Add(new Vector3(0, 1, 0));
                        
                        // Add the UV coordinates
                        uvs.Add(new Vector2(ux0, uz0));
                        uvs.Add(new Vector2(ux0, uz1));
                        uvs.Add(new Vector2(ux1, uz1));
                        uvs.Add(new Vector2(ux1, uz0));
                        
                        // Colour the vertices to denote whether the shader should add foam
                        colors.Add(foamNegX || foamNegZ || foamNegXnegZ ? Color.red : Color.black);
                        colors.Add(foamNegX || foamPosZ || foamNegXposZ ? Color.red : Color.black);
                        colors.Add(foamPosX || foamPosZ || foamPosXposZ ? Color.red : Color.black);
                        colors.Add(foamPosX || foamNegZ || foamPosXnegZ ? Color.red : Color.black);
                        
                        // Configure the vertices to connect into triangles
                        int v = vertices.Count - 4;
                        if (foamNegX && foamPosZ || foamPosX && foamNegZ)
                        {
                            triangles.Add(v + 1);
                            triangles.Add(v + 2);
                            triangles.Add(v + 3);
                            triangles.Add(v + 3);
                            triangles.Add(v);
                            triangles.Add(v + 1);
                        }
                        else
                        {
                            triangles.Add(v);
                            triangles.Add(v + 1);
                            triangles.Add(v + 2);
                            triangles.Add(v + 2);
                            triangles.Add(v + 3);
                            triangles.Add(v);
                        }
                    }

                    // Create the side faces
                    if (generateFaceNegX) {
                        
                        // Initialise the mesh data
                        Vector3 v1 = new Vector3(x0, y0, z1);
                        Vector3 v2 = new Vector3(x0, y1, z1);
                        Vector3 v3 = new Vector3(x0, y1, z0);
                        Vector3 v4 = new Vector3(x0, y0, z0);
                        Vector3 n = new Vector3(-1, 0, 0);
                        Vector2 uv1 = new Vector2(uy1, uz0);
                        Vector2 uv2 = new Vector2(uy1, uz1);
                        Vector2 uv3 = new Vector2(uy0, uz1);
                        Vector2 uv4 = new Vector2(uy0, uz0);

                        GenerateFace(v1, v2, v3, v4, n, uv1, uv2, uv3, uv4);
                    }
                    if (generateFacePosX) {
                        
                        // Initialise the mesh data
                        Vector3 v1 = new Vector3(x1, y0, z0);
                        Vector3 v2 = new Vector3(x1, y1, z0);
                        Vector3 v3 = new Vector3(x1, y1, z1);
                        Vector3 v4 = new Vector3(x1, y0, z1);
                        Vector3 n = new Vector3(1, 0, 0);
                        Vector2 uv1 = new Vector2(uz0, uy0);
                        Vector2 uv2 = new Vector2(uz0, uy1);
                        Vector2 uv3 = new Vector2(uz1, uy1);
                        Vector2 uv4 = new Vector2(uz1, uy0);
                        
                        GenerateFace(v1, v2, v3, v4, n, uv1, uv2, uv3, uv4);
                    }
                    if (generateFaceNegZ) {
                        
                        // Initialise the mesh data
                        Vector3 v1 = new Vector3(x0, y0, z0);
                        Vector3 v2 = new Vector3(x0, y1, z0);
                        Vector3 v3 = new Vector3(x1, y1, z0);
                        Vector3 v4 = new Vector3(x1, y0, z0);
                        Vector3 n = new Vector3(0, 0, -1);
                        Vector2 uv1 = new Vector2(ux0, uy0);
                        Vector2 uv2 = new Vector2(ux0, uy1);
                        Vector2 uv3 = new Vector2(ux1, uy1);
                        Vector2 uv4 = new Vector2(ux1, uy0);
                        
                        GenerateFace(v1, v2, v3, v4, n, uv1, uv2, uv3, uv4);
                    }
                    if (generateFacePosZ) {
                        
                        // Initialise the mesh data
                        Vector3 v1 = new Vector3(x1, y0, z1);
                        Vector3 v2 = new Vector3(x1, y1, z1);
                        Vector3 v3 = new Vector3(x0, y1, z1);
                        Vector3 v4 = new Vector3(x0, y0, z1);
                        Vector3 n = new Vector3(0, 0, 1);
                        Vector2 uv1 = new Vector2(ux1, uy0);
                        Vector2 uv2 = new Vector2(ux1, uy1);
                        Vector2 uv3 = new Vector2(ux0, uy1);
                        Vector2 uv4 = new Vector2(ux0, uy0);
                        
                        GenerateFace(v1, v2, v3, v4, n, uv1, uv2, uv3, uv4);
                    }
                    
                    /// <summary>
                    /// Local function to generate the side faces used to reduce code duplication
                    /// </summary>
                    void GenerateFace(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector3 n, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4) {
                        
                        // Add the vertices
                        vertices.Add(v1);
                        vertices.Add(v2);
                        vertices.Add(v3);
                        vertices.Add(v4);
                        
                        // Add the normals
                        normals.Add(n);
                        normals.Add(n);
                        normals.Add(n);
                        normals.Add(n);
                        
                        // Add the UV coordinates
                        uvs.Add(uv1);
                        uvs.Add(uv2);
                        uvs.Add(uv3);
                        uvs.Add(uv4);
                        
                        // Colour the vertices to denote whether the shader should add foam
                        colors.Add(Color.black);
                        colors.Add(posY ? Color.red : Color.black);
                        colors.Add(posY ? Color.red : Color.black);
                        colors.Add(Color.black);
                        
                        // Configure the vertices to connect into triangles
                        var v = vertices.Count - 4;
                        triangles.Add(v);
                        triangles.Add(v + 1);
                        triangles.Add(v + 2);
                        triangles.Add(v + 2);
                        triangles.Add(v + 3);
                        triangles.Add(v);
                    }
                }
            }
        }
        
        /*
        mesh.vertices = new Vector3[] {
            Vector3.zero, Vector3.right, Vector3.up, new Vector3(1f, 1f)
        };
        
        mesh.triangles = new int[] {
            0, 2, 1, 1, 2, 3
        };
        
        mesh.normals = new Vector3[] {
            Vector3.back, Vector3.back, Vector3.back, Vector3.back
        };
        
        mesh.uv = new Vector2[] {
            Vector2.zero, Vector2.right, Vector2.up, Vector2.one
        };
        */
        
        // apply the buffers
        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangles, 0);

        // update
        mesh.RecalculateBounds();
        //mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        // Assign the mesh to the mesh filter
        meshFilter.sharedMesh = mesh;
        
        // Flag mesh as built
        built = true;
    }
    
    
    void SpawnDebugPoints(float x0, float x1, float y0, float y1, float z0, float z1) {
        
        GameObject obj;
        
        obj = Instantiate(debugPoint, new Vector3(x0, y1, z0), Quaternion.identity);
        obj.name = "Debug Point 1";
        obj = Instantiate(debugPoint, new Vector3(x0, y1, z1), Quaternion.identity);
        obj.name = "Debug Point 2";
        obj = Instantiate(debugPoint, new Vector3(x1, y1, z1), Quaternion.identity);
        obj.name = "Debug Point 3";
        obj = Instantiate(debugPoint, new Vector3(x1, y1, z0), Quaternion.identity);
        obj.name = "Debug Point 4";

        debugged = true;
    }
}
