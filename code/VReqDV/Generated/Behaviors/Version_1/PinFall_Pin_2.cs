// GENERATED FILE — DO NOT EDIT
using UnityEngine;

namespace Version_1
{
    public class PinFall_Pin_2 : MonoBehaviour
    {
        void Update()
        {
            if ((Pin_2StateStorage.Get(GameObject.Find("Pin_2")) == Pin_2StateEnum.Standing && UserAlgorithms.IsPinFallen(GameObject.Find("Pin_2"))))
            {
                UserAlgorithms.SetPinFallen(GameObject.Find("Pin_2"));
            }
        }
    }
}
