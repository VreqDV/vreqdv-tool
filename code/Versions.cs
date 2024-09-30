// using UnityEngine;
// using System.Collections.Generic;
// using System.IO;
// using UnityEditor;
// using Newtonsoft.Json;

// public class Versions {
//     private PrimitiveType GetPrimitiveTypeByString(string shape)
//     {
//         switch (shape)
//         {
//             case "sphere":
//                 return PrimitiveType.Sphere;
//             case "cube":
//                 return PrimitiveType.Cube;
//             default:
//                 return PrimitiveType.Cube;
//         }
//     }

//     private Dictionary<string, object> GetTransformInitialPosition(GameObject obj)
//     {
//         Dictionary<string, object> transformInitialPosition = new Dictionary<string, object>();

//         if (obj.transform != null)
//         {
//             transformInitialPosition["#x"] = obj.transform.position.x;
//             transformInitialPosition["#y"] = obj.transform.position.y;
//             transformInitialPosition["#z"] = obj.transform.position.z;
//         }
//         else
//         {
//             transformInitialPosition["#x"] = 0;
//             transformInitialPosition["#y"] = 0;
//             transformInitialPosition["#z"] = 0;
//         }

//         return transformInitialPosition;
//     }

//     private Dictionary<string, object> GetTransformObjectScale(GameObject obj)
//     {
//         Dictionary<string, object> transformObjectScale = new Dictionary<string, object>();

//         if (obj.transform != null)
//         {
//             transformObjectScale["#x"] = obj.transform.localScale.x;
//             transformObjectScale["#y"] = obj.transform.localScale.y;
//             transformObjectScale["#z"] = obj.transform.localScale.z;
//         }
//         else
//         {
//             transformObjectScale["#x"] = 1;
//             transformObjectScale["#y"] = 1;
//             transformObjectScale["#z"] = 1;
//         }

//         return transformObjectScale;
//     }

//     private Dictionary<string, object> GetXRRigidObject(GameObject obj)
//     {
//         Dictionary<string, object> xrrigidObject = new Dictionary<string, object>();

//         if (obj.GetComponent<Rigidbody>() != null)
//         {
//             Rigidbody rigidObject = obj.GetComponent<Rigidbody>();
//             xrrigidObject["value"] = 1;
//             xrrigidObject["mass"] = rigidObject.mass;
//             xrrigidObject["dragfriction"] = rigidObject.drag;
//             xrrigidObject["angulardrag"] = rigidObject.drag;
//             xrrigidObject["Isgravityenable"] = rigidObject.useGravity;
//             xrrigidObject["IsKinematic"] = rigidObject.isKinematic;
//             if(rigidObject.interpolation == RigidbodyInterpolation.Interpolate)
//                 xrrigidObject["CanInterpolate"] = 1;
//             else if (rigidObject.interpolation == RigidbodyInterpolation.Extrapolate)
//                 xrrigidObject["CanInterpolate"] = 2;
//             else
//                 xrrigidObject["CanInterpolate"] = 0;
//             // xrrigidObject["CollisionPolling"] = rigidObject.collisionPolling;
//             switch (rigidObject.collisionDetectionMode)
//             {
//                 case CollisionDetectionMode.Discrete:
//                     xrrigidObject["CollisionPolling"] = "discrete";
//                     break;
//                 case CollisionDetectionMode.Continuous:
//                     xrrigidObject["CollisionPolling"] = "continuous";
//                     break;
//                 case CollisionDetectionMode.ContinuousDynamic:
//                     xrrigidObject["CollisionPolling"] = "continuous-dynamic";
//                     break;
//                 case CollisionDetectionMode.ContinuousSpeculative:
//                     xrrigidObject["CollisionPolling"] = "continuous-speculative";
//                     break;
//                 default:
//                     xrrigidObject["CollisionPolling"] = "none";
//                     break;
//             }
//         }
//         else
//         {
//             xrrigidObject["value"] = 0;
//             xrrigidObject["mass"] = 0;
//             xrrigidObject["dragfriction"] = 0;
//             xrrigidObject["angulardrag"] = 0;
//             xrrigidObject["Isgravityenable"] = false;
//             xrrigidObject["IsKinematic"] = false;
//             xrrigidObject["CanInterpolate"] = 0;
//             xrrigidObject["CollisionPolling"] = "none";
//         }

