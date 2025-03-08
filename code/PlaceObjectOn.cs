using UnityEngine;

[CreateAssetMenu(fileName = "NewBehavior", menuName = "Behaviors/PlaceObjectOn")]
public class PlaceObjectOn : ResponseTemplate
{
    public float pos_x = 0f;
    public float pos_y = 0.5f;
    public float pos_z = 0f;
    public override void Execute(GameObject obj)
    {
        obj.transform.position = new Vector3(pos_x, pos_y, pos_z);
    }
}