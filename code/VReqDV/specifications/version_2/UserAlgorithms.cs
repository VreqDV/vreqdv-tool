using UnityEngine;
using VReqDV;

namespace Version_2
{
    public static class UserAlgorithms
    {
        public static bool IsBoxClicked()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    var obj = GameObject.Find("Box");
                    if (obj != null && hit.collider.gameObject == obj)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void SetBoxActive()
        {
            GameObject obj = GameObject.Find("Box");
            if (obj != null)
            {
                Debug.Log($"[UserAlgorithms] Activating Box: {obj.name}");
                StateAccessor.SetState("Box", "Active", obj, "Version_2");
            }
        }

        public static void ChangeBoxColor()
        {
            GameObject obj = GameObject.Find("Box");
            if (obj != null)
            {
                Debug.Log($"[UserAlgorithms] Changing Box Color: {obj.name}");
                var renderer = obj.GetComponent<Renderer>();
                if (renderer != null) renderer.material.color = Color.Lerp(Color.red, Color.yellow, 0.5f); // Orange-ish
            }
        }
    }
}