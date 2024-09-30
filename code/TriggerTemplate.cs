using UnityEngine;

public abstract class TriggerTemplate : ScriptableObject
{
    public abstract bool IsTriggered(GameObject obj);
}
