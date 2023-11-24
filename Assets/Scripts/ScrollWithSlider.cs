using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollWithSlider : MonoBehaviour
{
    public ScrollRect Rect;
    public Slider ScrollSlider;

    private void OnEnable()
    {
        ScrollSlider.onValueChanged.AddListener(UpdateScrollPosition);
        Rect.onValueChanged.AddListener(UpdateSliderValue);
    }

    private void OnDisable()
    {
        // important! Don't forget to unsubscribe from events! 
        ScrollSlider.onValueChanged.RemoveListener(UpdateScrollPosition);
        Rect.onValueChanged.RemoveListener(UpdateSliderValue);
    }

    private void UpdateScrollPosition(float value)
    {
        // Here I flip the value in the code instead of trying to rotate the UI element itself since it's easier for me :P
        Rect.verticalNormalizedPosition = 1 - value;
    }

    private void UpdateSliderValue(Vector2 scrollPosition)
    {
        // Again, flippin the value for visual consistency
        ScrollSlider.SetValueWithoutNotify(1 - scrollPosition.y);
    }
}