//         return xrrigidObject;
//     }

//     public void SaveVersion()
//     {
//         // File.WriteAllText(Application.dataPath + "/version_trial_" + index + ".json", {
//         //     hi: "hi"
//         // });
//         List<SerializedObject> objects = new List<SerializedObject>();

//         // Get all objects in the scene
//         GameObject[] allObjects = FindObjectsOfType<GameObject>();
//         Dictionary<string, Dictionary<string, object>> data = new Dictionary<string, Dictionary<string, object>>();
//         int index = 0;
//         foreach (GameObject obj in allObjects)
//         {
//             // Component[] components = obj.GetComponents<MonoBehaviour>();
//             // foreach (MonoBehaviour script in components)
//             // {
//             //     Debug.Log("scripts");
//             //     Debug.Log(script.GetType().Name);
//             //     if (script is ClickExecutor)
//             //     {
//             //         ClickExecutor executor = (ClickExecutor)script;
//             //         Debug.Log(executor.behavior);
//             //     }
//             // }

//             SerializedObject so = null;
//             if(obj != null)
//             {
//                 // Get the SerializedObject for the object
//                 Dictionary<string, object> objData = new Dictionary<string, object>();
//                 objData["Transform_initialpos"] = GetTransformInitialPosition(obj);
//                 objData["XRRigidObject"] = GetXRRigidObject(obj);
//                 objData["Transform_objectscale"] = GetTransformObjectScale(obj);
//                 so = new SerializedObject(obj);
//                 so.Update();

//                 SerializedProperty iterator = so.GetIterator();
//                 while (iterator.NextVisible(true))
//                 {
//                     // If the property is a container (array or object), recursively add its contents
//                     // Debug.Log(iterator.propertyType);
//                     if (iterator.propertyType == SerializedPropertyType.ObjectReference || iterator.propertyType == SerializedPropertyType.ArraySize)
//                     {
//                         if (iterator.isArray)
//                         {
//                             // Debug.Log("Array mei hoon");
//                             SerializedProperty element = iterator.Copy();
//                             element.Next(true);
//                             int count = iterator.arraySize;
//                             List<object> elementsData = new List<object>();
//                             for (int i = 0; i < count; i++)
//                             {
//                                 SerializedObject elementObj = new SerializedObject(element.objectReferenceValue);
//                                 elementObj.Update();
//                                 elementsData.Add(SaveObject(elementObj));
//                                 element.Next(false);
//                             }
//                             objData[iterator.name] = elementsData;
//                         }
//                         else
//                         {
//                             // Debug.Log(iterator.propertyType);
//                             // Debug.Log(iterator.objectReferenceValue);
//                             if (iterator.objectReferenceValue != null)
//                             {
//                                 // Debug.Log("Object Reference mei hoon");
//                                 SerializedObject elementObj = new SerializedObject(iterator.objectReferenceValue);
//                                 elementObj.Update();
//                                 objData[iterator.name] = SaveObject(elementObj);
//                             }
//                             // else
//                             // {
//                             //     Debug.Log("Object Reference ka value nahi mila");
//                             // }
//                         }
//                     }
//                     else
//                     {
//                         // Otherwise, just add the property value
//                         // Debug.Log("Otherwise");
//                         objData[iterator.name] = SaveProperty(iterator);
//                     }
//                 }
//                 objects.Add(so);
//                 data[index.ToString()] = objData;
//                 index++;
//                 so.ApplyModifiedProperties();
//             }
            
