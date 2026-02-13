// GENERATED FILE — DO NOT EDIT
using UnityEngine;

namespace Version_15
{
    public class Pin_3Initializer : MonoBehaviour
    {
        public Pin_3StateEnum initialState = Pin_3StateEnum.Standing;

        void Awake()
        {
            Pin_3StateStorage.Register(gameObject, initialState);
        }
    }
}
