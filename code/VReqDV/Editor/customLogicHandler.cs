// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
// using System.IO;
// using System;
// using Newtonsoft.Json;
// using UnityEditor.SceneManagement;
// using Newtonsoft.Json.Linq;
// using HF = HelperFunctions;

// public class customLogicHandler
// {
//     public static void CreateCustomLogics(string directory_path)
//     {
//         string jsonData = File.ReadAllText(directory_path + "/customLogic.json");
//         CustomLogicList logicDataList = JsonConvert.DeserializeObject<CustomLogicList>(jsonData);
//         // Debug.Log("started creating custom logics");
//         foreach (CustomLogic logicData in logicDataList.customLogics)
//         {
//             Debug.Log("Creating Custom Logic: " + logicData.logic_name);
//             GameObject go = new GameObject(logicData.logic_name);
//             CustomLogicComponent logicComponent = go.AddComponent<CustomLogicComponent>();
//             logicComponent.parameters = logicData.parameters;
//             // Additional setup based on logicData can be done here
//         }
//     }
// }