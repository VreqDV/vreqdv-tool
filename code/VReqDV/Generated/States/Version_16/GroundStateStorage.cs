// GENERATED FILE — DO NOT EDIT
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Version_16
{
    public static class GroundStateStorage
    {
        private static Dictionary<GameObject, GroundStateEnum> stateTable = new();

        public static event Action<GameObject, GroundStateEnum> OnStateChanged;

        public static void Register(GameObject obj, GroundStateEnum initialState)
        {
            if (!stateTable.ContainsKey(obj))
                stateTable.Add(obj, initialState);
        }

        public static GroundStateEnum Get(GameObject obj) => stateTable[obj];

        public static bool IsStatic(GameObject obj) => stateTable[obj] == GroundStateEnum.Static;

        public static void SetStatic(GameObject obj) => SetState(obj, GroundStateEnum.Static);

        private static void SetState(GameObject obj, GroundStateEnum newState)
        {
            if (stateTable[obj] != newState)
            {
                stateTable[obj] = newState;
                OnStateChanged?.Invoke(obj, newState);
            }
        }
    }
}
