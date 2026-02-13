// GENERATED FILE — DO NOT EDIT
using UnityEngine;

namespace Version_16
{
    public class BoxActivate : MonoBehaviour
    {
        void Update()
        {
            if ((BoxStateStorage.Get(GameObject.Find("Box")) == BoxStateEnum.Idle && UserAlgorithms.IsBoxClicked()))
            {
                UserAlgorithms.SetBoxActive();
            }
        }
    }
}
