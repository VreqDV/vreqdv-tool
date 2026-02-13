// GENERATED FILE — DO NOT EDIT
using UnityEngine;

public static class BehaviorLoader_Version_16
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void AttachBehaviors()
    {
        Attach("Box", typeof(Version_16.BoxActivate));
        AttachInitializer("Box", "BoxInitializer");
        Attach("Box", typeof(Version_16.BoxColorChange));
        AttachInitializer("Box", "BoxInitializer");
    }

    private static void Attach(string objName, System.Type type)
    {
        GameObject obj = GameObject.Find(objName);
        if (obj != null)
        {
            if (obj.GetComponent(type) == null)
            {
                obj.AddComponent(type);
            }
        }
    }

    private static void AttachInitializer(string objName, string typeName)
    {
        GameObject obj = GameObject.Find(objName);
        if (obj != null)
        {
            System.Type type = System.Type.GetType(typeName);
            if (type != null && obj.GetComponent(type) == null)
            {
                obj.AddComponent(type);
            }
        }
    }
}
