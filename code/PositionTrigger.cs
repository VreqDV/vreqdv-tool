using UnityEngine;

[CreateAssetMenu(menuName = "Triggers/Position TriggerTemplate")]
public class PositionTrigger : TriggerTemplate
{
    public Vector3 targetPosition;
    public float threshold = 0.1f;

    public override bool IsTriggered(GameObject obj)
    {
        return Vector3.Distance(obj.transform.position, targetPosition) < threshold;
    }
}
