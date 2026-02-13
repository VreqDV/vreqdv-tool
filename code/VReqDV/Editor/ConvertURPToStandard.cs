using UnityEditor;
using UnityEngine;

public class ConvertURPToStandard : EditorWindow
{
    [MenuItem("Tools/Convert URP Materials to Standard")]
    static void Convert()
    {
        string[] guids = AssetDatabase.FindAssets("t:Material");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat != null && mat.shader != null &&
                mat.shader.name.Contains("Universal Render Pipeline"))
            {
                mat.shader = Shader.Find("Standard");
                Texture baseMap = mat.GetTexture("_BaseMap");
                if (baseMap) mat.SetTexture("_MainTex", baseMap);

                Texture normalMap = mat.GetTexture("_BumpMap");
                if (normalMap) mat.SetTexture("_BumpMap", normalMap);

                mat.SetFloat("_Glossiness", 0.5f);
                EditorUtility.SetDirty(mat);
            }
        }
        AssetDatabase.SaveAssets();
        Debug.Log("✅ All URP materials converted to Standard shader.");
    }
}

