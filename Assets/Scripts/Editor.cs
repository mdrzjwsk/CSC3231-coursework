using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TerrainGenerator gen = (TerrainGenerator)target;
        if (DrawDefaultInspector())
        {
            if (gen.autoUpdate)
            {
                gen.GenerateMap();
            }
        }
        if (GUILayout.Button("Generate"))
        {
            gen.GenerateMap();
        }
    }
}