//         }
//         // Convert the dictionary to a JSON string
//         Debug.Log("Finally we have this structure");
//         foreach (KeyValuePair<string, System.Collections.Generic.Dictionary<string, object>> pair in data)
//         {
//             Debug.Log("Key: " + pair.Key + ", Value: " + pair.Value);
//             foreach (KeyValuePair<string, object> pair_inside in pair.Value)
//             {
//                 Debug.Log("Key: " + pair_inside.Key + ", Value: " + pair_inside.Value);
//             }
//         }
//         // string json = JsonUtility.ToJson(data, true);
//         string json = JsonConvert.SerializeObject(data, Formatting.Indented);
//         Debug.Log(json);
//         // Save the JSON string to a file
//         File.WriteAllText(Application.dataPath + "/version_" + onScreenState.curr_version + ".json", json);
//         Debug.Log("version number");
//         Debug.Log(onScreenState.curr_version);
//         // version_index++;
//     }

//     private object SaveProperty(SerializedProperty property)
//     {
//         switch (property.propertyType)
//         {
//             case SerializedPropertyType.Integer:
//                 return property.intValue;
//             case SerializedPropertyType.Boolean:
//                 return property.boolValue;
//             case SerializedPropertyType.Float:
//                 return property.floatValue;
//             case SerializedPropertyType.String:
//                 return property.stringValue;
//             case SerializedPropertyType.Color:
//                 return property.colorValue;
//             case SerializedPropertyType.Vector2:
//                 return property.vector2Value;
//             case SerializedPropertyType.Vector3:
//                 return property.vector3Value;
//             case SerializedPropertyType.Vector4:
//                 return property.vector4Value;
//             case SerializedPropertyType.Rect:
//                 return property.rectValue;
//             case SerializedPropertyType.ArraySize:
//                 return property.intValue;
//             case SerializedPropertyType.Character:
//                 return property.stringValue[0];
//             case SerializedPropertyType.ObjectReference:
//                 return property.objectReferenceValue ? property.objectReferenceValue.name : null;
//             default:
//                 return null;
//         }
//     }

//     private object SaveObject(SerializedObject obj)
//     {
//         Dictionary<string, object> objData = new Dictionary<string, object>();
//         obj.Update();

//         // Iterate through all the properties of the object
//         SerializedProperty iterator = obj.GetIterator();
//         while (iterator.NextVisible(true))
//         {
//             // If the property is a container (array or object), recursively add its contents
//             if (iterator.propertyType == SerializedPropertyType.ObjectReference || iterator.propertyType == SerializedPropertyType.ArraySize)
//             {
//                 if (iterator.isArray)
//                 {
//                     SerializedProperty element = iterator.Copy();
//                     element.Next(true);
//                     int count = iterator.arraySize;
//                     List<object> elementsData = new List<object>();
//                     for (int i = 0; i < count; i++)
//                     {
//                         SerializedObject elementObj = new SerializedObject(element.objectReferenceValue);
//                         elementObj.Update();
//                         elementsData.Add(SaveObject(elementObj));
//                         element.Next(false);
//                     }
//                     objData[iterator.name] = elementsData;
//                 }
//                 else
//                 {
//                     SerializedObject elementObj = new SerializedObject(iterator.objectReferenceValue);
//                     elementObj.Update();
//                     objData[iterator.name] = SaveObject(elementObj);
//                 }
//             }
//             else
//             {
//                 // Otherwise, just add the property value
//                 objData[iterator.name] = SaveProperty(iterator);
//             }
//         }

//         // Add the object's components to the dictionary
//         GameObject gameObject = obj.targetObject as GameObject;
//         if (gameObject != null)
//         {
//             Component[] components = gameObject.GetComponents<Component>();
//             foreach (Component component in components)
//             {
//                 SerializedObject componentObj = new SerializedObject(component);
//                 componentObj.Update();
//                 objData[component.GetType().Name] = SaveObject(componentObj);
//             }
//         }

//         obj.ApplyModifiedProperties();
//         return objData;
//     }
 
// }