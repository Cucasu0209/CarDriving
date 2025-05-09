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
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        OnUserMouse?.Invoke(Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject());
    }
}
