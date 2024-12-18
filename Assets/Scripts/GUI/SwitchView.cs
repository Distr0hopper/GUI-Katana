using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SwitchView : MonoBehaviour
{
    [SerializeField] public UIDocument UIDocument;

    #region UIElements
    private VisualElement LaserView;
    private VisualElement CameraView;
    private VisualElement NoCameraMessage;

    private Button LaserButton;
    private Button CameraButton;

    #endregion

    #region Colors
    private Color activeButtonColor = new Color(0.5019608f, 0.5568628f, 0.6078432f, 0.3019608f);
    private Color inactiveButtonColor = new Color(0.5019608f, 0.5568628f, 0.6078432f, 0.0f);

    #endregion

    private CameraStateModel cameraStateModel;


    void Awake(){
        // Get the root of the UI
        var root = UIDocument.rootVisualElement;

        // Get the elements from the UI
        LaserView = root.Q<VisualElement>("Laserscan");
        CameraView = root.Q<VisualElement>("CameraStream");
        NoCameraMessage = root.Q<VisualElement>("CameraNotAvailable");
        LaserButton = root.Q<Button>("LaserscanButton");
        CameraButton = root.Q<Button>("CameraButton");
        var button = root.Q<Button>("MyButton");

        // Add onClick listeners to the buttons
        LaserButton.clicked += () => SwitchToLaserView();
        CameraButton.clicked += () => SwitchToCameraView();
    }

    void Start()
    {
        // Initialize the view
        SwitchToLaserView();
    }

    private void SwitchToLaserView()
    {
        LaserView.style.display = DisplayStyle.Flex;
        CameraView.style.display = DisplayStyle.None;
        NoCameraMessage.style.display = DisplayStyle.None;

        LaserButton.style.backgroundColor = activeButtonColor;
        CameraButton.style.backgroundColor = inactiveButtonColor;
    }

    private void SwitchToCameraView()
    {
        LaserView.style.display = DisplayStyle.None;
        CameraView.style.display = DisplayStyle.Flex;

        LaserButton.style.backgroundColor = inactiveButtonColor;
        CameraButton.style.backgroundColor = activeButtonColor;

        // Check the CameraStateModel to see if the camera data is available, if not, show the error message
        UpdateCameraErrorMessage();
    }

    private void UpdateCameraErrorMessage()
    {
        if (cameraStateModel == null){
            Debug.LogError("CameraStateModel is null in UpdateCameraErrorMessage.");
            return;
        } 
        // Show NoCameraMessage only if camera data is unavailable
        if (!cameraStateModel.IsDataAvailable && CameraView.style.display == DisplayStyle.Flex)
        {
            NoCameraMessage.style.display = DisplayStyle.Flex;
        }
        else
        {
            NoCameraMessage.style.display = DisplayStyle.None;
        }
    }

    public void SetCameraStateModel(CameraStateModel cameraStateModel)
    {
        if (cameraStateModel == null)
        {
            Debug.LogError("CameraStateModel is null in SetCameraStateModel.");
            return;
        }
        this.cameraStateModel = cameraStateModel;
        cameraStateModel.OnCameraStateChanged += UpdateCameraErrorMessage;
        UpdateCameraErrorMessage();
    }




    void OnDestroy()
    {
        if (cameraStateModel != null)
        {
            cameraStateModel.OnCameraStateChanged -= UpdateCameraErrorMessage;
        }
    }

}
