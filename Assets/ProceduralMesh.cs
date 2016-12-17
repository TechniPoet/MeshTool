using UnityEngine;
using System.Collections.Generic;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ProceduralMesh : UniqueMesh
{
    Spline s;
    [System.NonSerialized]
    public MeshFilter mf;
    
    new Mesh mesh;
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

    public List<Vector2> verts;

    public int GetVertCount() {
        return verts.Count;
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
        if (mf.sharedMesh == null)
            mf.sharedMesh = new Mesh();
        mesh = mf.sharedMesh;
        s = Parent._Spline;
        if (shape == null) {
            shape = new ExtrudeShape();
        }
        shape.verts = verts.ToArray();
        shape.normals = new Vector2[GetVertCount()];
        shape.uCoords = new float[GetVertCount()];

        float uLength = 0;
        float[] pointPos = new float[GetVertCount()];
        pointPos[0] = 0;
        for (int i = 1; i < verts.Count; i++) {
            uLength += Vector2.Distance(verts[i-1], verts[i]);
            pointPos[i] = uLength;
        }
        for (int i = 0; i < GetVertCount(); i++) {
            shape.uCoords[i] = pointPos[i] / uLength;
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

        float[] samples = BezierUtil.GenerateSamples(s.curvePoints.ToArray());
		
		if (s != null) {
            OrientedPoint[] op = new OrientedPoint[s.curvePoints.Count];
            string debug = "";
            for (int i = 0; i < op.Length; i++) {
                //op[i] = BezierUtil.GetOrientedPoint(s.curvePoints.ToArray(), ((float)i / (float)op.Length), samples);
                op[i] = new OrientedPoint(s.curvePoints[i], Quaternion.identity, samples.Sample(((float)i / (float)op.Length)));
                //debug += " " + op[i].vCoordinate;
            }
            //Debug.Log(debug);
            BezierUtil.Extrude(ref mesh, shape, op);
            //mesh.RecalculateNormals();
        } else {
            Debug.Log("no s");
        }
    }

    
}
