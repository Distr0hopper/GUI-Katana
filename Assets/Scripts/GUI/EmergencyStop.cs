using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EmergencyStop : MonoBehaviour
{

    [SerializeField] private UIDocument UIDocument;

    #region UI Elements 
    private Button emergencyStopButton;
    #endregion


    #region Colors
    //rgba(255, 63, 52,1.0)
    private Color emergencyStopInactive = new Color(255/255f, 63/255f, 52/255f, 0.6f);
    private Color emergencyStopActive = new Color(255/255f, 63/255f, 52/255f, 1.0f);   

    //rgba(128, 142, 155,1.0)
    private Color emergencyStopInactiveBorder = new Color(128/255f, 142/255f, 155/255f, 1.0f);

    //rgba(255, 211, 42,1.0)
    private Color emergencyStopActiveBorder = new Color(255/255f, 211/255f, 42/255f, 1.0f);

    #endregion

    private RobotModel robotModel;

    public void SetRobotModel(RobotModel robotModel)
    {
        this.robotModel = robotModel;
    }

    void Start()
    {
        var root = UIDocument.rootVisualElement;
        emergencyStopButton = root.Q<Button>("EmergencyStopButton");

        if (emergencyStopButton != null)
        {
            // Set initial apperance 
            UpdateButtonAppearance(robotModel != null && robotModel.EmergencyStopActive);

            emergencyStopButton.clicked += OnEmergencyStopButtonClicked;
        }
    }

private void OnEmergencyStopButtonClicked()
{
    if (robotModel == null)
    {
        Debug.LogWarning("RobotModel is null. Cannot update emergency stop state.");
        return;
    }

    // Toggle the emergency stop state
    robotModel.EmergencyStopActive = !robotModel.EmergencyStopActive;

    // Update the button appearance
    UpdateButtonAppearance(robotModel.EmergencyStopActive);

    Debug.Log($"Emergency Stop is now: {(robotModel.EmergencyStopActive ? "Active" : "Inactive")}");
}


    private void UpdateButtonAppearance(bool isActive){
        if (emergencyStopButton == null) return;

        // Update button background and borders
        emergencyStopButton.style.backgroundColor = isActive ? emergencyStopActive : emergencyStopInactive;
        emergencyStopButton.style.borderBottomColor = isActive ? emergencyStopActiveBorder : emergencyStopInactiveBorder;
        emergencyStopButton.style.borderLeftColor = isActive ? emergencyStopActiveBorder : emergencyStopInactiveBorder;
        emergencyStopButton.style.borderRightColor = isActive ? emergencyStopActiveBorder : emergencyStopInactiveBorder;
        emergencyStopButton.style.borderTopColor = isActive ? emergencyStopActiveBorder : emergencyStopInactiveBorder;
    }
}
