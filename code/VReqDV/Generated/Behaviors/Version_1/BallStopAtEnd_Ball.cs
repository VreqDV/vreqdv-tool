// GENERATED FILE — DO NOT EDIT
using UnityEngine;

namespace Version_1
{
    public class BallStopAtEnd_Ball : MonoBehaviour
    {
        void Update()
        {
            if ((BallStateStorage.Get(GameObject.Find("Ball")) == BallStateEnum.Rolling && UserAlgorithms.HasBallReachedEnd()))
            {
                UserAlgorithms.StopAndResetBall();
            }
        }
    }
}
