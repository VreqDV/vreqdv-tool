using UnityEngine;
using UnityEditor;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class JsonTransformSync
{
    private static string GetLatestArticleJsonPath()
    {
        string specsDir = "Assets/VReqDV/specifications";
        if (!Directory.Exists(specsDir)) return null;

        int latestVersion = -1;
        string latestPath = null;

        foreach (string dir in Directory.GetDirectories(specsDir))
        {
            string dirName = new DirectoryInfo(dir).Name;
            if (dirName.StartsWith("version_"))
            {
                if (int.TryParse(dirName.Substring(8), out int v))
                {
                    string possiblePath = Path.Combine(dir, "article.json").Replace("\\", "/");
                    if (File.Exists(possiblePath) && v > latestVersion)
                    {
                        latestVersion = v;
                        latestPath = possiblePath;
                    }
                }
            }
        }
        return latestPath;
    }

    [MenuItem("VReqDV/Sync/Update Latest JSON from Scene (Only Changed)")]
    public static void UpdateLatestJsonFromSceneChanged() => DoUpdateJsonFromScene(true, true);

    [MenuItem("VReqDV/Sync/Update Latest JSON from Scene (Overwrite All)")]
    public static void UpdateLatestJsonFromSceneAll() => DoUpdateJsonFromScene(false, true);

    [MenuItem("VReqDV/Sync/Update Custom JSON from Scene...")]
    public static void UpdateCustomJsonFromScene() => DoUpdateJsonFromScene(true, false);

    private static void DoUpdateJsonFromScene(bool onlyChanges, bool autoLatest)
    {
        string path;
        if (autoLatest)
        {
            path = GetLatestArticleJsonPath();
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("Could not find any article.json in version folders.");
                return;
            }
        }
        else
        {
            path = EditorUtility.OpenFilePanel("Select article.json", "Assets/VReqDV/specifications", "json");
            if (string.IsNullOrEmpty(path)) return;
        }

        string json = File.ReadAllText(path);
        JObject root = JObject.Parse(json);
        JArray articles = (JArray)root["articles"];

        if (articles == null)
        {
            Debug.LogError("No 'articles' array found in the selected JSON.");
            return;
        }

        int updatedCount = 0;

        foreach (JObject article in articles)
        {
            string objName = article["_objectname"]?.ToString();
            if (string.IsNullOrEmpty(objName)) continue;

            // Find the object in the active Scene
            GameObject sceneObj = GameObject.Find(objName);
            if (sceneObj != null)
            {
                bool hasChanged = true;

                if (onlyChanges)
                {
                    hasChanged = false;
                    var pNode = article["Transform_initialpos"];
                    var rNode = article["Transform_initialrotation"];
                    var sNode = article["Transform_objectscale"];

                    if (pNode == null || rNode == null || sNode == null || 
                        pNode.Type == JTokenType.Null || rNode.Type == JTokenType.Null || sNode.Type == JTokenType.Null)
                    {
                        hasChanged = true;
                    }
                    else
                    {
                        Vector3 jPos = new Vector3(pNode["x"]?.Value<float>() ?? 0, pNode["y"]?.Value<float>() ?? 0, pNode["z"]?.Value<float>() ?? 0);
                        Vector3 jRot = new Vector3(rNode["x"]?.Value<float>() ?? 0, rNode["y"]?.Value<float>() ?? 0, rNode["z"]?.Value<float>() ?? 0);
                        Vector3 jScl = new Vector3(sNode["x"]?.Value<float>() ?? 1, sNode["y"]?.Value<float>() ?? 1, sNode["z"]?.Value<float>() ?? 1);

                        // If any coordinate differs by more than 0.001, consider it changed
                        if (Vector3.Distance(sceneObj.transform.position, jPos) > 0.001f ||
                            Vector3.Distance(sceneObj.transform.eulerAngles, jRot) > 0.001f ||
                            Vector3.Distance(sceneObj.transform.localScale, jScl) > 0.001f)
                        {
                            hasChanged = true;
                        }
                    }
                }

                if (hasChanged)
                {
                    // Ensure Transform nodes exist if they were null
                    if (article["Transform_initialpos"] == null || article["Transform_initialpos"].Type == JTokenType.Null)
                        article["Transform_initialpos"] = new JObject();
                    
                    if (article["Transform_initialrotation"] == null || article["Transform_initialrotation"].Type == JTokenType.Null)
                        article["Transform_initialrotation"] = new JObject();
                    
                    if (article["Transform_objectscale"] == null || article["Transform_objectscale"].Type == JTokenType.Null)
                        article["Transform_objectscale"] = new JObject();

                    // Update Position
                    article["Transform_initialpos"]["x"] = sceneObj.transform.position.x.ToString("F2");
                    article["Transform_initialpos"]["y"] = sceneObj.transform.position.y.ToString("F2");
                    article["Transform_initialpos"]["z"] = sceneObj.transform.position.z.ToString("F2");

                    // Update Rotation
                    article["Transform_initialrotation"]["x"] = sceneObj.transform.eulerAngles.x.ToString("F2");
                    article["Transform_initialrotation"]["y"] = sceneObj.transform.eulerAngles.y.ToString("F2");
                    article["Transform_initialrotation"]["z"] = sceneObj.transform.eulerAngles.z.ToString("F2");

                    // Update Scale
                    article["Transform_objectscale"]["x"] = sceneObj.transform.localScale.x.ToString("F2");
                    article["Transform_objectscale"]["y"] = sceneObj.transform.localScale.y.ToString("F2");
                    article["Transform_objectscale"]["z"] = sceneObj.transform.localScale.z.ToString("F2");

                    updatedCount++;
                }
            }
        }

        File.WriteAllText(path, JsonConvert.SerializeObject(root, Formatting.Indented));
        AssetDatabase.Refresh();
        Debug.Log($"Successfully updated {updatedCount} objects in {Path.GetFileName(path)} based on the Scene!");
    }
}
