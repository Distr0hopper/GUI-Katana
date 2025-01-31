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

    [SerializeField] private PointCloud2CustomVisualizerSettings pointCloud2DefaultVisualizerSettings;
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
        var ModeController = FindObjectOfType<ModeController>();
        var VisualizationController = FindObjectOfType<PointCloud2VizController>();

        // Find the views
        var connectionLabel = FindObjectOfType<ConnectionLabel>();
        var switchView = FindObjectOfType<SwitchView>();
        var velocityLabel = FindObjectOfType<VelocityLabel>();
        var emergencyStop = FindObjectOfType<EmergencyStop>();
        var driveStatus = FindObjectOfType<DriveStatus>();
        var settingsPanel = FindObjectOfType<SettingsPanel>();
        var editLimitsPanel = FindObjectOfType<EditLimitsPanel>();

        // Find the publishers
        var outgoingMessageController = FindObjectOfType<OutgoingMessageController>();
        
        // Velodyone 
        //var velodyoneScript = FindObjectOfType<PointCloud2VisualizerSettings>();
    

        // Ensure the components exist
        if (connectionLabel == null)
        {
            Debug.LogError("ConnectionLabel not found in the scene.");
        }

        if (connectionController == null)
        {
            Debug.LogError("RobotConnectionManager not found in the scene.");
        }

        if (cameraStreamController == null)
        {
            Debug.LogError("CameraStreamController not found in the scene.");
        }

        if (switchView == null)
        {
            Debug.LogError("SwitchView not found in the scene.");
        } if (velocityController == null)
        {
            Debug.LogError("VelocityController not found in the scene.");
        } if (velocityLabel == null)
        {
            Debug.LogError("VelocityLabel not found in the scene.");
        } if (emergencyStop == null)
        {
            Debug.LogError("EmergencyStop not found in the scene.");
        } if (driveStatus == null)
        {
            Debug.LogError("DriveStatus not found in the scene.");
        } if (incomingMessageController == null)
        {
            Debug.LogError("IncomingMessageController not found in the scene.");;
        } if (outgoingMessageController == null)
        {
            Debug.LogError("OutgoingMessageController not found in the scene.");
        } if (ModeController == null)
        {
            Debug.LogError("StateController not found in the scene.");
        } if (settingsPanel == null)
        {
            Debug.LogError("SettingsPanel not found in the scene.");
        } if (editLimitsPanel == null)
        {
            Debug.LogError("EditLimitsPanel not found in the scene.");
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
        
        // Give the message handler permission about the connection and the controllers that manage the connection
        incomingMessageController.SetConnectionController(connectionController); // Given controller the model
        incomingMessageController.SetVelocityController(velocityController); // Given controller the model
        incomingMessageController.SetCameraStreamController(cameraStreamController); // Given controller the model
        incomingMessageController.SetStateController(ModeController); // Given controller the model

        incomingMessageController.InitilaizeSubscribers(); // Initialize the message handler
        
        ////// INITIALIZING PUBLISHERS ////// 
               
        // Give the ebreak publisher permission about the robot model and the connection controller
        outgoingMessageController.SetRobotModel(robotModel); // Given publisher the model
        outgoingMessageController.SetConnectionController(connectionController); // Given publisher the controller

        outgoingMessageController.RegisterPublisher();

        // Give the velocity controller permission about the robot model
        velocityController.SetRobotModel(robotModel); // Given controller the model
        velocityLabel.SetRobotModel(robotModel); // Given UI the model

        // Give the emergency stop controller permission about the robot model
        emergencyStop.SetRobotModel(robotModel); // Given UI the model

        ModeController.SetRobotModel(robotModel); // Given controller the model

        // Give the drive status controller permission about the robot model
        driveStatus.SetRobotModel(robotModel); // Given UI the model
       
        // Give the settings panel permission about the connection state
        settingsPanel.SetConnectionController(connectionController); // Given UI the model

        // Give the edit limits panel permission about the robot model
        editLimitsPanel.SetRobotModel(robotModel); // Given UI the model

        incomingMessageController.SetVizController(VisualizationController);
        
    }
}
