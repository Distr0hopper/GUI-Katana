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
    private Color emergencyStopInactive = new Color(255 / 255f, 63 / 255f, 52 / 255f, 0.6f);  // Inactive red
    private Color emergencyStopActive = new Color(255 / 255f, 63 / 255f, 52 / 255f, 1.0f);   // Active red
    private Color emergencyStopInactiveBorder = new Color(128 / 255f, 142 / 255f, 155 / 255f, 1.0f);  // Gray border
    private Color emergencyStopActiveBorder = new Color(255 / 255f, 211 / 255f, 42 / 255f, 1.0f);     // Yellow border
    #endregion

    private RobotModel robotModel;

    public void SetRobotModel(RobotModel robotModel)
    {
        this.robotModel = robotModel;

        if (robotModel != null)
        {
            // Subscribe to emergency stop state changes
            robotModel.OnEmergencyStopChanged += UpdateButtonAppearance;

            // Initialize button appearance with the current state
            UpdateButtonAppearance(robotModel.EmergencyStopActive);
        }
    }

    void Start()
    {
        var root = UIDocument.rootVisualElement;
        emergencyStopButton = root.Q<Button>("EmergencyStopButton");

        if (emergencyStopButton != null)
        {
            // Set initial appearance based on the RobotModel's current state
            UpdateButtonAppearance(robotModel != null && robotModel.EmergencyStopActive);

            // Attach the click event handler
            emergencyStopButton.clicked += OnEmergencyStopButtonClicked;
        }
        else
        {
            Debug.LogError("EmergencyStopButton not found in the UI.");
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

        Debug.Log($"Emergency Stop is now: {(robotModel.EmergencyStopActive ? "Active" : "Inactive")}");
    }

    private void UpdateButtonAppearance(bool isActive)
    {
        if (emergencyStopButton == null) return;

        // Update button background and borders based on the state
        emergencyStopButton.style.backgroundColor = isActive ? emergencyStopActive : emergencyStopInactive;
        emergencyStopButton.style.borderBottomColor = isActive ? emergencyStopActiveBorder : emergencyStopInactiveBorder;
        emergencyStopButton.style.borderLeftColor = isActive ? emergencyStopActiveBorder : emergencyStopInactiveBorder;
        emergencyStopButton.style.borderRightColor = isActive ? emergencyStopActiveBorder : emergencyStopInactiveBorder;
        emergencyStopButton.style.borderTopColor = isActive ? emergencyStopActiveBorder : emergencyStopInactiveBorder;
    }

    private void OnDestroy()
    {
        if (robotModel != null)
        {
            // Unsubscribe from emergency stop state changes
            robotModel.OnEmergencyStopChanged -= UpdateButtonAppearance;
        }
    }
}
