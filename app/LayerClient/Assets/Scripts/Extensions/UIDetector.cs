using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public static class UIDetector
{
    public static bool IsPointOverUIGB(this Vector2 pos)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

        PointerEventData eventPosition = new(EventSystem.current);
        eventPosition.position = new Vector2(pos.x, pos.y);

        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventPosition, results);

        return results.Count > 0;
    }

    public static Vector3 ConvertUIPosToScreenWorldPos(this GameObject targetUIElement, Camera arCamera)
    {
        Transform targetUITransform = targetUIElement.transform;
        Vector2 anchorPos = targetUIElement.GetComponent<RectTransform>().anchoredPosition;
        Vector2 screenPosition = RectTransformUtility.PixelAdjustPoint(anchorPos, targetUITransform, targetUIElement.GetComponentInParent<Canvas>());
        return arCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 0f));
    }
}


