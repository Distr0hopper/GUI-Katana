using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralManager : MonoBehaviour
{

    [SerializeField] private string robotIP = "213.65.204.181";
    #region Models
    private ConnectionState connectionState;
    private CameraStateModel cameraStateModel;
    private RobotModel robotModel;
    #endregion
    void Start()
    {
        // Find the controllers
        var connectionLabel = FindObjectOfType<ConnectionLabel>();
        var connectionController = FindObjectOfType<ConnectionController>();
        var cameraStreamController = FindObjectOfType<CameraStreamController>();
        var switchView = FindObjectOfType<SwitchView>();
        var speedSlider = FindObjectOfType<SpeedSlider>();

        // Find the publishers
        var speedPublisher = FindObjectOfType<SpeedPublisher>();
    

        // Ensure the components exist
        if (connectionLabel == null)
        {
            Debug.LogError("ConnectionLabel not found in the scene.");
            return;
        }

        if (connectionController == null)
        {
            Debug.LogError("RobotConnectionManager not found in the scene.");
            return;
        }

        if (cameraStreamController == null)
        {
            Debug.LogError("CameraStreamController not found in the scene.");
            return;
        }

        if (switchView == null)
        {
            Debug.LogError("SwitchView not found in the scene.");
            return;
        } if (speedSlider == null)
        {
            Debug.LogError("speedSlider not found in the scene.");
            return;
        } if (speedPublisher == null)
        {
            Debug.LogError("SpeedPublisher not found in the scene.");
            return;
        }

        // Initialize the ConnectionState
        connectionState = new ConnectionState(robotIP);
        // Initialize the connection to the robot
        connectionController.Initialize(connectionState); 

        // Inject the connection state into the controllers
        connectionLabel.SetConnectionState(connectionState); // Given UI the model 
        
        // Initialize the Camera Model 
        cameraStateModel = new CameraStateModel();
        // Inject the camera state model into the controllers
        cameraStreamController.SetCameraStateModel(cameraStateModel); // Given controller the model
        switchView.SetCameraStateModel(cameraStateModel); // Given UI the model

        // Initialize the RobotModel
        robotModel = new RobotModel();
        // Inject the robot model into the controllers and publishers
        speedSlider.SetRobotModel(robotModel); // Given controller the model
        speedPublisher.SetRobotModel(robotModel); // Given publisher the model
        speedPublisher.SetConnectionController(connectionController); // Given publisher the controller
    }
}
