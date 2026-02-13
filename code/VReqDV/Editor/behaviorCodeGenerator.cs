using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class BehaviorCodeGenerator
{
    public static void CreateBehaviors(string directory_path, Dictionary<string, List<string>> inheritanceMap = null)
    {
        string jsonData = File.ReadAllText(directory_path + "/behavior.json");

        // Parse JSON → runtime rules
        var behaviorRules = BehaviorJsonParser.Parse(jsonData);

        // Extract version from directory path (e.g., "version_16" -> "Version_16")
        string dirName = new DirectoryInfo(directory_path).Name;
        string version = char.ToUpper(dirName[0]) + dirName.Substring(1);

        List<BehaviorRule> finalRules = new List<BehaviorRule>();

        foreach (var rule in behaviorRules.behaviors)
        {
            finalRules.Add(rule);

            // Expand inherited behaviors
            if (inheritanceMap != null && inheritanceMap.ContainsKey(rule.Source))
            {
                foreach (string derivedObj in inheritanceMap[rule.Source])
                {
                    BehaviorRule newRule = DeepCopyRule(rule);
                    newRule.Id = rule.Id + "_" + derivedObj;
                    newRule.Source = derivedObj;
                    
                    // Recursive string replacement
                    ReplaceParams(newRule.Precondition, rule.Source, derivedObj);
                    ReplaceParams(newRule.Action, rule.Source, derivedObj);
                    if (newRule.Postcondition != null)
                    {
                        ReplaceParams(newRule.Postcondition, rule.Source, derivedObj);
                    }

                    finalRules.Add(newRule);
                }
            }
        }
        
        // Update the list with expanded rules
        behaviorRules.behaviors = finalRules;

        foreach (var rule in behaviorRules.behaviors)
        {
            GenerateBehavior(rule, "Assets/VReqDV/Generated/Behaviors/" + version, version);
        }
        
        GenerateLoader(behaviorRules, "Assets/VReqDV/Generated/Behaviors/" + version, version);

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    private static BehaviorRule DeepCopyRule(BehaviorRule rule)
    {
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(rule);
        return Newtonsoft.Json.JsonConvert.DeserializeObject<BehaviorRule>(json);
    }

    private static void ReplaceParams(ConditionNode node, string oldName, string newName)
    {
        if (node == null) return;

        if (node.all != null) foreach (var child in node.all) ReplaceParams(child, oldName, newName);
        if (node.any != null) foreach (var child in node.any) ReplaceParams(child, oldName, newName);
        
        if (node.equals != null)
        {
            for (int i = 0; i < node.equals.Count; i++)
            {
                node.equals[i] = node.equals[i].Replace(oldName, newName);
            }
        }

        if (node.@params != null)
        {
            var keys = new List<string>(node.@params.Keys);
            foreach (var key in keys)
            {
                node.@params[key] = node.@params[key].Replace(oldName, newName);
            }
        }
    }

    private static void ReplaceParams(ActionNode node, string oldName, string newName)
    {
        if (node == null) return;
        
        if (node.@params != null)
        {
            var keys = new List<string>(node.@params.Keys);
            foreach (var key in keys)
            {
                node.@params[key] = node.@params[key].Replace(oldName, newName);
            }
        }
    }

    public static void GenerateBehavior(
        BehaviorRule rule,
        string outputFolder,
        string version
    )
    {
        Debug.Log($"Generating behavior for file: {rule.Id} (Version: {version})");
        Directory.CreateDirectory(outputFolder);

        string code = rule.Event == "OnCondition"
            ? GenerateOnCondition(rule, version)
            : GenerateOnStateChange(rule, version);

        File.WriteAllText(
            Path.Combine(outputFolder, $"{rule.Id}.cs"),
            code
        );
    }

    // ------------------------------------------------------------
    // ON CONDITION TEMPLATE (Polling)
    // ------------------------------------------------------------
    private static string GenerateOnCondition(BehaviorRule rule, string version)
    {
        var sb = new StringBuilder();

        sb.AppendLine("// GENERATED FILE — DO NOT EDIT");
        sb.AppendLine("using UnityEngine;");
        sb.AppendLine();
        sb.AppendLine($"namespace {version}");
        sb.AppendLine("{");
        sb.AppendLine($"    public class {rule.Id} : MonoBehaviour");
        sb.AppendLine("    {");
        sb.AppendLine("        void Update()");
        sb.AppendLine("        {");
        sb.AppendLine($"            if ({GenerateCondition(rule.Precondition)})");
        sb.AppendLine("            {");
        
        string args = GetParamsString(rule.Action?.@params);
        sb.AppendLine($"                UserAlgorithms.{rule.ActionAlgorithm}({args});");
        
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine("    }");
        sb.AppendLine("}");

        return sb.ToString();
    }

    // ------------------------------------------------------------
    // ON STATE CHANGE TEMPLATE (Subscription)
    // ------------------------------------------------------------
    private static string GenerateOnStateChange(BehaviorRule rule, string version)
    {
        var sb = new StringBuilder();

        sb.AppendLine("// GENERATED FILE — DO NOT EDIT");
        sb.AppendLine("using UnityEngine;");
        sb.AppendLine();
        sb.AppendLine($"namespace {version}");
        sb.AppendLine("{");
        sb.AppendLine($"    public class {rule.Id} : MonoBehaviour");
        sb.AppendLine("    {");
        sb.AppendLine("        void OnEnable()");
        sb.AppendLine("        {");
        sb.AppendLine($"            {rule.Source}StateStorage.OnStateChanged += Handle;");
        sb.AppendLine("        }");
        sb.AppendLine();
        sb.AppendLine("        void OnDisable()");
        sb.AppendLine("        {");
        sb.AppendLine($"            {rule.Source}StateStorage.OnStateChanged -= Handle;");
        sb.AppendLine("        }");
        sb.AppendLine();
        sb.AppendLine($"        void Handle(GameObject obj, {rule.Source}StateEnum newState)");
        sb.AppendLine("        {");
        sb.AppendLine($"            if ({GenerateCondition(rule.Precondition)})");
        sb.AppendLine("            {");
        
        string args = GetParamsString(rule.Action?.@params);
        sb.AppendLine($"                UserAlgorithms.{rule.ActionAlgorithm}({args});");
        
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine("    }");
        sb.AppendLine("}");

        return sb.ToString();
    }

    // ------------------------------------------------------------
    // CONDITION GENERATOR (runtime ConditionNode)
    // ------------------------------------------------------------
    private static string GenerateCondition(ConditionNode condition)
    {
        if (condition == null)
        {
            return "true";
        }

        if (condition.all != null && condition.all.Count > 0)
        {
            var parts = new List<string>();
            foreach (var c in condition.all)
            {
                parts.Add(GenerateCondition(c));
            }
            return "(" + string.Join(" && ", parts) + ")";
        }

        if (condition.any != null && condition.any.Count > 0)
        {
            var parts = new List<string>();
            foreach (var c in condition.any)
            {
                parts.Add(GenerateCondition(c));
            }
            return "(" + string.Join(" || ", parts) + ")";
        }

        if (!string.IsNullOrEmpty(condition.left) &&
            !string.IsNullOrEmpty(condition.right))
        {
            if (condition.left.EndsWith(".state"))
            {
                string objName = condition.left.Substring(0, condition.left.LastIndexOf(".state"));
                string stateValue = Pascal(condition.right);
                return $"{objName}StateStorage.Get(GameObject.Find(\"{objName}\")) == {objName}StateEnum.{stateValue}";
            }
            return $"{condition.left} == {condition.right}";
        }

        if (!string.IsNullOrEmpty(condition.runAlgorithm))
        {
            string args = GetParamsString(condition.@params);
            return $"UserAlgorithms.{condition.runAlgorithm}({args})";
        }

        return "true";
    }

    private static string GetParamsString(Dictionary<string, string> parameters)
    {
        if (parameters != null && parameters.ContainsKey("obj"))
        {
            return $"GameObject.Find(\"{parameters["obj"]}\")";
        }
        return "";
    }

    private static string Pascal(string value)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return char.ToUpper(value[0]) + value.Substring(1);
    }

    public static void GenerateLoader(BehaviorList list, string outputFolder, string version)
    {
        var sb = new StringBuilder();

        sb.AppendLine("// GENERATED FILE — DO NOT EDIT");
        sb.AppendLine("using UnityEngine;");
        sb.AppendLine();
        sb.AppendLine($"public static class BehaviorLoader_{version}");
        sb.AppendLine("{");
        sb.AppendLine("    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]");
        sb.AppendLine("    public static void AttachBehaviors()");
        sb.AppendLine("    {");

        foreach (var rule in list.behaviors)
        {
            // Attach("...", typeof(Version_XX.BehaviorName))
            sb.AppendLine($"        Attach(\"{rule.Source}\", typeof({version}.{rule.Id}));");
            // Also attach the Initializer if it exists. Initializers are likely global since they handle state storage which might be shared or versioned?
            // Assuming StateAPI logic is global for now (User didn't ask to namespace that).
            // But if StateStorage is global, there's only one state for "Ball".
            // Since we are namespacing behavior, we'll just attach the Initializer by name.
            string initializerName = rule.Source + "Initializer";
            sb.AppendLine($"        AttachInitializer(\"{rule.Source}\", \"{version}.{initializerName}\");");
        }

        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    private static void Attach(string objName, System.Type type)");
        sb.AppendLine("    {");
        sb.AppendLine("        GameObject obj = GameObject.Find(objName);");
        sb.AppendLine("        if (obj != null)");
        sb.AppendLine("        {");
        sb.AppendLine("            if (obj.GetComponent(type) == null)");
        sb.AppendLine("            {");
        sb.AppendLine("                obj.AddComponent(type);");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    private static void AttachInitializer(string objName, string typeName)");
        sb.AppendLine("    {");
        sb.AppendLine("        GameObject obj = GameObject.Find(objName);");
        sb.AppendLine("        if (obj != null)");
        sb.AppendLine("        {");
        sb.AppendLine("            System.Type type = System.Type.GetType(typeName);");
        sb.AppendLine("            if (type != null && obj.GetComponent(type) == null)");
        sb.AppendLine("            {");
        sb.AppendLine("                obj.AddComponent(type);");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine("    }");
        sb.AppendLine("}");

        File.WriteAllText(Path.Combine(outputFolder, $"BehaviorLoader_{version}.cs"), sb.ToString());
    }
}
