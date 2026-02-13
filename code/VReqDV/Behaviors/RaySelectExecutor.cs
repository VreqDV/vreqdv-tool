using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[CreateAssetMenu(menuName = "Triggers/VR Ray Select TriggerTemplate")]
public class VRRaySelectTrigger : TriggerTemplate
{
    public override bool IsTriggered(GameObject obj)
    {
        // Check if object was selected by a ray interactor in this frame
        return IsSelectedByRay(obj);
    }

    private bool IsSelectedByRay(GameObject obj)
    {
        // Find all active XRRayInteractors in the scene
        XRRayInteractor[] rayInteractors = Object.FindObjectsOfType<XRRayInteractor>();

        foreach (XRRayInteractor rayInteractor in rayInteractors)
        {
            // Check if this interactor currently has a selected object
            if (rayInteractor.hasSelection && rayInteractor.firstInteractableSelected != null)
            {
                var selectedObj = rayInteractor.firstInteractableSelected.transform.gameObject;
                if (selectedObj == obj)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
