// GENERATED FILE — DO NOT EDIT
using UnityEngine;

namespace Version_15
{
    public class PlaneInitializer : MonoBehaviour
    {
        public PlaneStateEnum initialState = PlaneStateEnum.Ready;

        void Awake()
        {
            PlaneStateStorage.Register(gameObject, initialState);
        }
    }
}
