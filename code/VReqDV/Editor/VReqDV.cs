using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using Newtonsoft.Json;
using UnityEditor.SceneManagement;
using Newtonsoft.Json.Linq;
using HF = HelperFunctions;
using UnityEngine.XR.Interaction.Toolkit;

public class MainMenu : EditorWindow
{
    private static MainMenu window;
    [MenuItem("Window/VReqDV")]
    public static void ShowWindow()
    {
        window = GetWindow<MainMenu>("VReqDV");
    }

    private onScreenState screenState;

    public BehaviorList behaviorSpecifications;
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
        versionSpecs = Directory.GetDirectories("Assets/VReqDV/specifications");
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
                string dir_path = $"Assets/VReqDV/ScenePrefabs/version_{screenState.curr_version}";
                if(!Directory.Exists(dir_path))
                    SaveSceneToPrefab(screenState.curr_version);
            }

            GUILayout.Label("Compare with Version:", setFont(12));
            
            compare_version = EditorGUILayout.Popup(compare_version, version_list, GUILayout.Width(100));

            EditorGUI.BeginDisabledGroup(editingEnabled);
            if(GUILayout.Button("Display Comparison", GUILayout.Width(200)))
            {
                CompareHandler.ComparisonSideBySide(screenState.curr_version, compare_version);
            }
            // if(GUILayout.Button("Open in New Scene", GUILayout.Width(200)))
            // {
            //     OpenNewScene();
            // }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();

            try
            {
                string file = "Assets/VReqDV/specifications/version_" + screenState.curr_version + "/article.json";
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
                string file = "Assets/VReqDV/specifications/version_" + screenState.curr_version + "/action-response.json";
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
                    string file = "Assets/VReqDV/specifications/version_" + compare_version + "/article.json";
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
                    string file = "Assets/VReqDV/specifications/version_" + compare_version + "/action-response.json";
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
                string filePath1 = $"Assets/VReqDV/specifications/version_{screenState.curr_version}/article.json";
                string filePath2 = $"Assets/VReqDV/specifications/version_{screenState.curr_version}/action-response.json";
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
        string folder_path = "Assets/VReqDV/specifications/version_" + version_no;
        if (Directory.Exists(folder_path))
        {
            var inheritanceMap = ObjectHandler.CreateObjects(folder_path);
            // ActionHandler.CreateActions(folder_path);
            BehaviorCodeGenerator.CreateBehaviors(folder_path, inheritanceMap);
        }
        else
        {
            Debug.Log("Version Directory not found");
        }
    }

    private void SaveSceneToPrefab(int version)
    {
        string prefabDirectory = $"Assets/VReqDV/ScenePrefabs/version_{version}";
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
        var activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        Debug.Log($"ClearObjects(): Active Scene: {activeScene.name}");
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>(true); // includeInactive=true

        foreach (GameObject obj in allObjects)
        {
            if (obj.scene != activeScene || obj.transform.parent != null)
                continue;

            // Skip if this object has XRInteractionManager or XROrigin component
            if (obj.name == "XR Interaction Manager" || obj.name == "XR Origin (XR Rig)")
                continue;

            // deleting root objects automatically removes children too
            if (obj.scene == activeScene && obj.transform.parent == null)
            {
                UnityEngine.Object.DestroyImmediate(obj);
            }
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

                    if (changedProperties.ContainsKey("Transform_intitialpos"))
                    {
                        EditorGUILayout.LabelField("Change in Position", EditorStyles.boldLabel);
                        if (changedProperties["Transform_initialpos"] != null)
                        {
                            var pos = changedProperties["Transform_initialpos"] as JObject;

                            string xValue = pos["x"]?.ToString();
                            string yValue = pos["y"]?.ToString();
                            string zValue = pos["z"]?.ToString();

                            xValue = EditorGUILayout.TextField("x: ", xValue);
                            yValue = EditorGUILayout.TextField("y: ", yValue);
                            zValue = EditorGUILayout.TextField("z: ", zValue);

                            pos["x"] = xValue;
                            pos["y"] = yValue;
                            pos["z"] = zValue;

                            changedProperties["Transform_initialpos"] = pos;
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

                if (actionResponse.response_event.response == "change")
                {
                    var changedProperties = actionResponse.trigger_event.change_property_by as JObject;
                    if (changedProperties != null)
                    {
                        if (changedProperties.ContainsKey("Transform_initialpos"))
                        {
                            EditorGUILayout.LabelField("Place Object On", EditorStyles.boldLabel);
                            if (changedProperties["Transform_initialpos"] != null)
                            {
                                var pos = changedProperties["Transform_initialpos"] as JObject;

                                string xValue = pos["x"]?.ToString();
                                string yValue = pos["y"]?.ToString();
                                string zValue = pos["z"]?.ToString();

                                xValue = EditorGUILayout.TextField("x: ", xValue);
                                yValue = EditorGUILayout.TextField("y: ", yValue);
                                zValue = EditorGUILayout.TextField("z: ", zValue);

                                pos["x"] = xValue;
                                pos["y"] = yValue;
                                pos["z"] = zValue;

                                changedProperties["Transform_initialpos"] = pos;
                            }
                        }
                        actionResponse.response_event.change_property_by = changedProperties;
                    }
                }
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }
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
                objData["Transform_initialpos"] = HF.GetTransformInitialPosition(obj);
                objData["XRRigidObject"] = HF.GetXRRigidObject(obj);
                objData["Transform_objectscale"] = HF.GetTransformObjectScale(obj);
                objData["Transform_initialrotation"] = HF.GetTransformInitialRotation(obj);
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