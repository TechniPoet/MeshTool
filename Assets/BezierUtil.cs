using UnityEngine;
using System.Collections.Generic;


public static class BezierUtil
{
    // Un-optimized version
    /*
    Vector3 GetPoint( Vector3[] pts, float t ) {
        Vector3 a = Vector3.Lerp(pts[0], pts[1], t);
        Vector3 b = Vector3.Lerp(pts[1], pts[2], t);
        Vector3 c = Vector3.Lerp(pts[2], pts[3], t);
        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(d, e, t);
    }
    */

    public static Vector3 GetPoint(Vector3[] pts, float t, out Vector3 tangent, out Vector3 normal, out Quaternion orientation) {
        float t2 = t * t;
        float t3 = t2 * t;
        float it = (1 - t);
        float it2 = it * it;
        float it3 = it * it * it;

        tangent = CalculateTangent(pts, t, t2, it2);
        normal = CalculateNormal(tangent, Vector3.up);
        orientation = Quaternion.LookRotation(tangent, normal);

        return CalculatePoint(pts, t, t2, t3, it, it2, it3);
    }

    public static Vector3 GetPoint(Vector3[] pts, float t, out Vector3 tangent, out Vector3 normal) {
        float t2 = t * t;
        float t3 = t2 * t;
        float it = (1 - t);
        float it2 = it * it;
        float it3 = it * it * it;

        tangent = CalculateTangent(pts, t, t2, it2);
        normal = CalculateNormal(tangent, Vector3.up);

        return CalculatePoint(pts, t, t2, t3, it, it2, it3);
    }

    public static Vector3 GetPoint(Vector3[] pts, float t, out Vector3 tangent) {
        float t2 = t * t;
        float t3 = t2 * t;
        float it = (1 - t);
        float it2 = it * it;
        float it3 = it * it * it;

        tangent = CalculateTangent(pts, t, t2, it2);
        return CalculatePoint(pts, t, t2, t3, it, it2, it3);
    }

    public static Vector3 GetPoint(Vector3[] pts, float t) {
        float t2 = t * t;
        float t3 = t2 * t;
        float it = (1 - t);
        float it2 = it * it;
        float it3 = it * it * it;

        return CalculatePoint(pts, t, t2, t3, it, it2, it3);
    }


    static Vector3 CalculateNormal(Vector3 tangent, Vector3 up) {
        Vector3 binormal = Vector3.Cross(up, tangent);
        return Vector3.Cross(tangent, binormal);
    }

    static Vector3 CalculateTangent(Vector3[] pts, float t, float t2, float it2) {
        return (pts[0] * -it2 +
            pts[1] * (t * (3 * t - 4) + 1) +
            pts[2] * (-3 * t2 + t * 2) +
            pts[3] * t2).normalized;
    }

    static Vector3 CalculatePoint(Vector3[] pts, float t, float t2, float t3, float it, float it2, float it3) {
        return pts[0] * (it3) +
            pts[1] * (3 * it2 * t) +
            pts[2] * (3 * it * t2) +
            pts[3] * t3;
    }

    /*
    public static Vector3 GetPoint(Vector3[] pts, float t) {
        float t2 = t * t;
        float t3 = t2 * t;
        float it = (1 - t);
        float it2 = it * it;
        float it3 = it * it * it;

        return pts[0] * (it3) +
            pts[1] * (3 * it2 * t) +
            pts[2] * (3 * it * t2) +
            pts[3] * t3; ;
    }*/
    public static OrientedPoint GetOrientedPoint(Vector3[] pts, float t) {
        Vector3 tangent, normal;
        Quaternion orientation;

        Vector3 point = GetPoint(pts, t, out tangent, out normal, out orientation);

        return new OrientedPoint(point, orientation);
    }


    public static OrientedPoint GetOrientedPoint (Vector3[] pts, float t, float[] samples) {
        Vector3 tangent, normal;
        Quaternion orientation;

        Vector3 point = GetPoint(pts, t, out tangent, out normal, out orientation);

        return new OrientedPoint(point, orientation, samples.Sample(t));
    }


    public static float[] GenerateSamples (Vector3[] points) {
        Vector3 prevPoint = points[0];
        Vector3 pt;
        float total = 0;

        List<float> samples = new List<float>(10) { 0 };
        float step = 1.0f / 10.0f;
        for (float f = step; f < 1.0f; f += step) {
            pt = GetPoint(points, f);
            total += (pt - prevPoint).magnitude;
            samples.Add(total);

            prevPoint = pt;
        }

        pt = GetPoint(points, 1);
        samples.Add(total + (pt - prevPoint).magnitude);

        return samples.ToArray();
    }


