using UnityEngine;

[CreateAssetMenu(fileName = "CollisionBehavior", menuName = "Behaviors/Collision")]
public class CollisionBehavior : ResponseTemplate
{
    public override void Execute(GameObject obj)
    {
        if(obj.GetComponent<Rigidbody>() == null)
        {
            obj.AddComponent<Rigidbody>();
        }
    }
}
