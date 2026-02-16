// GENERATED FILE — DO NOT EDIT
using UnityEngine;

namespace Version_1
{
    public class Pin_2Initializer : MonoBehaviour
    {
        public Pin_2StateEnum initialState = Pin_2StateEnum.Standing;

        void Awake()
        {
            Pin_2StateStorage.Register(gameObject, initialState);
        }
    }
}
