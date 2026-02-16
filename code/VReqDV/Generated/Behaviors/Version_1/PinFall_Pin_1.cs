// GENERATED FILE — DO NOT EDIT
using UnityEngine;

namespace Version_1
{
    public class PinFall_Pin_1 : MonoBehaviour
    {
        void Update()
        {
            if ((Pin_1StateStorage.Get(GameObject.Find("Pin_1")) == Pin_1StateEnum.Standing && UserAlgorithms.IsPinFallen(GameObject.Find("Pin_1"))))
            {
                UserAlgorithms.SetPinFallen(GameObject.Find("Pin_1"));
            }
        }
    }
}
