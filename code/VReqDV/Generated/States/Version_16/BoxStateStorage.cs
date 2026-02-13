// GENERATED FILE — DO NOT EDIT
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Version_16
{
    public static class BoxStateStorage
    {
        private static Dictionary<GameObject, BoxStateEnum> stateTable = new();

        public static event Action<GameObject, BoxStateEnum> OnStateChanged;

        public static void Register(GameObject obj, BoxStateEnum initialState)
        {
            if (!stateTable.ContainsKey(obj))
                stateTable.Add(obj, initialState);
        }

        public static BoxStateEnum Get(GameObject obj) => stateTable[obj];

        public static bool IsIdle(GameObject obj) => stateTable[obj] == BoxStateEnum.Idle;
        public static bool IsActive(GameObject obj) => stateTable[obj] == BoxStateEnum.Active;

        public static void SetIdle(GameObject obj) => SetState(obj, BoxStateEnum.Idle);
        public static void SetActive(GameObject obj) => SetState(obj, BoxStateEnum.Active);

        private static void SetState(GameObject obj, BoxStateEnum newState)
        {
            if (stateTable[obj] != newState)
            {
                stateTable[obj] = newState;
                OnStateChanged?.Invoke(obj, newState);
            }
        }
    }
}
