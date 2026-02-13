// GENERATED FILE — DO NOT EDIT
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Version_15
{
    public static class PlaneStateStorage
    {
        private static Dictionary<GameObject, PlaneStateEnum> stateTable = new();

        public static event Action<GameObject, PlaneStateEnum> OnStateChanged;

        public static void Register(GameObject obj, PlaneStateEnum initialState)
        {
            if (!stateTable.ContainsKey(obj))
                stateTable.Add(obj, initialState);
        }

        public static PlaneStateEnum Get(GameObject obj) => stateTable[obj];

        public static bool IsReady(GameObject obj) => stateTable[obj] == PlaneStateEnum.Ready;

        public static void SetReady(GameObject obj) => SetState(obj, PlaneStateEnum.Ready);

        private static void SetState(GameObject obj, PlaneStateEnum newState)
        {
            if (stateTable[obj] != newState)
            {
                stateTable[obj] = newState;
                OnStateChanged?.Invoke(obj, newState);
            }
        }
    }
}
