// using UnityEngine;

// public class ClickExecutor : MonoBehaviour
// {
//     public ResponseTemplate behavior;

//     private void OnMouseDown()
//     {
//         behavior.Execute(GetComponent<Rigidbody>());
//     }
// }
using UnityEngine;

[CreateAssetMenu(menuName = "Triggers/User Click TriggerTemplate")]
public class UserClickTrigger : TriggerTemplate
{
    public override bool IsTriggered(GameObject obj)
    {
        // This trigger activates instantly upon user click, so return true immediately
        return Input.GetMouseButtonDown(0) && IsMouseOverObject(obj);
    }

    private bool IsMouseOverObject(GameObject obj)
    {
        // Raycast to detect if the mouse is over the object
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject == obj;
        }

        return false;
    }
}
