using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TilePRNGMapGenerator))]
public  class TilePRNGMapGeneratorEditor : Editor {

    public override void OnInspectorGUI()
    {



        TilePRNGMapGenerator map = target as TilePRNGMapGenerator;
        if (DrawDefaultInspector())
        {
            if (map.autoUpdate)
            {
                map.GenerateMap();
            }
        }

        if (GUILayout.Button("Generate Map"))
        {
            map.GenerateMap();
        }
    }
}
