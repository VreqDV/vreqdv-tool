// GENERATED FILE — DO NOT EDIT
using UnityEngine;

namespace Version_15
{
    public class BallClickToRoll : MonoBehaviour
    {
        void Update()
        {
            if ((BallStateStorage.Get(GameObject.Find("Ball")) == BallStateEnum.Ready && UserAlgorithms.IsBallClicked()))
            {
                UserAlgorithms.RollBall();
            }
        }
    }
}
