using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FurnitureButtonController : MonoBehaviour
{
    public UnityEvent _event;

    private void OnMouseDown()
    {
#if !UNITY_EDITOR
        if (IsPointerOverUIObject())
        {
            return;
        }
        _event?.Invoke();
#endif

#if UNITY_EDITOR
        if (IsPointerOverUIObject())
        {
            return;
        }
        _event?.Invoke();
#endif
    }
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        foreach (RaycastResult r in results)
        {
            if (r.gameObject.GetComponent<RectTransform>() != null)
                return true;
        }

        return false;
    }

    public static bool IsPointerOverUIObjectDontEditor()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        foreach (RaycastResult r in results)
        {
            if (r.gameObject.GetComponent<RectTransform>() != null)
                return true;
        }

        return false;
    }
}
public enum FurnitureType
{
    Gamearcade = 0,
    Car = 1,
    BassPad = 2,
    Stadium = 3,
    Ring = 4,
    Locker = 5,
}
