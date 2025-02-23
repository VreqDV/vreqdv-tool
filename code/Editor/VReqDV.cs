using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
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
    private Vector2 scrollPositionObjectCompare;

    // private static int total_versions = screenState.total_versions;
    private int selected_display_component = 0;
    private string[] version_list;
    private static string[] versionSpecs;
    private int compare_version = 0;
    private bool editingEnabled = false;

    private void Initialize()
    {
        versionSpecs = Directory.GetDirectories("Assets/specifications");
        screenState.total_versions = versionSpecs.Length;
        version_list = new string[screenState.total_versions + 1];

        for (int i = 1; i <= screenState.total_versions; i++)
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
        if (screenState.total_versions == 0)
        {
            GUILayout.Label("To start using VReqDV to track your project versions, upload the project specifications in a new version, or save the contents of the current scene to a new version.");
            if (GUILayout.Button("Save Version"))
            {
                screenState.total_versions++;
                screenState.curr_version = screenState.total_versions;
                SaveVersion(screenState.curr_version);
                SaveSceneToPrefab(screenState.curr_version);
                window.Repaint();
            }
        }

        else
        {
            // Show the current version specifications
            GUILayout.BeginHorizontal();
            GUILayout.Label("Current Version: " + screenState.curr_version, setFont(14));
            GUILayout.Label("Total Versions: " + screenState.total_versions, setFont(14), GUILayout.Width(400));

            if(GUILayout.Button("Save New Version", GUILayout.Width(200)))
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
            GUILayout.Label("Editing Enabled: " + editingEnabled, setFont(14));
            GUILayout.Label("NOTE: If editing is enabled, compare versions will not work!", setFont(14), GUILayout.Width(750));
            if(GUILayout.Button("Enable/Disable Form Editing", GUILayout.Width(200)))
            {
                editingEnabled = !editingEnabled;
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Change Current Version:", setFont(12));
            screenState.curr_version = EditorGUILayout.Popup(screenState.curr_version, version_list, GUILayout.Width(100));
            if(GUILayout.Button("Display Mock-up", GUILayout.Width(200)))
            {
                ClearObjects();
                OpenScene(screenState.curr_version);
                string dir_path = $"Assets/ScenePrefabs/version_{screenState.curr_version}";
                if(!Directory.Exists(dir_path))
                    SaveSceneToPrefab(screenState.curr_version);
            }

            GUILayout.Label("Compare with Version:", setFont(12));
            
            compare_version = EditorGUILayout.Popup(compare_version, version_list, GUILayout.Width(100));

            EditorGUI.BeginDisabledGroup(editingEnabled);
            if(GUILayout.Button("Display Comparison", GUILayout.Width(200)))
            {
                CreateComparisonScene(screenState.curr_version, compare_version);
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();

            try
            {
                string file = "Assets/specifications/version_" + screenState.curr_version + "/article.json";
                string objectData = File.ReadAllText(file);
                objectSpecifications = JsonConvert.DeserializeObject<ArticleList>(objectData);
            }
            catch (FileNotFoundException)
            {
                objectSpecifications = new ArticleList { articles = new List<Article> { new Article { _objectname = "Error", _slabel = "File not found" } } };
            }
            catch (JsonException)
            {
                objectSpecifications = new ArticleList { articles = new List<Article> { new Article { _objectname = "Error", _slabel = "Failed to parse JSON" } } };
            }

            try
            {
                string file = "Assets/specifications/version_" + screenState.curr_version + "/action-response.json";
                string actionData = File.ReadAllText(file);
                actionSpecifications = JsonConvert.DeserializeObject<ActionResponseList>(actionData);
            }
            catch (FileNotFoundException)
            {
                actionSpecifications = new ActionResponseList { ObjAction = new List<ActionResponse> { new ActionResponse { actresid = "Error - File not found" } } };
            }
            catch (JsonException)
            {
                actionSpecifications = new ActionResponseList { ObjAction = new List<ActionResponse> { new ActionResponse { actresid = "Error - Failed to parse JSON" } } };
            }

            if (compare_version != 0)
            {
                try
                {
                    string file = "Assets/specifications/version_" + compare_version + "/article.json";
                    string objectData = File.ReadAllText(file);
                    compareObjectSpecifications = JsonConvert.DeserializeObject<ArticleList>(objectData);
                }
                catch (FileNotFoundException)
                {
                    compareObjectSpecifications = new ArticleList { articles = new List<Article> { new Article { _objectname = "Error", _slabel = "File not found" } } };
                }
                catch (JsonException)
                {
                    compareObjectSpecifications = new ArticleList { articles = new List<Article> { new Article { _objectname = "Error", _slabel = "Failed to parse JSON" } } };
                }

                try
                {
                    string file = "Assets/specifications/version_" + compare_version + "/action-response.json";
                    string actionData = File.ReadAllText(file);
                    compareActionSpecifications = JsonConvert.DeserializeObject<ActionResponseList>(actionData);
                }
                catch (FileNotFoundException)
                {
                    compareActionSpecifications = new ActionResponseList { ObjAction = new List<ActionResponse> { new ActionResponse { actresid = "Error - File not found" } } };
                }
                catch (JsonException)
                {
                    compareActionSpecifications = new ActionResponseList { ObjAction = new List<ActionResponse> { new ActionResponse { actresid = "Error - Failed to parse JSON" } } };
                }
            }

            string[] list = new string[] { "Assets", "Actions" };
            // int selected_display_component = 0;
            selected_display_component = EditorGUILayout.Popup("Select Component", selected_display_component, list);

            GUILayout.BeginHorizontal();

            // Scrollable area for object data
            scrollPositionObject = EditorGUILayout.BeginScrollView(scrollPositionObject, GUILayout.Height(position.height - 110), GUILayout.Width(position.width / 2));

            if (objectSpecifications != null && objectSpecifications.articles != null)
            {
                if (list[selected_display_component] == "Assets")
                {
                    DisplayArticleForm(objectSpecifications.articles, screenState.curr_version);
                }
            }
            if (actionSpecifications != null && actionSpecifications.ObjAction != null)
            {
                if (list[selected_display_component] == "Actions")
                {
                    DisplayActionForm(actionSpecifications.ObjAction, screenState.curr_version);
                }
            }
            EditorGUILayout.EndScrollView();

            // compare with
            if (compare_version != 0)
            {
                scrollPositionObjectCompare = EditorGUILayout.BeginScrollView(scrollPositionObjectCompare, GUILayout.Height(position.height - 110), GUILayout.Width(position.width / 2));

                if (compareObjectSpecifications != null && compareObjectSpecifications.articles != null)
                {
                    if (list[selected_display_component] == "Assets")
                        DisplayArticleForm(compareObjectSpecifications.articles, compare_version);
                }
                if (compareActionSpecifications != null && compareActionSpecifications.ObjAction != null)
                {
                    if (list[selected_display_component] == "Actions")
                        DisplayActionForm(compareActionSpecifications.ObjAction, compare_version);
                }
                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndHorizontal();
            
            if (editingEnabled && GUI.changed)
            {
                string json1 = JsonConvert.SerializeObject(objectSpecifications, Formatting.Indented);
                string json2 = JsonConvert.SerializeObject(actionSpecifications, Formatting.Indented);
                string filePath1 = $"Assets/specifications/version_{screenState.curr_version}/article.json";
                string filePath2 = $"Assets/specifications/version_{screenState.curr_version}/action-response.json";
                File.WriteAllText(filePath1, json1);
                File.WriteAllText(filePath2, json2);
                AssetDatabase.Refresh();
                ClearObjects();
                OpenScene(screenState.curr_version);
                SaveSceneToPrefab(screenState.curr_version);
            }
        }
    }

    private void OpenScene(int version_no)
    {
        string folder_path = "Assets/specifications/version_" + version_no;
        if (Directory.Exists(folder_path))
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
            GameObject go = null;
            if(objectData.context_img_source != null)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(objectData.context_img_source);
                if(prefab != null)
                {
                    go = Instantiate(prefab);
                    go.name = objectData._objectname;
                    // go = Instantiate(go);
                    if(go.GetComponent<BoxCollider>() == null)
                        go.AddComponent<BoxCollider>();
                }
                else
                {
                    Debug.LogWarning("Asset not found at: " + objectData.context_img_source);
                    go = GameObject.CreatePrimitive(GetPrimitiveTypeByString(objectData.shape));
                    go.name = objectData._objectname;
                }
                Debug.Log("Creation Done");
            }
            else
            {
                go = GameObject.CreatePrimitive(GetPrimitiveTypeByString(objectData.shape));
                go.name = objectData._objectname;
            }

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

            if(objectData.XRRigidObject.value == "1" && go.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = go.AddComponent<Rigidbody>();
                rb.mass = float.Parse(objectData.XRRigidObject.mass);
                rb.drag = float.Parse(objectData.XRRigidObject.dragfriction);
                rb.angularDrag = float.Parse(objectData.XRRigidObject.angulardrag);
                rb.useGravity = bool.Parse(objectData.XRRigidObject.Isgravityenable);
                rb.isKinematic = bool.Parse(objectData.XRRigidObject.IsKinematic);

                switch (objectData.XRRigidObject.CollisionPolling)
                {
                    case "discrete":
                        rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
                        break;
                    case "continuous":
                        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                        break;
                    case "continuous-dynamic":
                        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                        break;
                    case "continuous-speculative":
                        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                        break;
                    default:
                        rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
                        break;
                }

                switch (int.Parse(objectData.XRRigidObject.CanInterpolate))
                {
                    case 1:
                        rb.interpolation = RigidbodyInterpolation.Interpolate;
                        break;
                    case 2:
                        rb.interpolation = RigidbodyInterpolation.Extrapolate;
                        break;
                    default:
                        rb.interpolation = RigidbodyInterpolation.None;
                        break;
                }
            }
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
            // Debug.Log(actionResponse.actresid);
            GameObject sourceObject = GameObject.Find(actionResponse.trigger_event.sourceObj);
            GameObject targetObject = GameObject.Find(actionResponse.response_event.targetObj);

            if (sourceObject != null && targetObject != null)
            {
                // Rigidbody rb = targetObject.GetComponent<Rigidbody>();
                ActionComponent actionComponent = sourceObject.AddComponent<ActionComponent>();
                // Debug.Log("No problem so far");
                TriggerTemplate trigger = CreateTrigger(actionResponse.trigger_event);
                ResponseTemplate response = CreateResponse(actionResponse.response_event);
                // Debug.Log("done creations");
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
            CollisionTrigger collisionTrigger = ScriptableObject.CreateInstance<CollisionTrigger>();
            return collisionTrigger;

            // GameObject sourceObject = GameObject.Find(triggerEvent.sourceObj);
            // Rigidbody source_rb = sourceObject.GetComponent<Rigidbody>();
            // if (source_rb == null)
            //     source_rb = sourceObject.AddComponent<Rigidbody>();
        }
        if (triggerEvent.action == "none")
            return null;

        if (triggerEvent.action == "change")
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
                            angleTrigger.fallThreshold_x = xRotationToken.ToObject<float>();
                        }
                        if (rotationProperties.TryGetValue("y", out JToken yRotationToken))
                        {
                            angleTrigger.fallThreshold_y = yRotationToken.ToObject<float>();
                        }
                        if (rotationProperties.TryGetValue("z", out JToken zRotationToken))
                        {
                            angleTrigger.fallThreshold_z = zRotationToken.ToObject<float>();
                        }
                        return angleTrigger;
                    }
                }
            }
        }

        if (triggerEvent.action == "input")
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
            CollisionBehavior collisionBehavior = ScriptableObject.CreateInstance<CollisionBehavior>();
            return collisionBehavior;

            // GameObject targetObject = GameObject.Find(responseEvent.targetObj);
            // Rigidbody target_rb = targetObject.GetComponent<Rigidbody>();
            // if (target_rb == null)
            //     target_rb = targetObject.AddComponent<Rigidbody>();

        }
        if (responseEvent.response == "disappear")
        {
            DisappearBehavior disappearBehavior = ScriptableObject.CreateInstance<DisappearBehavior>();
            return disappearBehavior;
        }

        if (responseEvent.response == "force")
        {
            if (responseEvent.force != null)
            {
                JObject forceProperties = JObject.FromObject(responseEvent.force);
                if (forceProperties != null)
                {
                    MoveForwardBehavior moveForwardBehavior = ScriptableObject.CreateInstance<MoveForwardBehavior>();

                    if (forceProperties.TryGetValue("force_x", out JToken xForceToken))
                    {
                        moveForwardBehavior.force_x = xForceToken.ToObject<float>();
                    }
                    if (forceProperties.TryGetValue("force_y", out JToken yForceToken))
                    {
                        moveForwardBehavior.force_y = yForceToken.ToObject<float>();
                    }
                    if (forceProperties.TryGetValue("force_z", out JToken zForceToken))
                    {
                        moveForwardBehavior.force_z = zForceToken.ToObject<float>();
                    }
                    if (forceProperties.TryGetValue("type", out JToken forceTypeToken))
                    {
                        moveForwardBehavior.type = forceTypeToken.ToObject<string>();
                    }
                    return moveForwardBehavior;
                }
            }
        }

        return null;
    }

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

    private void ClearObjects()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            UnityEngine.Object.DestroyImmediate(obj);
        }

        GameObject mainCamera = new GameObject("Main Camera");
        mainCamera.AddComponent<Camera>();
        mainCamera.tag = "MainCamera";
        mainCamera.transform.position = new Vector3(0, 1, -10);

        GameObject directionalLight = new GameObject("Directional Light");
        Light lightComp = directionalLight.AddComponent<Light>();
        lightComp.type = LightType.Directional;
        directionalLight.transform.position = new Vector3(0, 3, 0);
        directionalLight.transform.rotation = Quaternion.Euler(50, -30, 0);
    }

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
            if (article._objectname == "Main Camera" || article._objectname == "Directional Light")
                continue;

            EditorGUILayout.LabelField("Object Name:", article._objectname, EditorStyles.boldLabel);
            article._objectname = EditorGUILayout.TextField("Object Name:", article._objectname);
            article._sid = EditorGUILayout.TextField("SID:", article._sid);
            article._slabel = EditorGUILayout.TextField("Label:", article._slabel);
            article._IsHidden = EditorGUILayout.IntField("Is Hidden:", article._IsHidden);
            article._enumcount = EditorGUILayout.IntField("Enum Count:", article._enumcount);
            article._Is3DObject = EditorGUILayout.IntField("Is 3D Object:", article._Is3DObject);
            article.HasChild = EditorGUILayout.IntField("Has Child:", article.HasChild);
            if(article.context_img_source != null)
                article.context_img_source = EditorGUILayout.TextField("Asset Path:", article.context_img_source);
            else
                article.shape = EditorGUILayout.TextField("Shape:", article.shape);

            // Lighting
            if (article.lighting != null)
            {
                EditorGUILayout.LabelField("Lighting", EditorStyles.boldLabel);
                article.lighting.CastShadow = EditorGUILayout.TextField("Cast Shadow:", article.lighting.CastShadow);
                article.lighting.ReceiveShadow = EditorGUILayout.TextField("Receive Shadow:", article.lighting.ReceiveShadow);
                article.lighting.ContributeGlobalIlumination = EditorGUILayout.TextField("Contribute Global Illumination:", article.lighting.ContributeGlobalIlumination);
            }

            if (article.Transform_initialpos != null)
            {
                EditorGUILayout.LabelField("Position", EditorStyles.boldLabel);
                article.Transform_initialpos.x = EditorGUILayout.TextField("x position: ", article.Transform_initialpos.x);
                article.Transform_initialpos.y = EditorGUILayout.TextField("y position: ", article.Transform_initialpos.y);
                article.Transform_initialpos.z = EditorGUILayout.TextField("z position: ", article.Transform_initialpos.z);
            }
            if (article.Transform_objectscale != null)
            {
                EditorGUILayout.LabelField("Object Scale", EditorStyles.boldLabel);
                article.Transform_objectscale.x = EditorGUILayout.TextField("x scale: ", article.Transform_objectscale.x);
                article.Transform_objectscale.y = EditorGUILayout.TextField("y scale: ", article.Transform_objectscale.y);
                article.Transform_objectscale.z = EditorGUILayout.TextField("z scale: ", article.Transform_objectscale.z);
            }
            if (article.Transform_initialrotation != null)
            {
                EditorGUILayout.LabelField("Rotation", EditorStyles.boldLabel);
                article.Transform_initialrotation.x = EditorGUILayout.TextField("x rotation: ", article.Transform_initialrotation.x);
                article.Transform_initialrotation.y = EditorGUILayout.TextField("y rotation: ", article.Transform_initialrotation.y);
                article.Transform_initialrotation.z = EditorGUILayout.TextField("z rotation: ", article.Transform_initialrotation.z);
            }
            if (article.XRRigidObject != null)
            {
                EditorGUILayout.LabelField("XR Rigid Object", EditorStyles.boldLabel);
                article.XRRigidObject.value = EditorGUILayout.TextField("Is XR Rigid Object: ", article.XRRigidObject.value);
                article.XRRigidObject.mass = EditorGUILayout.TextField("Mass ", article.XRRigidObject.mass);
                article.XRRigidObject.dragfriction = EditorGUILayout.TextField("Drag ", article.XRRigidObject.dragfriction);
                article.XRRigidObject.angulardrag = EditorGUILayout.TextField("Angular Drag ", article.XRRigidObject.angulardrag);
                article.XRRigidObject.Isgravityenable = EditorGUILayout.TextField("Use Gravity ", article.XRRigidObject.Isgravityenable);
                article.XRRigidObject.IsKinematic = EditorGUILayout.TextField("Is Kinematic ", article.XRRigidObject.IsKinematic);
                article.XRRigidObject.CanInterpolate = EditorGUILayout.TextField("Interpolate ", article.XRRigidObject.CanInterpolate);
                article.XRRigidObject.CollisionPolling = EditorGUILayout.TextField("Collision Detection ", article.XRRigidObject.CollisionPolling);
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

            if (actionResponse.trigger_event != null)
            {
                EditorGUILayout.LabelField("Trigger Event", EditorStyles.boldLabel);
                actionResponse.trigger_event.sourceObj = EditorGUILayout.TextField("Source Object: ", actionResponse.trigger_event.sourceObj);
                actionResponse.trigger_event.IsCollision = EditorGUILayout.TextField("Is Collision: ", actionResponse.trigger_event.IsCollision);
                actionResponse.trigger_event.action = EditorGUILayout.TextField("Action: ", actionResponse.trigger_event.action);
                actionResponse.trigger_event.inputType = EditorGUILayout.TextField("Input Type: ", actionResponse.trigger_event.inputType);

                var changedProperties = actionResponse.trigger_event.change_property_by as JObject;
                if (changedProperties != null)
                {
                    if (changedProperties.ContainsKey("Transform_initialrotation"))
                    {
                        EditorGUILayout.LabelField("Change in Angle", EditorStyles.boldLabel);
                        if (changedProperties["Transform_initialrotation"] != null)
                        {
                            var angles = changedProperties["Transform_initialrotation"] as JObject;

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

            if (actionResponse.response_event != null)
            {
                EditorGUILayout.LabelField("Response Event", EditorStyles.boldLabel);
                actionResponse.response_event.targetObj = EditorGUILayout.TextField("Target Object: ", actionResponse.response_event.targetObj);
                actionResponse.response_event.IsCollision = EditorGUILayout.TextField("Is Collision: ", actionResponse.response_event.IsCollision);
                actionResponse.response_event.response = EditorGUILayout.TextField("Response: ", actionResponse.response_event.response);
                actionResponse.response_event.outputType = EditorGUILayout.TextField("Output Type: ", actionResponse.response_event.outputType);
                if(actionResponse.response_event.response == "force")
                {
                    var forceParameters = actionResponse.response_event.force as JObject;
                    EditorGUILayout.LabelField("Force Parameters", EditorStyles.boldLabel);
                    if(forceParameters != null)
                    {
                        if(forceParameters.ContainsKey("force_x"))
                        {
                            string xforce = forceParameters["force_x"]?.ToString();
                            xforce = EditorGUILayout.TextField("Force in x: ", xforce);
                            forceParameters["force_x"] = xforce;
                        }
                        else
                        {
                            string xforce = "0";
                            xforce = EditorGUILayout.TextField("Force in x: ", xforce);
                        }
                        if(forceParameters.ContainsKey("force_y"))
                        {
                            string yforce = forceParameters["force_y"]?.ToString();
                            yforce = EditorGUILayout.TextField("Force in y: ", yforce);
                            forceParameters["force_y"] = yforce;
                        }
                        else
                        {
                            string yforce = "0";
                            yforce = EditorGUILayout.TextField("Force in y: ", yforce);
                        }
                        if(forceParameters.ContainsKey("force_z"))
                        {
                            string zforce = forceParameters["force_z"]?.ToString();
                            zforce = EditorGUILayout.TextField("Force in z: ", zforce);
                            forceParameters["force_z"] = zforce;
                        }
                        else
                        {
                            string zforce = "0";
                            zforce = EditorGUILayout.TextField("Force in z: ", zforce);
                        }
                        if(forceParameters.ContainsKey("type") && forceParameters["type"] != null)
                        {
                            string forceType = forceParameters["type"]?.ToString();
                            forceType = EditorGUILayout.TextField("Type: ", forceType);
                        }
                        else
                        {
                            string forceType = "impulse";
                            forceType = EditorGUILayout.TextField("Type: ", forceType);
                        }
                    }
                    actionResponse.response_event.force = forceParameters;
                }
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }
    }

    private PrimitiveType GetPrimitiveTypeByString(string shape)
    {
        if (Enum.TryParse(shape, true, out PrimitiveType primitiveType))
        {
            return primitiveType;
        }

        return PrimitiveType.Cube;
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

    private Dictionary<string, object> GetTransformInitialRotation(GameObject obj)
    {
        Dictionary<string, object> transformInitialRotation = new Dictionary<string, object>();

        if (obj.transform != null)
        {
            transformInitialRotation["x"] = obj.transform.rotation.eulerAngles.x;
            transformInitialRotation["y"] = obj.transform.rotation.eulerAngles.y;
            transformInitialRotation["z"] = obj.transform.rotation.eulerAngles.z;
        }
        else
        {
            transformInitialRotation["x"] = 0;
            transformInitialRotation["y"] = 0;
            transformInitialRotation["z"] = 0;
        }

        return transformInitialRotation;
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
            if (rigidObject.interpolation == RigidbodyInterpolation.Interpolate)
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
        List<SerializedObject> objects = new List<SerializedObject>();

        // Get all objects in the scene
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        // current formats
        Dictionary<string, List<Dictionary<string, object>>> new_format_data = new Dictionary<string, List<Dictionary<string, object>>>();
        new_format_data["articles"] = new List<Dictionary<string, object>>();
        Dictionary<string, List<Dictionary<string, object>>> actres_data = new Dictionary<string, List<Dictionary<string, object>>>();
        actres_data["ObjAction"] = new List<Dictionary<string, object>>();
        // old format (legacy purpose??)
        Dictionary<string, Dictionary<string, object>> data = new Dictionary<string, Dictionary<string, object>>();

        int index = 0;
        foreach (GameObject obj in allObjects)
        {
            if(obj.name == "Main Camera" || obj.name == "Directional Light")
                continue;

            SerializedObject so = null;
            if (obj != null)
            {
                // Get the SerializedObject for the object
                Dictionary<string, object> objData = new Dictionary<string, object>();
                objData["Transform_initialpos"] = GetTransformInitialPosition(obj);
                objData["XRRigidObject"] = GetXRRigidObject(obj);
                objData["Transform_objectscale"] = GetTransformObjectScale(obj);
                objData["Transform_initialrotation"] = GetTransformInitialRotation(obj);
                so = new SerializedObject(obj);
                so.Update();

                SerializedProperty iterator = so.GetIterator();
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
                            if (iterator.objectReferenceValue != null)
                            {
                                SerializedObject elementObj = new SerializedObject(iterator.objectReferenceValue);
                                elementObj.Update();
                                objData[iterator.name] = SaveObject(elementObj);
                            }
                        }
                    }
                    else
                    {
                        // Otherwise, just add the property value
                        objData[iterator.name] = SaveProperty(iterator);
                    }
                }
                if(obj.GetComponent<MeshFilter>() && obj.GetComponent<MeshFilter>().sharedMesh)
                    objData["shape"] = obj.GetComponent<MeshFilter>().sharedMesh.name;
                objData["_objectname"] = objData["m_Name"];
                objects.Add(so);
                data[index.ToString()] = objData;
                new_format_data["articles"].Add(objData);
                index++;
                so.ApplyModifiedProperties();
            }


            ActionComponent[] actionComponents = obj.GetComponents<ActionComponent>();
            if (actionComponents != null && actionComponents.Length > 0)
            {
                foreach (ActionComponent actionComponent in actionComponents)
                {
                    Dictionary<string, object> actionData = new Dictionary<string, object>();
                    Dictionary<string, object> triggerData = new Dictionary<string, object>();
                    Dictionary<string, object> responseData = new Dictionary<string, object>();

                    triggerData["sourceObj"] = actionComponent.gameObject.name;
                    if (actionComponent.targetObject != null)
                    {
                        responseData["targetObj"] = actionComponent.targetObject.name;
                    }
                    else
                    {
                        responseData["targetObj"] = "none";
                    }
                    if (actionComponent.trigger != null)
                    {
                        string ActResId = actionComponent.trigger.name;
                        ActResId = ActResId.Substring(0, ActResId.IndexOf("_Trigger"));
                        actionData["actresid"] = ActResId;
                        string TrigName = actionComponent.trigger.GetType().Name;
                        switch (TrigName) {
                            case "UserClickTrigger":
                                triggerData["isCollision"] = "false";
                                triggerData["action"] = "input";
                                triggerData["inputType"] = "click";
                                triggerData["change_property_by"] = "none";
                                break;
                            case "AngleTrigger":
                                triggerData["isCollision"] = "false";
                                triggerData["action"] = "change";
                                triggerData["inputType"] = "none";
                                AngleTrigger angleTrigger = actionComponent.trigger as AngleTrigger;
                                if (angleTrigger != null)
                                {
                                    triggerData["change_property_by"] = new Dictionary<string, Dictionary<string, string>>
                                    {
                                        { 
                                            "Transform_initialrotation", new Dictionary<string, string>
                                            {
                                                { "x", angleTrigger.fallThreshold_x.ToString() },
                                                { "y", angleTrigger.fallThreshold_y.ToString() },
                                                { "z", angleTrigger.fallThreshold_z.ToString() }
                                            }
                                        }
                                    };
                                }
                                break;
                            case "CollisionTrigger":
                                triggerData["isCollision"] = "true";
                                triggerData["action"] = "none";
                                triggerData["inputType"] = "none";
                                triggerData["change_property_by"] = "none";
                                break;
                            default:
                                triggerData["isCollision"] = "false";
                                triggerData["action"] = "none";
                                triggerData["inputType"] = "none";
                                triggerData["change_property_by"] = "none";
                                break;
                        }
                    }
                    else
                    {
                        triggerData["isCollision"] = "false";
                        triggerData["action"] = "none";
                        triggerData["inputType"] = "none";
                        triggerData["change_property_by"] = "none";
                    }

                    if (actionComponent.response != null)
                    {
                        string RespName = actionComponent.response.GetType().Name;
                        Debug.Log(RespName);
                        switch (RespName) {
                            case "DisappearBehavior":
                                responseData["isCollision"] = "false";
                                responseData["response"] = "disappear";
                                responseData["force"] = "none";
                                break;
                            case "MoveForwardBehavior":
                                responseData["isCollision"] = "false";
                                responseData["response"] = "force";
                                MoveForwardBehavior moveForwardBehavior = actionComponent.response as MoveForwardBehavior;
                                if(moveForwardBehavior != null)
                                {
                                    responseData["force"] = new Dictionary<string, string>
                                    {
                                        { "force_x", moveForwardBehavior.force_x.ToString() },
                                        { "force_y", moveForwardBehavior.force_y.ToString() },
                                        { "force_z", moveForwardBehavior.force_z.ToString() },
                                        { "type", moveForwardBehavior.type.ToString() }
                                    };
                                }
                                break;
                            case "CollisionBehavior":
                                responseData["isCollision"] = "true";
                                responseData["response"] = "none";
                                responseData["force"] = "none";
                                break;
                            default:
                                responseData["isCollision"] = "false";
                                responseData["response"] = "none";
                                responseData["force"] = "none";
                                break;
                        }
                    }
                    else
                    {
                        responseData["isCollision"] = "false";
                        responseData["response"] = "none";
                        responseData["force"] = "none";
                    }

                    actionData["trigger_event"] = triggerData;
                    actionData["response_event"] = responseData;

                    actres_data["ObjAction"].Add(actionData);
                }
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
        string json_actres = JsonConvert.SerializeObject(actres_data, Formatting.Indented);
        // Debug.Log(json);
        // Save the JSON string to a file
        string directory_path = "Assets/specifications/version_" + version_no;
        if (!Directory.Exists(directory_path))
            Directory.CreateDirectory(directory_path);

        File.WriteAllText(directory_path + "/article_old.json", json);
        File.WriteAllText(directory_path + "/article.json", json_new_format);
        File.WriteAllText(directory_path + "/action-response.json", json_actres);
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