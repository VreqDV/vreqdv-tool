// GENERATED FILE — DO NOT EDIT
using UnityEngine;

namespace Version_15
{
    public static class BallStateAPI
    {
        public static bool Ready(GameObject obj) => BallStateStorage.IsReady(obj);
        public static bool Rolling(GameObject obj) => BallStateStorage.IsRolling(obj);

        public static void SetReady(GameObject obj) => BallStateStorage.SetReady(obj);
        public static void SetRolling(GameObject obj) => BallStateStorage.SetRolling(obj);
    }
}
