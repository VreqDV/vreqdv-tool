using UnityEngine;

[CreateAssetMenu(fileName = "NewBehavior", menuName = "Behaviors/MoveForward")]
public class MoveForwardBehavior : ResponseTemplate
{
    public float force_x = 10f;
    public float force_y = 0f;
    public float force_z = 0f;
    public string type = "impulse";
    public override void Execute(GameObject obj)
    {
        Vector3 force_vec = new Vector3(force_x, force_y, force_z);
        ForceMode mode;
        switch (type.ToLower())
        {
            case "force":
                mode = ForceMode.Force;
                break;
            case "acceleration":
                mode = ForceMode.Acceleration;
                break;
            case "velocitychange":
                mode = ForceMode.VelocityChange;
                break;
            case "impulse":
            default:
                mode = ForceMode.Impulse;
                break;
        }
        if(obj.GetComponent<Rigidbody>() == null)
        {
            obj.AddComponent<Rigidbody>();
        }
        obj.GetComponent<Rigidbody>().AddForce(force_vec, mode);
        Debug.Log("Force added to move object forward");
    }
}