// GENERATED FILE — DO NOT EDIT
using UnityEngine;

namespace Version_15
{
    public static class Pin_2StateAPI
    {
        public static bool Standing(GameObject obj) => Pin_2StateStorage.IsStanding(obj);
        public static bool Fallen(GameObject obj) => Pin_2StateStorage.IsFallen(obj);

        public static void SetStanding(GameObject obj) => Pin_2StateStorage.SetStanding(obj);
        public static void SetFallen(GameObject obj) => Pin_2StateStorage.SetFallen(obj);
    }
}
