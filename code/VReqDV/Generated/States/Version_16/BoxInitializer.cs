// GENERATED FILE — DO NOT EDIT
using UnityEngine;

namespace Version_16
{
    public class BoxInitializer : MonoBehaviour
    {
        public BoxStateEnum initialState = BoxStateEnum.Idle;

        void Start()
        {
            BoxStateStorage.Register(gameObject, initialState);
        }
    }
}
