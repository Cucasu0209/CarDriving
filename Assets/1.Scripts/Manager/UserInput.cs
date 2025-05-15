using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.EventSystems;

public class UserInput : MonoBehaviour
{
    public static UserInput Instance;
    public Action<bool> OnUserMouse;
    bool IsTouchUI = false;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        IsTouchUI = EventSystem.current.IsPointerOverGameObject();

#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                IsTouchUI = true;
            }
        }
#endif


        OnUserMouse?.Invoke(Input.GetMouseButton(0) && !IsTouchUI);
    }
}
