using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

[CustomEditor(typeof(MeshTool))]
public class MeshToolInspector : Editor
{
    MeshTool m;
    string name = "";
    string oldName = "";


    public override void OnInspectorGUI () {
        SetRefs();
        DrawDefaultInspector();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Mesh Name:", GUILayout.Width(75));
        name = GUILayout.TextField(name);
        if (name != oldName) {
            Undo.RecordObject(m, "Mesh Name");
            EditorUtility.SetDirty(m);
        }
        GUILayout.EndHorizontal();
        if (name.Replace(" ", "") == string.Empty) {
            EditorGUILayout.HelpBox("Mesh name can't be empty", MessageType.Error);
        }
        if (GUILayout.Button("Save Mesh")) {
            string file = FileUtil.GetProjectRelativePath(EditorUtility.SaveFilePanel("Save Mesh","", name, "asset"));
            AssetDatabase.CreateAsset(m._PMesh.mf.sharedMesh, file);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }


    void SetRefs () {
        if (m == null) {
            m = target as MeshTool;
        }
    }
}
