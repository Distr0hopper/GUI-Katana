using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpeedControll : MonoBehaviour
{
    [SerializeField] public UIDocument UIDocument;

    #region UIElements
    private SliderInt sliderInt;

    #endregion


    void Awake(){
        // Get the root of the UI
        var root = UIDocument.rootVisualElement;

        // Get the elements from the UI
        sliderInt = root.Q<SliderInt>("SpeedSlider");

        if (sliderInt != null)
        {
            // Register a callback for value changes
            sliderInt.RegisterValueChangedCallback(OnSliderValueChanged);

            // Print the initial value
            Debug.Log($"Initial Slider Value: {sliderInt.value}");
        }
        else
        {
            Debug.LogError("SliderInt 'SpeedSlider' not found in the UI.");
        }
    }

    
    // Callback for slider value changes
    private void OnSliderValueChanged(ChangeEvent<int> evt)
    {
        Debug.Log($"Slider Value Changed: {evt.newValue}");
    }
}
