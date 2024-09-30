using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionComponent : MonoBehaviour
{
    // private ActionResponse data;
    // trigger event
    // response behavior
    public TriggerTemplate trigger;
    public ResponseTemplate response;
    public GameObject targetObject;

    void Update()
    {
        if (trigger != null && response != null && targetObject != null)
        {
            if (trigger.IsTriggered(gameObject))
            {
                Rigidbody rb = targetObject.GetComponent<Rigidbody>();
                response.Execute(rb);
            }
        }
    }
}
