// GENERATED FILE — DO NOT EDIT
using UnityEngine;

namespace Version_1
{
    public class PinReset_Pin_3 : MonoBehaviour
    {
        void Update()
        {
            if ((Pin_3StateStorage.Get(GameObject.Find("Pin_3")) == Pin_3StateEnum.Fallen && Pin_2StateStorage.Get(GameObject.Find("Pin_2")) == Pin_2StateEnum.Fallen && Pin_3StateStorage.Get(GameObject.Find("Pin_3")) == Pin_3StateEnum.Fallen && BallStateStorage.Get(GameObject.Find("Ball")) == BallStateEnum.Ready))
            {
                UserAlgorithms.ResetBallAndPins();
            }
        }
    }
}
