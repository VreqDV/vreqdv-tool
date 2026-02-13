// GENERATED FILE — DO NOT EDIT
using UnityEngine;

namespace Version_16
{
    public class GroundInitializer : MonoBehaviour
    {
        public GroundStateEnum initialState = GroundStateEnum.Static;

        void Start()
        {
            GroundStateStorage.Register(gameObject, initialState);
        }
    }
}
