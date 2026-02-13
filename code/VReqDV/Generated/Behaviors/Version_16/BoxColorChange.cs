// GENERATED FILE — DO NOT EDIT
using UnityEngine;

namespace Version_16
{
    public class BoxColorChange : MonoBehaviour
    {
        void OnEnable()
        {
            BoxStateStorage.OnStateChanged += Handle;
        }

        void OnDisable()
        {
            BoxStateStorage.OnStateChanged -= Handle;
        }

        void Handle(GameObject obj, BoxStateEnum newState)
        {
            if (BoxStateStorage.Get(GameObject.Find("Box")) == BoxStateEnum.Active)
            {
                UserAlgorithms.ChangeBoxColor();
            }
        }
    }
}
