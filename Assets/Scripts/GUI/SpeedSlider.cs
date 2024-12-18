using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpeedSlider : MonoBehaviour
{
    [SerializeField] public UIDocument UIDocument;
    private RobotModel robotModel;

    #region UIElements


    private SliderInt sliderInt;
    private Label speedValue;

    #endregion


    void Awake(){
        // Get the root of the UI
        var root = UIDocument.rootVisualElement;

        // Get the elements from the UI
        sliderInt = root.Q<SliderInt>("SpeedSlider");
        speedValue = root.Q<Label>("SpeedValue");

        if (sliderInt != null)
        {
            // Register a callback for value changes
            sliderInt.RegisterValueChangedCallback(OnSliderValueChanged);
        }
        else
        {
            Debug.LogError("SliderInt 'SpeedSlider' not found in the UI.");
        }
    }

    public void SetRobotModel(RobotModel robotModel)
    {
        // Set the initial value of the slider
        this.robotModel = robotModel;
    }

    
    // Callback for slider value changes
    private void OnSliderValueChanged(ChangeEvent<int> evt)
    {
        // Update the speed value label
        speedValue.text = evt.newValue.ToString();

        // Change the robot speed in the model
        robotModel.Speed = evt.newValue;
    }
}
