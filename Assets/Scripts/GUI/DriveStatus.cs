using UnityEngine;
using UnityEngine.UIElements;

public class DriveStatus : MonoBehaviour
{
    [SerializeField] private UIDocument UIDocument;

    #region UI Elements
    private Label driveStatusLabel;
    #endregion

    private RobotModel robotModel;

    public void SetRobotModel(RobotModel model)
    {
        robotModel = model;

        // Subscribe to drive mode changes
        robotModel.OnDriveModeChanged += UpdateDriveStatusLabel;

        // Initialize the label with the current mode
        UpdateDriveStatusLabel(robotModel.CurrentDriveMode);
    }

    void Start()
    {
        var root = UIDocument.rootVisualElement;
        driveStatusLabel = root.Q<Label>("DriveLabel");

        if (driveStatusLabel == null)
        {
            Debug.LogError("DriveLabel not found in the UI.");
        }
    }

    private void UpdateDriveStatusLabel(RobotModel.DriveMode driveMode)
    {
        if (driveStatusLabel != null)
        {
            driveStatusLabel.text = driveMode switch
            {
                RobotModel.DriveMode.Manual => "Manual Override",
                RobotModel.DriveMode.EmergencyStop => "Emergency Break",
                RobotModel.DriveMode.Auto => "Autonomous Drive",
                _ => "Unknown"
            };

            Debug.Log($"DriveStatus updated to: {driveStatusLabel.text}");
        }
    }

    void OnDestroy()
    {
        if (robotModel != null)
        {
            robotModel.OnDriveModeChanged -= UpdateDriveStatusLabel;
        }
    }
}
