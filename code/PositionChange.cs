using UnityEngine;

[CreateAssetMenu(menuName = "Triggers/Position Change")]
public class PositionChange : TriggerTemplate
{
    public float fallThreshold_x = -10000f; // Angle at which the object is considered to have "fallen"
    public float fallThreshold_y = -10000f;
    public float fallThreshold_z = -10000f;

    public override bool IsTriggered(GameObject obj)
    {
       float posX = obj.transform.position.x;
       float posY = obj.transform.position.y;
       float posZ = obj.transform.position.z;

        if (posX < fallThreshold_x || posY < fallThreshold_y || posZ < fallThreshold_z)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

       return posX < fallThreshold_x || posY < fallThreshold_y || posZ < fallThreshold_z;
    }
}
