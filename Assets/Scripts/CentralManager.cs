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

    /**
     * Dependency Injection
     * CentralManager is responsible for initializing the connection to the robot, 
     * and injecting the connection state into the controllers and views that need it.
     */
    void Start()
    {
        // Find the controllers
        var connectionController = FindObjectOfType<ConnectionController>();
        var cameraStreamController = FindObjectOfType<CameraStreamController>();
        var incomingMessageController = FindObjectOfType<IncomingMessageController>();
        var velocityController = FindObjectOfType<VelocityController>();

        // Find the views
        var connectionLabel = FindObjectOfType<ConnectionLabel>();
        var speedSlider = FindObjectOfType<SpeedSlider>();
        var switchView = FindObjectOfType<SwitchView>();
        var velocityLabel = FindObjectOfType<VelocityLabel>();

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

        // Give the message handler permission about the connection and the controllers that manage the connection
        incomingMessageController.SetConnectionController(connectionController); // Given controller the model
        incomingMessageController.SetVelocityController(velocityController); // Given controller the model
        incomingMessageController.SetCameraStreamController(cameraStreamController); // Given controller the model

        incomingMessageController.InitilaizeSubscribers(); // Initialize the message handler

        // Give the velocity controller permission about the robot model
        velocityController.SetRobotModel(robotModel); // Given controller the model
        velocityLabel.SetRobotModel(robotModel); // Given UI the model
    }
}
