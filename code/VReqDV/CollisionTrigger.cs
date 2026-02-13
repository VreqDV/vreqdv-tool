using UnityEngine;

[CreateAssetMenu(menuName = "Triggers/Collision Trigger")]
public class CollisionTrigger : TriggerTemplate
{

    public override bool IsTriggered(GameObject obj)
    {
        if(obj.GetComponent<Rigidbody>() == null)
        {
            obj.AddComponent<Rigidbody>();
        }
        return true;
    }
}
