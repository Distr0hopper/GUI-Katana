using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralManager : MonoBehaviour
{
    private ConnectionState connectionState;

    void Start()
    {
        // Find the controllers
        var connectionLabel = FindObjectOfType<ConnectionLabel>();
        var connectionController = FindObjectOfType<RobotConnectionManager>();

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

        // Initialize the ConnectionState
        connectionState = new ConnectionState("213.65.204.181");

        // Inject the dependency
        connectionLabel.SetConnectionState(connectionState);
        connectionController.SetConnectionState(connectionState);
    }
}
