using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class ExtrudeShape
{
    public Vector2[] verts;
    public Vector2[] normals;
    public float[] uCoords;

    IEnumerable<int> LineSegment(int i) {
        yield return i;
        yield return i + 1;
    }

    int[] lines;
    public int[] Lines {
        get {
            if (lines == null) {
                lines = Enumerable.Range(0, verts.Length - 1)
                    .SelectMany(i => LineSegment(i))
                    .ToArray();
            }

            return lines;
        }
    }
    
}
