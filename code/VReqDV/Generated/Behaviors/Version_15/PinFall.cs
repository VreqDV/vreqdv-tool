// GENERATED FILE — DO NOT EDIT
using UnityEngine;

namespace Version_15
{
    public class PinFall : MonoBehaviour
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
