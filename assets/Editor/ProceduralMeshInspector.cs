using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(ProceduralMesh))]
[CanEditMultipleObjects]
public class ProceduralMeshInspector : Editor {
    private ProceduralMesh m;
    private Vector2[] editorVerts;
    PreviewRenderUtility _util;


    public override void OnInspectorGUI() {
        SetRefs();
        DrawDefaultInspector();
    }


    void SetRefs() {
        if (m == null) {
            m = target as ProceduralMesh;
        }
    }


    void OnSceneGUI() {
        SetRefs();
        Show2DShape();
    }


    void Show2DShape() {
        editorVerts = new Vector2[m.GetVertCount()];
        //EditorGUI.DrawRect(new Rect(20,0, 150, 150), Color.white);
    }

    public override void OnPreviewSettings() {
        if (GUILayout.Button("Reset Camera", EditorStyles.whiteMiniLabel)) {

        }
            
    }

    public override void OnInteractivePreviewGUI(Rect r, GUIStyle background) {
        base.OnInteractivePreviewGUI(r, background);
        if (_util == null) {
            _util = new PreviewRenderUtility();
        }
        _util.BeginPreview(r, background);
        GUILayout.Button("asdf");
        Texture resultRender = _util.EndPreview();
        GUI.DrawTexture(r, resultRender, ScaleMode.StretchToFill, false);

    }

    void OnDestroy() {
        //Gotta clean up after yourself!
        if (_util != null)
            _util.Cleanup();
    }
}