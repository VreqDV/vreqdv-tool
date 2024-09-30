using UnityEngine;

[CreateAssetMenu(fileName = "NewBehavior", menuName = "Behaviors/MoveForward")]
public class MoveForwardBehavior : ResponseTemplate
{
    public override void Execute(Rigidbody rb)
    {
        // Vector3 forceDirection = rb.transform.TransformDirection(Vector3.forward);
        Vector3 forceDirection = new Vector3(1.0f, 0.0f, 0.0f);
        float strength = 10f; // Adjust the strength as needed
        rb.AddForce(forceDirection * strength, ForceMode.Impulse);
        Debug.Log("Force added to move object forward");
    }
}