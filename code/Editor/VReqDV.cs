using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Newtonsoft.Json;
using UnityEditor.SceneManagement;
using Newtonsoft.Json.Linq;

public class MainMenu : EditorWindow
{
    private static MainMenu window;
    [MenuItem("Window/VReqDV")]
    public static void ShowWindow()
    {
        window = GetWindow<MainMenu>("VReqDV");
    }

    private onScreenState screenState;
    
    private ArticleList objectSpecifications;
    private ActionResponseList actionSpecifications;
    private ArticleList compareObjectSpecifications;
    private ActionResponseList compareActionSpecifications;
    // private ActionResponseData actionSpecifications;

    private Vector2 scrollPositionObject;
    private Vector2 scrollPositionAction;

    // private static int total_versions = screenState.total_versions;
    private int selected_display_component = 0;
    private string[] version_list;
    private static string[] versionSpecs;
    private int compare_version = 0;

    private void Initialize()
    {
        versionSpecs = Directory.GetDirectories("Assets/specifications");
        screenState.total_versions = versionSpecs.Length;
        version_list = new string[screenState.total_versions + 1];
        
        for (int i = 0; i <= screenState.total_versions; i++)
        {
            version_list[i] = i.ToString();
            // version_list.Add(i);
        }
    }

    private GUIStyle setFont(int x)
    {
        GUIStyle customLabel = new GUIStyle(EditorStyles.label);
        customLabel.fontSize = x;
        return customLabel;
    }

    private void OnEnable()
    {
        screenState = new onScreenState();  
        window = this;

        Initialize();
    }

