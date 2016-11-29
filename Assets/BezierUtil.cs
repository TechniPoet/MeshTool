using UnityEngine;

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

    // use bernstein curves
    public static Vector3 GetPoint(Vector3[] pts, float t) {
        float omt = 1f - t;
        float omt2 = omt * omt;
        float t2 = t * t;
        return pts[0] * (omt2 * omt) +
               pts[1] * (3f * omt2 * t) +
               pts[2] * (3f * omt * t2) +
               pts[3] * (t2 * t);
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

    public static void Extrude(Mesh mesh, ExtrudeShape shape, OrientedPoint[] path) {
        int vertsInShape = shape.verts.Length;
        int segments = path.Length - 1;
        int edgeLoops = path.Length;
        int vertCount = vertsInShape * edgeLoops;
        int triCount = shape.Lines.Length * segments;
        int triIndexCount = triCount * 3;

        int[] triangleIndices = new int[triIndexCount];
        Vector3[] vertices = new Vector3[vertCount];
        Vector3[] normals = new Vector3[vertCount];
        Vector2[] uvs = new Vector2[vertCount];

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
    }
}