    public static Vector3 GetTangent(Vector3[] pts, float t) {
        float omt = 1f - t;
        float omt2 = omt * omt;
        float t2 = t * t;
        Vector3 tangent =
            pts[0] * (-omt2) +
            pts[1] * (3 * t2 + 2 * t) +
            pts[2] * (-3 * t2 + 2 * t) +
            pts[3] * (t2);
        return tangent.normalized;
    }

    public static Vector3 GetNormal2D(Vector3[] pts, float t) {
        Vector3 tng = GetTangent(pts, t);
        return new Vector3(-tng.y, tng.x, 0f);
    }


    public static Vector3 GetNormal3D(Vector3[] pts, float t, Vector3 up) {
        Vector3 tng = GetTangent(pts, t);
        Vector3 binormal = Vector3.Cross(up, tng).normalized;
        return Vector3.Cross(tng, binormal);
    }


    public static Quaternion GetOrientation2D(Vector3[] pts, float t) {
        Vector3 tng = GetTangent(pts, t);
        Vector3 nrm = GetNormal2D(pts, t);
        return Quaternion.LookRotation(tng, nrm);
    }

    public static Quaternion GetOrientation3D(Vector3[] pts, float t, Vector3 up) {
        Vector3 tng = GetTangent(pts, t);
        Vector3 nrm = GetNormal3D(pts, t, up);
        return Quaternion.LookRotation(tng, nrm);
    }


    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t) {
        return
            2f * (1f - t) * (p1 - p0) +
            2f * t * (p2 - p1);
    }

	public static float Cross(Vector2 p, Vector2 q)
	{
		return (p.x*q.y - p.y*q.x);
	}


    public static void Extrude(ref Mesh mesh, ExtrudeShape shape, OrientedPoint[] path) {
        int vertsInShape = shape.verts.Length;
        int segments = path.Length - 1;
        int edgeLoops = path.Length;
        int vertCount = vertsInShape * edgeLoops;
        int triCount = shape.Lines.Length * segments;
        int triIndexCount = triCount * 3;
        //Debug.Log(string.Format("VertsInShape:{0}\nsegments:{1}\nedgeLoops:{2}\nvertCount:{3}\ntriCount:{4}\ntriIndexCount:{5}",
        //    vertsInShape, segments, edgeLoops, vertCount, triCount, triIndexCount));
        int[] triangleIndices = new int[triIndexCount];
        Vector3[] vertices = new Vector3[vertCount];
        Vector3[] normals = new Vector3[vertCount];
        Vector2[] uvs = new Vector2[vertCount];

        // Generate all of the vertices and normals
        for (int i = 0; i < path.Length; i++) {
            int offset = i * vertsInShape;
            string debug = string.Format("pathPoint:{0}\noffset:{1}\n", i, offset);
            for (int j = 0; j < vertsInShape; j++) {
                int id = offset + j;
                vertices[id] = path[i].LocalToWorld(shape.verts[j]);
                //vertices[id] = shape.verts[j];
                normals[id] = path[i].LocalToWorldDirection(shape.normals[j]);
                //normals[id] = shape.normals[j];
                uvs[id] = new Vector2(shape.uCoords[j], path[i].vCoordinate);
                debug += string.Format("id:{0}\nvert:{1}\nnormals:{2}\nuv:{3}\n", id, vertices[id], normals[id], uvs[id]);
            }
            //Debug.Log(debug);
        }

        //Generation
        int ti = 0;
        for (int i = 0; i < segments; i++) {
            int offset = i * vertsInShape;
            for (int l = 0; l < shape.Lines.Length; l += 2) {
                int a = offset + shape.Lines[l];
                int b = offset + shape.Lines[l] + vertsInShape;
                int c = offset + shape.Lines[l + 1] + vertsInShape;
                int d = offset + shape.Lines[l + 1];
                triangleIndices[ti++] = a;
                triangleIndices[ti++] = b;
                triangleIndices[ti++] = c;
                triangleIndices[ti++] = c;
                triangleIndices[ti++] = d;
                triangleIndices[ti++] = a;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangleIndices;
        mesh.normals = normals;
        mesh.uv = uvs;
        string debugs = "Final Verts\n";
        for (int i = 0; i < mesh.vertices.Length; i++) {
            debugs += string.Format("vert {0}: {1}\n", i, mesh.vertices[i]);
        }
        //Debug.Log(debugs);
    }
}
