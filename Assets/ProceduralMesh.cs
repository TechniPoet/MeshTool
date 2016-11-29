using UnityEngine;
using System;

public class ProceduralMesh : UniqueMesh
{

	public void Awake()
	{
        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf.sharedMesh == null) {
            mf.sharedMesh = new Mesh();
        }
        Mesh mesh = mf.sharedMesh;
        Vector3[] vertices = new Vector3[] {
            new Vector3(1, 0, 1),
            new Vector3(-1, 0, 1),
            new Vector3(1, 0, -1),
            new Vector3(-1, 0, -1)
        };
        Vector3[] normals = new Vector3[] {
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 0)
        };
        Vector2[] uvs = new Vector2[] {
            new Vector2(0, 1),
            new Vector2(0, 0),
            new Vector2(1, 1),
            new Vector2(1, 0)
        };
        int[] triangles = new int[] {
            0, 2, 3,
            3, 1, 0
        };

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;


        //exctrude shape.

        ExtrudeShape shape = new ExtrudeShape();
        Vector3[] verts;
        Vector3[] norms;
        float[] us;
        int[] lines = new int[] {
            0, 1,
            2,3,
            3,4,
            4,5,
        };



    }

    
}
