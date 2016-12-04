using UnityEngine;
using System;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ProceduralMesh : UniqueMesh
{
    Spline s;
    MeshFilter mf;
    Mesh mesh;
    ExtrudeShape shape;

    MeshTool parent;
    public MeshTool Parent {
        get {
            if (parent == null) {
                parent = GetComponentInParent<MeshTool>();
            }
            return parent;
        }
    }

    public Vector2[] verts;

    public int GetVertCount() {
        return verts.Length;
    }


    public Vector2 GetVert(int ind) {
        return verts[ind];
    }
    

    public void SetVert(int ind, Vector2 val) {
        verts[ind] = val;
    }

    public void GenerateMesh() {
        mf = GetComponent<MeshFilter>();
        mf.mesh = null;
        mf.sharedMesh = new Mesh();
        mesh = mf.sharedMesh;
        s = Parent._Spline;
        shape = new ExtrudeShape();
        shape.verts = verts;
        shape.normals = new Vector2[GetVertCount()];
        shape.uCoords = new float[GetVertCount()];

        for (int i = 0; i < GetVertCount(); i++) {
            shape.uCoords[i] = i / GetVertCount();
			float dx, dy;
			Vector2 normal;
			if (i == 0)
			{
				dx = shape.verts[1].x - shape.verts[0].x;
				dy = shape.verts[1].y - shape.verts[0].y;
				normal = new Vector2(dy, -dx);
				
				shape.normals[i] = BezierUtil.Cross(new Vector2(dx, dy), normal) > 0 ? normal : new Vector2(-dy, dx);
			}
			else if (i == GetVertCount() - 1)
			{
				dx = shape.verts[0].x - shape.verts[i].x;
				dy = shape.verts[0].y - shape.verts[i].y;
				normal = new Vector2(dy, -dx);
			}
			else
			{
				dx = shape.verts[i + 1].x - shape.verts[i].x;
				dy = shape.verts[i + 1].y - shape.verts[i].y;
				normal = new Vector2(dy, -dx);
			}
			normal = BezierUtil.Cross(new Vector2(dy, dx), normal) > 0 ? normal : new Vector2(-dy, dx);
			shape.normals[i] = normal.normalized;
			//print(string.Format("{0}:{1}", i, normal.normalized));
		}

		
		if (s != null) {
            OrientedPoint[] op = new OrientedPoint[s.curvePoints.Count];
            
            for (int i = 0; i < op.Length; i++) {
                op[i] = new OrientedPoint(s.curvePoints[i], Quaternion.identity, (i / s.ControlPointCount));
            }
            BezierUtil.Extrude(ref mesh, shape, op);
            //mesh.RecalculateNormals();
        } else {
            Debug.Log("no s");
        }
    }

    
}
