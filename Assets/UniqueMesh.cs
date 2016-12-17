using UnityEngine;

/// <summary>
/// Copied from Unite 2015 Talk
/// </summary>
public class UniqueMesh : MonoBehaviour
{
    [HideInInspector]
    int ownerID; // Unique id
    MeshFilter _mf;
    MeshFilter mf {
        get {
            _mf = _mf == null ? GetComponent<MeshFilter>() : _mf;
            _mf = _mf == null ? gameObject.AddComponent<MeshFilter>() : _mf;
            return _mf;
        }
    }
    [System.NonSerialized]
    public Mesh meshToSave;
    Mesh _mesh;
    protected Mesh mesh {
        get {
            bool isOwner = ownerID == gameObject.GetInstanceID();
            if ( mf.sharedMesh == null || !isOwner) {
                mf.sharedMesh = _mesh = new Mesh();
                ownerID = gameObject.GetInstanceID();
                _mesh.name = string.Format("Mesh [{0}]", ownerID);
            }
            return _mesh;
        }
    }
}
