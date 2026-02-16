// GENERATED FILE — DO NOT EDIT
using UnityEngine;

namespace Version_1
{
    public class BallInitializer : MonoBehaviour
    {
        public BallStateEnum initialState = BallStateEnum.Ready;

        void Awake()
        {
            BallStateStorage.Register(gameObject, initialState);
        }
    }
}
