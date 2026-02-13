// GENERATED FILE — DO NOT EDIT
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Version_15
{
    public static class Pin_1StateStorage
    {
        private static Dictionary<GameObject, Pin_1StateEnum> stateTable = new();

        public static event Action<GameObject, Pin_1StateEnum> OnStateChanged;

        public static void Register(GameObject obj, Pin_1StateEnum initialState)
        {
            if (!stateTable.ContainsKey(obj))
                stateTable.Add(obj, initialState);
        }

        public static Pin_1StateEnum Get(GameObject obj) => stateTable[obj];

        public static bool IsStanding(GameObject obj) => stateTable[obj] == Pin_1StateEnum.Standing;
        public static bool IsFallen(GameObject obj) => stateTable[obj] == Pin_1StateEnum.Fallen;

        public static void SetStanding(GameObject obj) => SetState(obj, Pin_1StateEnum.Standing);
        public static void SetFallen(GameObject obj) => SetState(obj, Pin_1StateEnum.Fallen);

        private static void SetState(GameObject obj, Pin_1StateEnum newState)
        {
            if (stateTable[obj] != newState)
            {
                stateTable[obj] = newState;
                OnStateChanged?.Invoke(obj, newState);
            }
        }
    }
}
