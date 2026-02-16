// GENERATED FILE — DO NOT EDIT
using UnityEngine;

namespace Version_1
{
    public class BallClickToRoll_Ball : MonoBehaviour
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
