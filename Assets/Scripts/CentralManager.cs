using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralManager : MonoBehaviour
{
    #region Models
    private ConnectionState connectionState;
    private CameraStateModel cameraStateModel;
    #endregion
    void Start()
    {
        // Find the controllers
        var connectionLabel = FindObjectOfType<ConnectionLabel>();
        var connectionController = FindObjectOfType<ConnectionController>();
        var cameraStreamController = FindObjectOfType<CameraStreamController>();
        var switchView = FindObjectOfType<SwitchView>();
    

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
        }

        // Initialize the ConnectionState
        connectionState = new ConnectionState("213.65.204.181");

        // Inject the connection state into the controllers
        connectionLabel.SetConnectionState(connectionState); // Given UI the model 
        connectionController.SetConnectionState(connectionState); // Given controller the model
        
        // Initialize the Camera Model 
        cameraStateModel = new CameraStateModel();

        // Inject the camera state model into the controllers
        cameraStreamController.SetCameraStateModel(cameraStateModel); // Given controller the model
        switchView.SetCameraStateModel(cameraStateModel); // Given UI the model
    }
}
