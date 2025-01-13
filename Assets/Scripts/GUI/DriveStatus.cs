using UnityEngine;
using UnityEngine.UIElements;

public class DriveStatus : MonoBehaviour
{
    [SerializeField] private UIDocument UIDocument;

    #region UI Elements
    private Label autoModeLabel;
    private VisualElement manualActiveIndicator;
    private VisualElement manualInactiveIndicator;
    #endregion

    private Color passiveRed = new Color(255f / 255f, 63f / 255f, 52f / 255f, 0.3f);
    private Color activeRed = new Color(255f / 255f, 63f / 255f, 52f / 255f, 1f);
    private Color passiveGreen = new Color(11f / 255f, 232f / 255f, 129f / 255f, 0.3f);
    private Color activeGreen = new Color(11f / 255f, 232f / 255f, 129f / 255f, 1f);

    private RobotModel robotModel;

    public void SetRobotModel(RobotModel model)
    {
        robotModel = model;

        // Subscribe to updates
        robotModel.OnManualModeChanged += UpdateManualModeIndicators;
        robotModel.OnAutoModeChanged += UpdateAutoModeLabel;

        // Initialize the UI with the current state
        UpdateManualModeIndicators(robotModel.ManualModeActive);
        UpdateAutoModeLabel(robotModel.CurrentAutoMode);
    }

    void Start()
    {
        var root = UIDocument.rootVisualElement;
        autoModeLabel = root.Q<Label>("ActiveMode");
        manualActiveIndicator = root.Q<VisualElement>("manualActive");
        manualInactiveIndicator = root.Q<VisualElement>("manualPassive");

        if (autoModeLabel == null) Debug.LogError("ActiveMode not found in the UI.");
        if (manualActiveIndicator == null) Debug.LogError("ManualActiveIndicator not found in the UI.");
        if (manualInactiveIndicator == null) Debug.LogError("ManualInactiveIndicator not found in the UI.");
    }

    /// <summary>
    /// Updates the manual mode indicator lights in the UI.
    /// </summary>
    private void UpdateManualModeIndicators(bool isManualActive)
    {
        if (manualActiveIndicator != null && manualInactiveIndicator != null)
        {
            if (isManualActive)
            {
                manualActiveIndicator.style.backgroundColor = activeGreen;
                manualInactiveIndicator.style.backgroundColor = passiveRed;
            }
            else
            {
                manualActiveIndicator.style.backgroundColor = passiveGreen;
                manualInactiveIndicator.style.backgroundColor = activeRed;
            }
        }
    }

    /// <summary>
    /// Updates the auto mode label in the UI.
    /// </summary>
    private void UpdateAutoModeLabel(RobotModel.AutoModes autoMode)
    {
        if (autoModeLabel != null)
        {
            autoModeLabel.text = autoMode switch
            {
                RobotModel.AutoModes.Explore => "Exploring",
                RobotModel.AutoModes.Docking => "Docking",
                RobotModel.AutoModes.Return => "Returning",
                _ => "None"
            };
        }
    }

    void OnDestroy()
    {
        if (robotModel != null)
        {
            robotModel.OnManualModeChanged -= UpdateManualModeIndicators;
            robotModel.OnAutoModeChanged -= UpdateAutoModeLabel;
        }
    }
}
