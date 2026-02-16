// GENERATED FILE — DO NOT EDIT
using UnityEngine;

namespace Version_1
{
    public class PinFall_Pin_3 : MonoBehaviour
    {
        void Update()
        {
            if ((Pin_3StateStorage.Get(GameObject.Find("Pin_3")) == Pin_3StateEnum.Standing && UserAlgorithms.IsPinFallen(GameObject.Find("Pin_3"))))
            {
                UserAlgorithms.SetPinFallen(GameObject.Find("Pin_3"));
            }
        }
    }
}
