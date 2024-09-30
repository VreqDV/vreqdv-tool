using UnityEngine;

[CreateAssetMenu(menuName = "Triggers/Angle Trigger")]
public class AngleTrigger : TriggerTemplate
{
    public float fallThreshold_x = 45f; // Angle at which the object is considered to have "fallen"
    public float fallThreshold_y = 45f;
    public float fallThreshold_z = 45f;

    public override bool IsTriggered(GameObject obj)
    {
        float angleX = Mathf.Abs(obj.transform.eulerAngles.x);
        float angleY = Mathf.Abs(obj.transform.eulerAngles.y);
        float angleZ = Mathf.Abs(obj.transform.eulerAngles.z);

        // Adjust angles for better comparison
        if (angleX > 180f) angleX = 360f - angleX;
        if (angleY > 180f) angleY = 360f - angleY;
        if (angleZ > 180f) angleZ = 360f - angleZ;

        return angleX > fallThreshold_x || angleY > fallThreshold_y || angleZ > fallThreshold_z;
    }
}
