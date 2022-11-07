using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Water.Attribute;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GenerateMesh : MonoBehaviour
{
    private MeshFilter meshFilter;
    private bool built = false;
    private Mesh mesh;
    
    public Vector3 dimensions = Vector3.one;
    public float TileScale = 1f;
    public float UVScale = 1f;
    
    [Flags] // This allows multiple flags to be set within the editor concurrently
    public enum TileFace : int
    {
        NegX = 1,
        PosX = 2,
        NegZ = 4,
        PosZ = 8
    }
    
    [FlagEnum]
    public TileFace IncludeFaces = TileFace.NegX | TileFace.NegZ | TileFace.PosX | TileFace.PosZ;

    [FlagEnum]
    public TileFace IncludeFoam = TileFace.NegX | TileFace.NegZ | TileFace.PosX | TileFace.PosZ;
    
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
                    float x0 = x * TileScale;
                    float x1 = x0 + TileScale;
                    float y0 = y * TileScale;
                    float y1 = y0 + TileScale;
                    float z0 = z * TileScale;
                    float z1 = z0 + TileScale;

                    // UV coordinates for the tile
                    // *UVScale is used to scale the UV coordinates
                    // /TileScale is used to scale the UV coordinates to the tile size
                    Vector3 position = transform.position;
                    float ux0 = x0 * UVScale / TileScale + position.x;
                    float ux1 = x1 * UVScale / TileScale + position.x;
                    float uy0 = y0 * UVScale / TileScale + position.y;
                    float uy1 = y1 * UVScale / TileScale + position.y;
                    float uz0 = z0 * UVScale / TileScale + position.z;
                    float uz1 = z1 * UVScale / TileScale + position.z;
                    
                    // check for edges
                    bool negX = x == 0;
                    bool posX = x == dimensions.x - 1;
                    bool negY = y == 0;
                    bool posY = y == dimensions.y - 1;
                    bool negZ = z == 0;
                    bool posZ = z == dimensions.z - 1;
                    bool negXnegZ = !negX && !negZ && x > 0 && z > 0;
                    bool negXposZ = !negX && !posZ && x > 0 && z < dimensions.z;
                    bool posXposZ = !posX && !posZ && x < dimensions.x && z < dimensions.z;
                    bool posXnegZ = !posX && !negZ && x < dimensions.x && z > 0;
                    bool faceNegX = negX && (IncludeFaces & TileFace.NegX) == TileFace.NegX;
                    bool facePosX = posX && (IncludeFaces & TileFace.PosX) == TileFace.PosX;
                    bool faceNegZ = negZ && (IncludeFaces & TileFace.NegZ) == TileFace.NegZ;
                    bool facePosZ = posZ && (IncludeFaces & TileFace.PosZ) == TileFace.PosZ;
                    bool foamNegX = negX && (IncludeFoam & TileFace.NegX) == TileFace.NegX;
                    bool foamPosX = posX && (IncludeFoam & TileFace.PosX) == TileFace.PosX;
                    bool foamNegZ = negZ && (IncludeFoam & TileFace.NegZ) == TileFace.NegZ;
                    bool foamPosZ = posZ && (IncludeFoam & TileFace.PosZ) == TileFace.PosZ;
                    bool foamNegXnegZ = negXnegZ && ((IncludeFoam & TileFace.NegX) == TileFace.NegX || (IncludeFoam & TileFace.NegZ) == TileFace.NegZ);
                    bool foamNegXposZ = negXposZ && ((IncludeFoam & TileFace.PosX) == TileFace.PosX || (IncludeFoam & TileFace.PosZ) == TileFace.PosZ);
                    bool foamPosXposZ = posXposZ && ((IncludeFoam & TileFace.NegZ) == TileFace.NegZ || (IncludeFoam & TileFace.PosZ) == TileFace.PosZ);
                    bool foamPosXnegZ = posXnegZ && ((IncludeFoam & TileFace.PosZ) == TileFace.PosZ || (IncludeFoam & TileFace.NegZ) == TileFace.NegZ);

                    
                    // create the top face
                        if (y == dimensions.y - 1)
                        {
                            vertices.Add(new Vector3(x0, y1, z0));
                            vertices.Add(new Vector3(x0, y1, z1));
                            vertices.Add(new Vector3(x1, y1, z1));
                            vertices.Add(new Vector3(x1, y1, z0));
                            
                            normals.Add(new Vector3(0, 1, 0));
                            normals.Add(new Vector3(0, 1, 0));
                            normals.Add(new Vector3(0, 1, 0));
                            normals.Add(new Vector3(0, 1, 0));
                            
                            uvs.Add(new Vector2(ux0, uz0));
                            uvs.Add(new Vector2(ux0, uz1));
                            uvs.Add(new Vector2(ux1, uz1));
                            uvs.Add(new Vector2(ux1, uz0));
                            
                            colors.Add(foamNegX || foamNegZ || foamNegXnegZ ? Color.red : Color.black);
                            colors.Add(foamNegX || foamPosZ || foamNegXposZ ? Color.red : Color.black);
                            colors.Add(foamPosX || foamPosZ || foamPosXposZ ? Color.red : Color.black);
                            colors.Add(foamPosX || foamNegZ || foamPosXnegZ ? Color.red : Color.black);
                            
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
}
