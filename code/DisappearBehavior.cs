using UnityEngine;

[CreateAssetMenu(fileName = "NewDisappearBehavior", menuName = "Behaviors/Disappear")]
public class DisappearBehavior : ResponseTemplate
{
    public override void Execute(GameObject obj)
    {
        obj.SetActive(false);
        Debug.Log("Object has disappeared");
    }
}
