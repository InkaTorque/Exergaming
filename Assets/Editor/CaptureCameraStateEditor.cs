using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MinigameOverseer))]
public class CaptureCameraStateEditor : Editor {

    public override void OnInspectorGUI()
    {
        MinigameOverseer script = target as MinigameOverseer;
        DrawDefaultInspector(); 
        if (GUILayout.Button("Capture Cam Pos"))
        {
            script.SaveCameraState();
        }
    }
}
