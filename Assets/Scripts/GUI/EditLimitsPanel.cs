using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class EditLimitsPanel : MonoBehaviour
{
    [SerializeField] private UIDocument UIDocument;

    #region UIElements
    private Button editSteering;
    private Button editVelocity;
    private Button closeEditButton;
    private FloatField velocityInput;
    private FloatField steeringInput;
    private VisualElement editPanel;
    private Button setLimits;
    private VisualElement invalidLimits;
    private FloatField wallDistanceInput;
    private Button setWallDistance;
    #endregion

    #region Colors
    private Color clickedColor = new Color(11 / 255f, 232 / 255f, 129 / 255f, 1.0f);

    //rgba(72, 84, 96,1.0)
    private Color defaultColor = new Color(72 / 255f, 84 / 255f, 96 / 255f, 1.0f);
    #endregion

    private float velocityLimit = 1.0f;
    private float steeringLimit = 0.2f;
    private float wallDistance = 0.5f;

    private RobotModel robotModel;

    public void SetRobotModel(RobotModel robotModel)
    {
        this.robotModel = robotModel;
    }

    void Start()
    {
        var root = UIDocument.rootVisualElement;

        // Get UI elements
        editSteering = root.Q<Button>("EditSteering");
        editVelocity = root.Q<Button>("EditVelocity");
        closeEditButton = root.Q<Button>("CloseEditButton");
        editPanel = root.Q<VisualElement>("EditPanel");
        velocityInput = root.Q<FloatField>("VelocityInput");
        steeringInput = root.Q<FloatField>("SteeringInput");
        setLimits = root.Q<Button>("SetLimits");
        invalidLimits = root.Q<VisualElement>("LimitsInvalid");
        wallDistanceInput = root.Q<FloatField>("InputWall");
        setWallDistance = root.Q<Button>("SetDistanceButton");

        // Attach event handlers
        editSteering.clicked += OnEditButtonClicked;
        editVelocity.clicked += OnEditButtonClicked;
        closeEditButton.clicked += OnCloseEditButtonClicked;
        setLimits.clicked += OnSetLimitsClicked;
        setWallDistance.clicked += () =>
        {
            OnSetWallDistanceClicked();
            StartCoroutine(AnimateButton(setWallDistance));
        };
    }

    private void OnSetWallDistanceClicked()
    {
        wallDistance = wallDistanceInput.value;
        robotModel.UpdateWallDistance(wallDistance);
    }

    private void OnEditButtonClicked()
    {
        editPanel.AddToClassList("bottompanel-up");
    }

    private void OnCloseEditButtonClicked()
    {
        editPanel.RemoveFromClassList("bottompanel-up");
    }

    private void OnSetLimitsClicked()
    {
        velocityLimit = velocityInput.value;
        steeringLimit = steeringInput.value;
        editPanel.RemoveFromClassList("bottompanel-up");
        robotModel.UpdateMaxVelocity(velocityLimit);
        robotModel.UpdateMaxSteering(steeringLimit);
    }

    private IEnumerator AnimateButton(Button button)
    {
        // Set the clicked color
        button.style.backgroundColor = clickedColor;

        // Wait for 1 second
        yield return new WaitForSeconds(0.2f);

        // Revert to the original color
        button.style.backgroundColor = defaultColor;
    }
}
