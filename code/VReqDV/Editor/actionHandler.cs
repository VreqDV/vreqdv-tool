using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using Newtonsoft.Json;
using UnityEditor.SceneManagement;
using Newtonsoft.Json.Linq;
using HF = HelperFunctions;

public class ActionHandler
{
    public static void CreateActions(string directory_path)
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
                    UnityEditor.AssetDatabase.CreateAsset(trigger, $"Assets/VReqDV/Triggers/{actionResponse.actresid}_Trigger.asset");
                if(response != null)
                    UnityEditor.AssetDatabase.CreateAsset(response, $"Assets/VReqDV/Responses/{actionResponse.actresid}_Response.asset");
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
    public static TriggerTemplate CreateTrigger(TriggerEvent triggerEvent)
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
                
                if (changedProperties != null && changedProperties.ContainsKey("Transform_initialpos"))
                {
                    var positionProperties = changedProperties["Transform_initialpos"] as JObject;
                    if (positionProperties != null)
                    {
                        PositionChange positionChange = ScriptableObject.CreateInstance<PositionChange>();

                        if (positionProperties.TryGetValue("x", out JToken xPositionToken))
                        {
                            positionChange.fallThreshold_x = xPositionToken.ToObject<float>();
                        }
                        if (positionProperties.TryGetValue("y", out JToken yPositionToken))
                        {
                            positionChange.fallThreshold_y = yPositionToken.ToObject<float>();
                        }
                        if (positionProperties.TryGetValue("z", out JToken zPositionToken))
                        {
                            positionChange.fallThreshold_z = zPositionToken.ToObject<float>();
                        }
                        return positionChange;
                    }
                }
            }
        }

        if (triggerEvent.action == "input")
        {
            // UserClickTrigger userClickTrigger = ScriptableObject.CreateInstance<UserClickTrigger>();
            // return userClickTrigger;
            VRRaySelectTrigger raySelectTrigger = ScriptableObject.CreateInstance<VRRaySelectTrigger>();
            return raySelectTrigger;
        }

        // Add more conditions for other trigger types
        return null;
    }

    public static ResponseTemplate CreateResponse(ResponseEvent responseEvent)
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

        if (responseEvent.response == "change")
        {
            if (responseEvent.change_property_by != null)
            {
                var changedProperties = responseEvent.change_property_by as JObject;
                if (changedProperties != null && changedProperties.ContainsKey("Transform_initialpos"))
                {
                    var positionProperties = changedProperties["Transform_initialpos"] as JObject;
                    if (positionProperties != null)
                    {
                        PlaceObjectOn placeObjectOn = ScriptableObject.CreateInstance<PlaceObjectOn>();

                        if (positionProperties.TryGetValue("x", out JToken xPositionToken))
                        {
                            placeObjectOn.pos_x = xPositionToken.ToObject<float>();
                        }
                        if (positionProperties.TryGetValue("y", out JToken yPositionToken))
                        {
                            placeObjectOn.pos_y = yPositionToken.ToObject<float>();
                        }
                        if (positionProperties.TryGetValue("z", out JToken zPositionToken))
                        {
                            placeObjectOn.pos_z = zPositionToken.ToObject<float>();
                        }
                        return placeObjectOn;
                    }
                }
                // if (changedProperties != null && changedProperties.ContainsKey("Transform_initialpos"))
                // {
                //     ChangeAngleBehavior changeAngleBehavior = ScriptableObject.CreateInstance<ChangeAngleBehavior>();

                //     if (changedProperties.TryGetValue("Transform_initialrotation", out JToken newAngleToken) && newAngleToken is JObject newAngleObject)
                //     {
                //         float x = newAngleObject.TryGetValue("x", out JToken xToken) ? xToken.ToObject<float>() : 0f;
                //         float y = newAngleObject.TryGetValue("y", out JToken yToken) ? yToken.ToObject<float>() : 0f;
                //         float z = newAngleObject.TryGetValue("z", out JToken zToken) ? zToken.ToObject<float>() : 0f;

                //         changeAngleBehavior.newAngle = new Vector3(x, y, z);
                //     }

                //     return changeAngleBehavior;

                // }
            }
        }

        return null;
    }

}