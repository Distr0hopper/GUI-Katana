using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpeedSlider : MonoBehaviour
{
    [SerializeField] public UIDocument UIDocument;
    private RobotModel robotModel;

    #region UIElements


    private Slider slider;
    private Label speedValue;

    #endregion

    private float initialSliderValue;
    private float doubleClickThreshold = 0.3f;
    private float lastClickTime = 0;

    void Awake(){
        // Get the root of the UI
        var root = UIDocument.rootVisualElement;

        // Get the elements from the UI
        slider = root.Q<Slider>("SpeedSlider");
        speedValue = root.Q<Label>("SpeedValue");

        if (slider != null)
        {
            // Register a callback for value changes
            slider.RegisterValueChangedCallback(OnSliderValueChanged);
            // Register a callback for double clicks
            slider.RegisterCallback<ClickEvent>(OnSliderClicked);
        }
        else
        {
            Debug.LogError("SliderInt 'SpeedSlider' not found in the UI.");
        }

        initialSliderValue = slider.value;
    }

    public void SetRobotModel(RobotModel robotModel)
    {
        // Set the initial value of the slider
        this.robotModel = robotModel;
    }

    
    // Callback for slider value changes
    private void OnSliderValueChanged(ChangeEvent<float> evt)
    {
        // Update the speed value label
        speedValue.text = evt.newValue.ToString();

        // Change the robot speed in the model
        robotModel.Speed = evt.newValue;
    }

        // Callback for slider clicks
    private void OnSliderClicked(ClickEvent evt)
    {
        float currentTime = Time.time;

        // Check for double-click
        if (currentTime - lastClickTime <= doubleClickThreshold)
        {
            ResetSliderToInitialValue();
        }

        lastClickTime = currentTime; // Update the last click time
    }

    private void ResetSliderToInitialValue()
    {
        if (slider != null)
        {
            slider.value = initialSliderValue; // Reset slider value
        }

        if (robotModel != null)
        {
            robotModel.Speed = initialSliderValue; // Reset robot speed
        }

        Debug.Log($"Slider reset to initial value: {initialSliderValue}");
    }

}
