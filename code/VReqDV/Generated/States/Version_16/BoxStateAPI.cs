// GENERATED FILE — DO NOT EDIT
using UnityEngine;

namespace Version_16
{
    public static class BoxStateAPI
    {
        public static bool Idle(GameObject obj) => BoxStateStorage.IsIdle(obj);
        public static bool Active(GameObject obj) => BoxStateStorage.IsActive(obj);

        public static void SetIdle(GameObject obj) => BoxStateStorage.SetIdle(obj);
        public static void SetActive(GameObject obj) => BoxStateStorage.SetActive(obj);
    }
}
