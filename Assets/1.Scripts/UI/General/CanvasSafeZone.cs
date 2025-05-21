using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSafeZone : MonoBehaviour
{
    [SerializeField] private RectTransform SafeRect;

    private void Awake()
    {
        ApplySafeArea();
    }

    void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        // Normalize to 0–1 range (based on screen size)
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        SafeRect.anchorMin = anchorMin;
        SafeRect.anchorMax = anchorMax;
    }
}
