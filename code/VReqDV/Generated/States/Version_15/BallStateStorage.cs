// GENERATED FILE — DO NOT EDIT
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Version_15
{
    public static class BallStateStorage
    {
        private static Dictionary<GameObject, BallStateEnum> stateTable = new();

        public static event Action<GameObject, BallStateEnum> OnStateChanged;

        public static void Register(GameObject obj, BallStateEnum initialState)
        {
            if (!stateTable.ContainsKey(obj))
                stateTable.Add(obj, initialState);
        }

        public static BallStateEnum Get(GameObject obj) => stateTable[obj];

        public static bool IsReady(GameObject obj) => stateTable[obj] == BallStateEnum.Ready;
        public static bool IsRolling(GameObject obj) => stateTable[obj] == BallStateEnum.Rolling;

        public static void SetReady(GameObject obj) => SetState(obj, BallStateEnum.Ready);
        public static void SetRolling(GameObject obj) => SetState(obj, BallStateEnum.Rolling);

        private static void SetState(GameObject obj, BallStateEnum newState)
        {
            if (stateTable[obj] != newState)
            {
                stateTable[obj] = newState;
                OnStateChanged?.Invoke(obj, newState);
            }
        }
    }
}
