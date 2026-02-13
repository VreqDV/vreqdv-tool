// GENERATED FILE — DO NOT EDIT
using UnityEngine;

public static class BehaviorLoader_Version_15
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void AttachBehaviors()
    {
        Attach("Ball", typeof(Version_15.BallClickToRoll));
        AttachInitializer("Ball", "Version_15.BallInitializer");
        Attach("Ball", typeof(Version_15.BallStopAtEnd));
        AttachInitializer("Ball", "Version_15.BallInitializer");
        Attach("Pin_1", typeof(Version_15.PinFall));
        AttachInitializer("Pin_1", "Version_15.Pin_1Initializer");
        Attach("Pin_2", typeof(Version_15.PinFall_Pin_2));
        AttachInitializer("Pin_2", "Version_15.Pin_2Initializer");
        Attach("Pin_3", typeof(Version_15.PinFall_Pin_3));
        AttachInitializer("Pin_3", "Version_15.Pin_3Initializer");
        Attach("", typeof(Version_15.PinReset));
        AttachInitializer("", "Version_15.Initializer");
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
