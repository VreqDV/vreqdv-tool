// GENERATED FILE — DO NOT EDIT
using UnityEngine;

namespace Version_15
{
    public class Pin_1Initializer : MonoBehaviour
    {
        public Pin_1StateEnum initialState = Pin_1StateEnum.Standing;

        void Awake()
        {
            Pin_1StateStorage.Register(gameObject, initialState);
        }
    }
}
