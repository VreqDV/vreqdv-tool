using UnityEngine;

[CreateAssetMenu(fileName = "NewDisappearBehavior", menuName = "Behaviors/Disappear")]
public class DisappearBehavior : ResponseTemplate
{
    public override void Execute(Rigidbody rb)
    {
        rb.gameObject.SetActive(false);
        Debug.Log("Object has disappeared");
    }
}