    private void OnGUI()
    {
        Initialize();
        if(screenState.total_versions == 0)
        {
            // If no current versions are saved
            // and specifications found
            // then total versions is null
            // Show only save version button
            // to start tracking versions
            GUILayout.Label("To start using VReqDV to track your project versions, upload the project specifications in a new version, or save the contents of the current scene to a new version.");
            if (GUILayout.Button("Save Version"))
            {
                screenState.total_versions++;
                screenState.curr_version = screenState.total_versions; 
                SaveVersion(screenState.curr_version);
                SaveSceneToPrefab(screenState.curr_version);
                window.Repaint();
            }
            // if(GUILayout.Button("Add Specifications"))
            // {
            //     // Versions.save()
            //     private Article article = new Article();
            //     private ArticleList new_specs = new ArticleList(); 
            //     new_specs.articles.Add(article);
            //     DisplayArticleForm(new_specs);
            //     GUILayout.Label("hello");
            // }
        }

        else
        {
            // Show the current version specifications
            GUILayout.BeginHorizontal();
            GUILayout.Label("Current Version: "+ screenState.curr_version, setFont(14));
            GUILayout.Label("Total Versions: " + screenState.total_versions, setFont(14));
            // if(GUILayout.Button("Create Scene"), GUILayout.Width(200))
            // {
            //     CreateScene(screenState.curr_version);
            // }
            
            if (GUILayout.Button("Save New Version", GUILayout.Width(200)))
            {
                screenState.total_versions++;
                screenState.curr_version = screenState.total_versions; 
                SaveVersion(screenState.curr_version);
                SaveSceneToPrefab(screenState.curr_version);
                window.Repaint();
            }
            if(GUILayout.Button("Save to Current Version", GUILayout.Width(200)))
            {
                SaveVersion(screenState.curr_version);
                SaveSceneToPrefab(screenState.curr_version);
                window.Repaint();
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Change Current Version:", setFont(12));            
            screenState.curr_version = EditorGUILayout.Popup(screenState.curr_version, version_list, GUILayout.Width(100));
            if(GUILayout.Button("Display Mock-up", GUILayout.Width(200)))
            {
                OpenScene(screenState.curr_version);
            }
            GUILayout.Label("Compare with Version:", setFont(12));
            compare_version = EditorGUILayout.Popup(compare_version, version_list, GUILayout.Width(100));
            if(GUILayout.Button("Display Comparison", GUILayout.Width(200)))
            {
                CreateComparisonScene(screenState.curr_version, compare_version);
            }
            GUILayout.EndHorizontal();

            try
            {
                string file = "Assets/specifications/version_" + screenState.curr_version + "/article.json";
                string objectData = File.ReadAllText(file);
                objectSpecifications = JsonConvert.DeserializeObject<ArticleList>(objectData);
            }
            catch (FileNotFoundException)
            {
                objectSpecifications = new ArticleList { articles = new List<Article> { new Article { _objectname = "Error", _slabel = "File not found"} } };
            }
            catch (JsonException)
            {
                objectSpecifications = new ArticleList { articles = new List<Article> { new Article { _objectname = "Error", _slabel = "Failed to parse JSON"} } };
            }

            try
            {
                string file = "Assets/specifications/version_" + screenState.curr_version + "/action-response.json";
                string actionData = File.ReadAllText(file);
                actionSpecifications = JsonConvert.DeserializeObject<ActionResponseList>(actionData);
            }
            catch (FileNotFoundException)
            {
                actionSpecifications = new ActionResponseList { ObjAction = new List<ActionResponse> { new ActionResponse { actresid = "Error - File not found"} } };
            }
            catch (JsonException)
            {
                actionSpecifications = new ActionResponseList { ObjAction = new List<ActionResponse> { new ActionResponse { actresid = "Error - Failed to parse JSON"} } };
            }

            if(compare_version != 0)
            {
                try
                {
                    string file = "Assets/specifications/version_" + compare_version + "/article.json";
                    string objectData = File.ReadAllText(file);
                    compareObjectSpecifications = JsonConvert.DeserializeObject<ArticleList>(objectData);
                }
                catch (FileNotFoundException)
                {
                    compareObjectSpecifications = new ArticleList { articles = new List<Article> { new Article { _objectname = "Error", _slabel = "File not found"} } };
                }
                catch (JsonException)
                {
                    compareObjectSpecifications = new ArticleList { articles = new List<Article> { new Article { _objectname = "Error", _slabel = "Failed to parse JSON"} } };
                }

                try
                {
                    string file = "Assets/specifications/version_" + compare_version + "/action-response.json";
                    string actionData = File.ReadAllText(file);
                    compareActionSpecifications = JsonConvert.DeserializeObject<ActionResponseList>(actionData);
                }
                catch (FileNotFoundException)
                {
                    compareActionSpecifications = new ActionResponseList { ObjAction = new List<ActionResponse> { new ActionResponse { actresid = "Error - File not found"} } };
                }
                catch (JsonException)
                {
                    compareActionSpecifications = new ActionResponseList { ObjAction = new List<ActionResponse> { new ActionResponse { actresid = "Error - Failed to parse JSON"} } };
                }
            }

            string[] list = new string[] { "Assets", "Actions" };
            // int selected_display_component = 0;
            selected_display_component = EditorGUILayout.Popup("Select Component", selected_display_component, list);

            GUILayout.BeginHorizontal();
            
            // Scrollable area for object data
            scrollPositionObject = EditorGUILayout.BeginScrollView(scrollPositionObject, GUILayout.Height(400), GUILayout.Width(position.width / 2));
            
            if (objectSpecifications != null && objectSpecifications.articles != null)
            {
                if (list[selected_display_component] == "Assets")
                    DisplayArticleForm(objectSpecifications.articles, screenState.curr_version); 
            }
            if(actionSpecifications != null && actionSpecifications.ObjAction != null)
            {
                if (list[selected_display_component] == "Actions")
                    DisplayActionForm(actionSpecifications.ObjAction, screenState.curr_version);
            }
            EditorGUILayout.EndScrollView();
            
            // compare with
            if(compare_version != 0)
            {
                scrollPositionObject = EditorGUILayout.BeginScrollView(scrollPositionObject, GUILayout.Height(400), GUILayout.Width(position.width / 2));
                
                if (compareObjectSpecifications != null && compareObjectSpecifications.articles != null)
                {
                    if (list[selected_display_component] == "Assets")
                        DisplayArticleForm(compareObjectSpecifications.articles, compare_version);
                }
                if(compareActionSpecifications != null && compareActionSpecifications.ObjAction != null)
                {
                    if (list[selected_display_component] == "Actions")
                        DisplayActionForm(compareActionSpecifications.ObjAction, compare_version);
                }
                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndHorizontal();
        }
    }

    // private void DisplayForm<T>(int display_spec, List<T> list, int version)
    // {
    //     if(display_spec == 0)
    //     {
    //         List <Article> articles = list as List <Article>;
    //         DisplayArticleForm(articles, version);
    //     }

    //     if(display_spec == 1)
    //     {
    //         List <ActionResponse> actions = list as List <ActionResponse>;
    //         DisplayActionForm(actions, version);
    //     }    
    // }

    // private void CreateScene(int version_no)
    // {
    //     SceneConfig sceneConfig = File.ReadAllText("Assets/specifications/version_" + version_no + "/scene.json");

    // }

    private void OpenScene(int version_no)
    {
        string folder_path = "Assets/specifications/version_" + version_no;
        if(Directory.Exists(folder_path))
        {
            CreateObjects(folder_path);
            CreateActions(folder_path);
        }
        else
        {
            Debug.Log("Version Directory not found");
        }
    }

    private void CreateObjects(string directory_path)
    {
        string jsonData = File.ReadAllText(directory_path + "/article.json");
        ArticleList objectDataList = JsonConvert.DeserializeObject<ArticleList>(jsonData);
        // Debug.Log("started creating objects");
        foreach (Article objectData in objectDataList.articles)
        {
            // Debug.Log(objectData._objectname);
            GameObject go = GameObject.CreatePrimitive(GetPrimitiveTypeByString(objectData.shape));
            go.name = objectData._objectname;
            // go.transform.position = new Vector3(float.Parse(objectData.Transform_initialpos["#x"]), float.Parse(objectData.Transform_initialpos["#y"]), float.Parse(objectData.Transform_initialpos["#z"]));
            // go.transform.rotation = Quaternion.Euler(float.Parse(objectData.Transform_initialrotation["x"]), float.Parse(objectData.Transform_initialrotation["y"]), float.Parse(objectData.Transform_initialrotation["z"]));
            // go.transform.localScale = new Vector3(float.Parse(objectData.Transform_objectscale["#x"]), float.Parse(objectData.Transform_objectscale["#y"]), float.Parse(objectData.Transform_objectscale["#z"]));
            go.transform.position = new Vector3(
                float.Parse(objectData.Transform_initialpos.x), 
                float.Parse(objectData.Transform_initialpos.y), 
                float.Parse(objectData.Transform_initialpos.z)
            );

            go.transform.rotation = Quaternion.Euler(
                float.Parse(objectData.Transform_initialrotation.x), 
                float.Parse(objectData.Transform_initialrotation.y), 
                float.Parse(objectData.Transform_initialrotation.z)
            );

            go.transform.localScale = new Vector3(
                float.Parse(objectData.Transform_objectscale.x), 
                float.Parse(objectData.Transform_objectscale.y), 
                float.Parse(objectData.Transform_objectscale.z)
            );

            // AddActionResponse(go, objectData._objectname);
            // createdObjects.Add(go);
        }
    }

    private void CreateActions(string directory_path)
    {
        // Read the action-response JSON file
        string jsonData = File.ReadAllText(directory_path + "/action-response.json");
        ActionResponseList actionResponseData = JsonConvert.DeserializeObject<ActionResponseList>(jsonData);

        // Iterate through each action-response and apply corresponding actions
        foreach (ActionResponse actionResponse in actionResponseData.ObjAction)
        {
            Debug.Log(actionResponse.actresid);
            GameObject sourceObject = GameObject.Find(actionResponse.trigger_event.sourceObj);
            GameObject targetObject = GameObject.Find(actionResponse.response_event.targetObj);

            if (sourceObject != null && targetObject != null)
            {
                // Rigidbody rb = targetObject.GetComponent<Rigidbody>();
                ActionComponent actionComponent = sourceObject.AddComponent<ActionComponent>();
                Debug.Log("No problem so far");
                TriggerTemplate trigger = CreateTrigger(actionResponse.trigger_event);
                ResponseTemplate response = CreateResponse(actionResponse.response_event);
                Debug.Log("done creations");
                actionComponent.trigger = trigger;
                actionComponent.response = response;
                actionComponent.targetObject = targetObject;
                #if UNITY_EDITOR
                if(trigger != null)
                    UnityEditor.AssetDatabase.CreateAsset(trigger, $"Assets/Triggers/{actionResponse.actresid}_Trigger.asset");
                if(response != null)
                    UnityEditor.AssetDatabase.CreateAsset(response, $"Assets/Responses/{actionResponse.actresid}_Response.asset");
                if(trigger != null && response != null)
                    UnityEditor.AssetDatabase.SaveAssets();
                #endif
            }
            else
            {
                Debug.LogWarning("Source or target object not found for action: " + actionResponse.actresid);
            }
        }
    }
    TriggerTemplate CreateTrigger(TriggerEvent triggerEvent)
    {
        Debug.Log("creating trigger");
        if (triggerEvent.IsCollision == "true")
        {
            GameObject sourceObject = GameObject.Find(triggerEvent.sourceObj);
            Rigidbody source_rb = sourceObject.GetComponent<Rigidbody>();
            if(source_rb == null)
                source_rb = sourceObject.AddComponent<Rigidbody>();

            // return null;
            // CollisionTrigger trigger = ScriptableObject.CreateInstance<CollisionTrigger>();
            // trigger.targetTag = triggerEvent.action; // Assuming action stores the tag
            // return trigger;
        }
        if(triggerEvent.action == "none")
            return null;

        if(triggerEvent.action == "change")
        {
            if (triggerEvent.change_property_by != null)
            {
                var changedProperties = triggerEvent.change_property_by as JObject;
                if (changedProperties != null && changedProperties.ContainsKey("Transform_initialrotation"))
                {
                    var rotationProperties = changedProperties["Transform_initialrotation"] as JObject;
                    if (rotationProperties != null)
                    {
                        AngleTrigger angleTrigger = ScriptableObject.CreateInstance<AngleTrigger>();

                        if (rotationProperties.TryGetValue("x", out JToken xRotationToken))
                        {
                            angleTrigger.fallThreshold_x = xRotationToken.ToObject<float>(); // Set fallThreshold based on x rotation
                        }
                        if (rotationProperties.TryGetValue("y", out JToken yRotationToken))
                        {
                            angleTrigger.fallThreshold_y = yRotationToken.ToObject<float>(); // Set fallThreshold based on y rotation
                        }
                        if (rotationProperties.TryGetValue("z", out JToken zRotationToken))
                        {
                            angleTrigger.fallThreshold_z = zRotationToken.ToObject<float>(); // Set fallThreshold based on z rotation
                        }
                        return angleTrigger;
                    }
                }
            }
        }

        if(triggerEvent.action == "input")
        {
            UserClickTrigger userClickTrigger = ScriptableObject.CreateInstance<UserClickTrigger>();
            return userClickTrigger;
        }

        // Add more conditions for other trigger types
        return null;
    }

    ResponseTemplate CreateResponse(ResponseEvent responseEvent)
    {
        Debug.Log("creating response");
        if (responseEvent.IsCollision == "true")
        {
            GameObject targetObject = GameObject.Find(responseEvent.targetObj);
            Rigidbody target_rb = targetObject.GetComponent<Rigidbody>();
            if(target_rb == null)
                target_rb = targetObject.AddComponent<Rigidbody>();

        }
        if (responseEvent.response == "disappear")
        {
            DisappearBehavior disappearBehavior = ScriptableObject.CreateInstance<DisappearBehavior>();
            return disappearBehavior;
        }
        
        if(responseEvent.response == "force")
        {
            MoveForwardBehavior moveForwardBehavior = ScriptableObject.CreateInstance<MoveForwardBehavior>();
            return moveForwardBehavior;
        }

        return null;
    }

    // private PrimitiveType GetPrimitiveTypeByString(string shape)
    // {
    //     switch (shape)
    //     {
    //         case "sphere":
    //             return PrimitiveType.Sphere;
    //         case "cube":
    //             return PrimitiveType.Cube;
    //         default:
    //             return PrimitiveType.Cube;
    //     }
    // }

    private void SaveSceneToPrefab(int version)
    {
        string prefabDirectory = $"Assets/ScenePrefabs/version_{version}";
        if (!Directory.Exists(prefabDirectory))
        {
            Directory.CreateDirectory(prefabDirectory);
        }

        GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (GameObject rootObject in rootObjects)
        {
            string prefabPath = Path.Combine(prefabDirectory, rootObject.name + ".prefab");
            PrefabUtility.SaveAsPrefabAsset(rootObject, prefabPath);
        }

        Debug.Log($"Scene saved as a prefab in version {version}.");
    }

    // private void OpenScene(string version)
    // {
    //     string scenePath = $"{scenesDirectory}/{version}.unity";

    //     if (System.IO.File.Exists(scenePath))
    //     {
    //         EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
    //     }
    //     else
    //     {
    //         EditorUtility.DisplayDialog("Error", "Scene file not found!", "OK");
    //     }
    // }

    private void CreateComparisonScene(int v1, int v2)
    {
        var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        EditorSceneManager.SetActiveScene(newScene);

        string[] prefabPaths1 = AssetDatabase.FindAssets("t:Prefab", new string[] { $"Assets/ScenePrefabs/version_{v1}" });
        string[] prefabPaths2 = AssetDatabase.FindAssets("t:Prefab", new string[] { $"Assets/ScenePrefabs/version_{v2}" });

        float offset = 0;

        foreach (string prefabPath in prefabPaths1)
        {
            string path = AssetDatabase.GUIDToAssetPath(prefabPath);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if (instance != null)
            {
                instance.transform.position += new Vector3(offset, 0, 0);
            }
        }

        offset += 24; // Separate the versions

        foreach (string prefabPath in prefabPaths2)
        {
            string path = AssetDatabase.GUIDToAssetPath(prefabPath);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if (instance != null)
            {
                instance.transform.position += new Vector3(offset, 0, 0);
            }
        }

        Debug.Log($"Comparison scene created for versions {v1} and {v2}.");
    }

    private void DisplayArticleForm(List<Article> articles, int ver_no)
    {
        GUILayout.Label("Assets Specifications - Version " + ver_no, EditorStyles.boldLabel);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        // Debug.Log(articles[0].);
        foreach (var article in articles)
        {
            // Debug.Log("yes");
            if(article._objectname == "Main Camera" || article._objectname == "Directional Light")
                continue;

            EditorGUILayout.LabelField("Object Name:", article._objectname, EditorStyles.boldLabel);
            article._objectname = EditorGUILayout.TextField("Object Name:", article._objectname);
            article._sid = EditorGUILayout.TextField("SID:", article._sid);
            article._slabel = EditorGUILayout.TextField("Label:", article._slabel);
            article._IsHidden = EditorGUILayout.IntField("Is Hidden:", article._IsHidden);
            article._enumcount = EditorGUILayout.IntField("Enum Count:", article._enumcount);
            article._Is3DObject = EditorGUILayout.IntField("Is 3D Object:", article._Is3DObject);
            article.HasChild = EditorGUILayout.IntField("Has Child:", article.HasChild);
            article.shape = EditorGUILayout.TextField("Shape:", article.shape);

            // Dimension
            // if (article.dimension != null)
            // {
            //     EditorGUILayout.LabelField("Dimension", EditorStyles.boldLabel);
            //     article.dimension.dradii = EditorGUILayout.FloatField("Radii:", article.dimension.dradii);
            //     article.dimension.dvolumn = EditorGUILayout.TextField("Volume:", article.dimension.dvolumn);
            //     article.dimension.dlength = EditorGUILayout.TextField("Length:", article.dimension.dlength);
            //     article.dimension.dbreadth = EditorGUILayout.TextField("Breadth:", article.dimension.dbreadth);
            //     article.dimension.dheigth = EditorGUILayout.TextField("Height:", article.dimension.dheigth);
            // }

            // Lighting
            if (article.lighting != null)
            {
                EditorGUILayout.LabelField("Lighting", EditorStyles.boldLabel);
                article.lighting.CastShadow = EditorGUILayout.TextField("Cast Shadow:", article.lighting.CastShadow);
                article.lighting.ReceiveShadow = EditorGUILayout.TextField("Receive Shadow:", article.lighting.ReceiveShadow);
                article.lighting.ContributeGlobalIlumination = EditorGUILayout.TextField("Contribute Global Illumination:", article.lighting.ContributeGlobalIlumination);
            }

            if(article.Transform_initialpos != null)
            {
                EditorGUILayout.LabelField("Position", EditorStyles.boldLabel);
                article.Transform_initialpos.x = EditorGUILayout.TextField("x position: ", article.Transform_initialpos.x);
                article.Transform_initialpos.y = EditorGUILayout.TextField("y position: ", article.Transform_initialpos.y);
                article.Transform_initialpos.z = EditorGUILayout.TextField("z position: ", article.Transform_initialpos.z);
            }
            if(article.Transform_objectscale != null)
            {
                EditorGUILayout.LabelField("Object Scale", EditorStyles.boldLabel);
                article.Transform_objectscale.x = EditorGUILayout.TextField("x scale: ", article.Transform_objectscale.x);
                article.Transform_objectscale.y = EditorGUILayout.TextField("y scale: ", article.Transform_objectscale.y);
                article.Transform_objectscale.z = EditorGUILayout.TextField("z scale: ", article.Transform_objectscale.z);
            }
            if(article.Transform_initialrotation != null)
            {
                EditorGUILayout.LabelField("Rotation", EditorStyles.boldLabel);
                article.Transform_initialrotation.x = EditorGUILayout.TextField("x rotation: ", article.Transform_initialrotation.x);
                article.Transform_initialrotation.y = EditorGUILayout.TextField("y rotation: ", article.Transform_initialrotation.y);
                article.Transform_initialrotation.z = EditorGUILayout.TextField("z rotation: ", article.Transform_initialrotation.z);
            }

            // Other fields...

            // Handle nested objects similarly
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            // ActionResponse a = new ActionResponse();
        }
    }

    private void DisplayActionForm(List<ActionResponse> actions, int version)
    {
        GUILayout.Label("Action-Response Specifications - Version " + version, EditorStyles.boldLabel);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        foreach (var actionResponse in actions)
        {
            EditorGUILayout.LabelField("ActResID: ", actionResponse.actresid, EditorStyles.boldLabel);

            actionResponse.comment = EditorGUILayout.TextField("Comment: ", actionResponse.comment);
            actionResponse.Syncronous = EditorGUILayout.TextField("Syncronous: ", actionResponse.Syncronous);

            if(actionResponse.trigger_event != null)
            {
                EditorGUILayout.LabelField("Trigger Event", EditorStyles.boldLabel);
                actionResponse.trigger_event.sourceObj = EditorGUILayout.TextField("Source Object: ", actionResponse.trigger_event.sourceObj);
                actionResponse.trigger_event.IsCollision = EditorGUILayout.TextField("Is Collision: ", actionResponse.trigger_event.IsCollision);
                actionResponse.trigger_event.action = EditorGUILayout.TextField("Action: ", actionResponse.trigger_event.action);
                actionResponse.trigger_event.inputType = EditorGUILayout.TextField("Input Type: ", actionResponse.trigger_event.inputType);
                
                var changedProperties = actionResponse.trigger_event.change_property_by as JObject;
                if(changedProperties != null)
                {
                    if(changedProperties.ContainsKey("Transform_initialrotation"))
                    {
                        EditorGUILayout.LabelField("Change in Angle", EditorStyles.boldLabel);
                        // var rotationProperties = changedProperties as JObject;
                        
                        if (changedProperties["Transform_initialrotation"] != null)
                        {
                            var angles = changedProperties["Transform_initialrotation"] as JObject;
                            // Debug.Log("here: line 593: " + angles["x"]);

                            string xValue = angles["x"]?.ToString();
                            string yValue = angles["y"]?.ToString();
                            string zValue = angles["z"]?.ToString();

                            xValue = EditorGUILayout.TextField("x: ", xValue);
                            yValue = EditorGUILayout.TextField("y: ", yValue);
                            zValue = EditorGUILayout.TextField("z: ", zValue);

                            angles["x"] = xValue;
                            angles["y"] = yValue;
                            angles["z"] = zValue;

                            changedProperties["Transform_initialrotation"] = angles;
                        }
                    }
                    actionResponse.trigger_event.change_property_by = changedProperties;
                }
            }
            
            if(actionResponse.response_event != null)
            {
                EditorGUILayout.LabelField("Response Event", EditorStyles.boldLabel);
                actionResponse.response_event.targetObj = EditorGUILayout.TextField("Target Object: ", actionResponse.response_event.targetObj);
                actionResponse.response_event.IsCollision = EditorGUILayout.TextField("Is Collision: ", actionResponse.response_event.IsCollision);
                actionResponse.response_event.response = EditorGUILayout.TextField("Response: ", actionResponse.response_event.response);
                actionResponse.response_event.outputType = EditorGUILayout.TextField("Output Type: ", actionResponse.response_event.outputType);
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }
    }
    
    private PrimitiveType GetPrimitiveTypeByString(string shape)
    {
        switch (shape)
        {
            case "sphere":
                return PrimitiveType.Sphere;
            case "cube":
                return PrimitiveType.Cube;
            default:
                return PrimitiveType.Cube;
        }
    }

    private Dictionary<string, object> GetTransformInitialPosition(GameObject obj)
    {
        Dictionary<string, object> transformInitialPosition = new Dictionary<string, object>();

        if (obj.transform != null)
        {
            transformInitialPosition["x"] = obj.transform.position.x;
            transformInitialPosition["y"] = obj.transform.position.y;
            transformInitialPosition["z"] = obj.transform.position.z;
        }
        else
        {
            transformInitialPosition["x"] = 0;
            transformInitialPosition["y"] = 0;
            transformInitialPosition["z"] = 0;
        }

        return transformInitialPosition;
    }

    private Dictionary<string, object> GetTransformObjectScale(GameObject obj)
    {
        Dictionary<string, object> transformObjectScale = new Dictionary<string, object>();

        if (obj.transform != null)
        {
            transformObjectScale["x"] = obj.transform.localScale.x;
            transformObjectScale["y"] = obj.transform.localScale.y;
            transformObjectScale["z"] = obj.transform.localScale.z;
        }
        else
        {
            transformObjectScale["x"] = 1;
            transformObjectScale["y"] = 1;
            transformObjectScale["z"] = 1;
        }

        return transformObjectScale;
    }

    private Dictionary<string, object> GetXRRigidObject(GameObject obj)
    {
        Dictionary<string, object> xrrigidObject = new Dictionary<string, object>();

        if (obj.GetComponent<Rigidbody>() != null)
        {
            Rigidbody rigidObject = obj.GetComponent<Rigidbody>();
            xrrigidObject["value"] = 1;
            xrrigidObject["mass"] = rigidObject.mass;
            xrrigidObject["dragfriction"] = rigidObject.drag;
            xrrigidObject["angulardrag"] = rigidObject.drag;
            xrrigidObject["Isgravityenable"] = rigidObject.useGravity;
            xrrigidObject["IsKinematic"] = rigidObject.isKinematic;
            if(rigidObject.interpolation == RigidbodyInterpolation.Interpolate)
                xrrigidObject["CanInterpolate"] = 1;
            else if (rigidObject.interpolation == RigidbodyInterpolation.Extrapolate)
                xrrigidObject["CanInterpolate"] = 2;
            else
                xrrigidObject["CanInterpolate"] = 0;
            // xrrigidObject["CollisionPolling"] = rigidObject.collisionPolling;
            switch (rigidObject.collisionDetectionMode)
            {
                case CollisionDetectionMode.Discrete:
                    xrrigidObject["CollisionPolling"] = "discrete";
                    break;
                case CollisionDetectionMode.Continuous:
                    xrrigidObject["CollisionPolling"] = "continuous";
                    break;
                case CollisionDetectionMode.ContinuousDynamic:
                    xrrigidObject["CollisionPolling"] = "continuous-dynamic";
                    break;
                case CollisionDetectionMode.ContinuousSpeculative:
                    xrrigidObject["CollisionPolling"] = "continuous-speculative";
                    break;
                default:
                    xrrigidObject["CollisionPolling"] = "none";
                    break;
            }
        }
        else
        {
            xrrigidObject["value"] = 0;
            xrrigidObject["mass"] = 0;
            xrrigidObject["dragfriction"] = 0;
            xrrigidObject["angulardrag"] = 0;
            xrrigidObject["Isgravityenable"] = false;
            xrrigidObject["IsKinematic"] = false;
            xrrigidObject["CanInterpolate"] = 0;
            xrrigidObject["CollisionPolling"] = "none";
        }

        return xrrigidObject;
    }

    public void SaveVersion(int version_no)
    {
        // File.WriteAllText(Application.dataPath + "/version_trial_" + index + ".json", {
        //     hi: "hi"
        // });
        List<SerializedObject> objects = new List<SerializedObject>();

        // Get all objects in the scene
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        Dictionary<string, List<Dictionary<string, object>>> new_format_data = new Dictionary<string, List<Dictionary<string, object>>>();
        new_format_data["articles"] = new List<Dictionary<string, object>>();
        Dictionary<string, Dictionary<string, object>> data = new Dictionary<string, Dictionary<string, object>>();
        int index = 0;
        foreach (GameObject obj in allObjects)
        {
            // Component[] components = obj.GetComponents<MonoBehaviour>();
            // foreach (MonoBehaviour script in components)
            // {
            //     Debug.Log("scripts");
            //     Debug.Log(script.GetType().Name);
            //     if (script is ClickExecutor)
            //     {
            //         ClickExecutor executor = (ClickExecutor)script;
            //         Debug.Log(executor.behavior);
            //     }
            // }

            SerializedObject so = null;
            if(obj != null)
            {
                // Get the SerializedObject for the object
                Dictionary<string, object> objData = new Dictionary<string, object>();
                objData["Transform_initialpos"] = GetTransformInitialPosition(obj);
                objData["XRRigidObject"] = GetXRRigidObject(obj);
                objData["Transform_objectscale"] = GetTransformObjectScale(obj);
                so = new SerializedObject(obj);
                so.Update();

                SerializedProperty iterator = so.GetIterator();
                while (iterator.NextVisible(true))
                {
                    // If the property is a container (array or object), recursively add its contents
                    // Debug.Log(iterator.propertyType);
                    if (iterator.propertyType == SerializedPropertyType.ObjectReference || iterator.propertyType == SerializedPropertyType.ArraySize)
                    {
                        if (iterator.isArray)
                        {
                            // Debug.Log("Array mei hoon");
                            SerializedProperty element = iterator.Copy();
                            element.Next(true);
                            int count = iterator.arraySize;
                            List<object> elementsData = new List<object>();
                            for (int i = 0; i < count; i++)
                            {
                                SerializedObject elementObj = new SerializedObject(element.objectReferenceValue);
                                elementObj.Update();
                                elementsData.Add(SaveObject(elementObj));
                                element.Next(false);
                            }
                            objData[iterator.name] = elementsData;
                        }
                        else
                        {
                            // Debug.Log(iterator.propertyType);
                            // Debug.Log(iterator.objectReferenceValue);
                            if (iterator.objectReferenceValue != null)
                            {
                                // Debug.Log("Object Reference mei hoon");
                                SerializedObject elementObj = new SerializedObject(iterator.objectReferenceValue);
                                elementObj.Update();
                                objData[iterator.name] = SaveObject(elementObj);
                            }
                            // else
                            // {
                            //     Debug.Log("Object Reference ka value nahi mila");
                            // }
                        }
                    }
                    else
                    {
                        // Otherwise, just add the property value
                        // Debug.Log("Otherwise");
                        objData[iterator.name] = SaveProperty(iterator);
                    }
                }
                objData["_objectname"] = objData["m_Name"];
                objects.Add(so);
                data[index.ToString()] = objData;
                new_format_data["articles"].Add(objData);
                index++;
                so.ApplyModifiedProperties();
            }
            
        }
        // Convert the dictionary to a JSON string
        Debug.Log("Finally we have this structure");
        foreach (KeyValuePair<string, System.Collections.Generic.Dictionary<string, object>> pair in data)
        {
            Debug.Log("Key: " + pair.Key + ", Value: " + pair.Value);
            foreach (KeyValuePair<string, object> pair_inside in pair.Value)
            {
                Debug.Log("Key: " + pair_inside.Key + ", Value: " + pair_inside.Value);
            }
        }
        // string json = JsonUtility.ToJson(data, true);
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        string json_new_format = JsonConvert.SerializeObject(new_format_data, Formatting.Indented);
        Debug.Log(json);
        // Save the JSON string to a file
        string directory_path = "Assets/specifications/version_" + version_no;
        if(!Directory.Exists(directory_path))
            Directory.CreateDirectory(directory_path);

        File.WriteAllText(directory_path + "/article_old.json", json);
        File.WriteAllText(directory_path + "/article.json", json_new_format);
        Debug.Log("version number");
        Debug.Log(version_no);
        // version_index++;
    }

    private object SaveProperty(SerializedProperty property)
    {
        switch (property.propertyType)
        {
            case SerializedPropertyType.Integer:
                return property.intValue;
            case SerializedPropertyType.Boolean:
                return property.boolValue;
            case SerializedPropertyType.Float:
                return property.floatValue;
            case SerializedPropertyType.String:
                return property.stringValue;
            case SerializedPropertyType.Color:
                return property.colorValue;
            case SerializedPropertyType.Vector2:
                return property.vector2Value;
            case SerializedPropertyType.Vector3:
                return property.vector3Value;
            case SerializedPropertyType.Vector4:
                return property.vector4Value;
            case SerializedPropertyType.Rect:
                return property.rectValue;
            case SerializedPropertyType.ArraySize:
                return property.intValue;
            case SerializedPropertyType.Character:
                return property.stringValue[0];
            case SerializedPropertyType.ObjectReference:
                return property.objectReferenceValue ? property.objectReferenceValue.name : null;
            default:
                return null;
        }
    }

    private object SaveObject(SerializedObject obj)
    {
        Dictionary<string, object> objData = new Dictionary<string, object>();
        obj.Update();

        // Iterate through all the properties of the object
        SerializedProperty iterator = obj.GetIterator();
        while (iterator.NextVisible(true))
        {
            // If the property is a container (array or object), recursively add its contents
            if (iterator.propertyType == SerializedPropertyType.ObjectReference || iterator.propertyType == SerializedPropertyType.ArraySize)
            {
                if (iterator.isArray)
                {
                    SerializedProperty element = iterator.Copy();
                    element.Next(true);
                    int count = iterator.arraySize;
                    List<object> elementsData = new List<object>();
                    for (int i = 0; i < count; i++)
                    {
                        SerializedObject elementObj = new SerializedObject(element.objectReferenceValue);
                        elementObj.Update();
                        elementsData.Add(SaveObject(elementObj));
                        element.Next(false);
                    }
                    objData[iterator.name] = elementsData;
                }
                else
                {
                    SerializedObject elementObj = new SerializedObject(iterator.objectReferenceValue);
                    elementObj.Update();
                    objData[iterator.name] = SaveObject(elementObj);
                }
            }
            else
            {
                // Otherwise, just add the property value
                objData[iterator.name] = SaveProperty(iterator);
            }
        }

        // Add the object's components to the dictionary
        GameObject gameObject = obj.targetObject as GameObject;
        if (gameObject != null)
        {
            Component[] components = gameObject.GetComponents<Component>();
            foreach (Component component in components)
            {
                SerializedObject componentObj = new SerializedObject(component);
                componentObj.Update();
                objData[component.GetType().Name] = SaveObject(componentObj);
            }
        }

        obj.ApplyModifiedProperties();
        return objData;
    }
 
}