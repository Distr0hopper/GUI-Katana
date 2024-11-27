using System.Collections;
using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector;
using Unity.VisualScripting;
using UnityEngine;
using RosMessageTypes.Sensor;

public class RobotConnectionManager : MonoBehaviour
{
    private ConnectionState connectionState;
    [SerializeField] private string katana_ip = "213.65.204.181";
    private ROSConnection rosConnection { get; set; }

    private float connectionCheckDelay = 1.0f; 
    private float lastConnectionCheckTime;

    // Define method for connection status changed event
    public delegate void ConnectionStatusChangedHandler(bool isConnected);
    
    // Event for connection status changed
    public event ConnectionStatusChangedHandler OnConnectionStatusChanged;
    

    void Start()
    {
        // Initialize ConnectionState model
        connectionState = new ConnectionState(katana_ip);

        // Initialize ROS connection
        rosConnection = ROSConnection.GetOrCreateInstance();
        rosConnection.RosIPAddress = katana_ip;
        rosConnection.Connect();
    }
        
    void Update()
    {
        // Check connection status at intervals defined by connectionCheckDelay
        if (Time.time - lastConnectionCheckTime > connectionCheckDelay)
        {
            lastConnectionCheckTime = Time.time;
            CheckConnection();
            Debug.Log("Connection Status: " + GetConnectionState().isConnected);
        }
    }

    private void CheckConnection()
    {
        // Logic to check the actual connection status
        bool currentConnectionStatus = !rosConnection.HasConnectionError;
        if (connectionState.isConnected != currentConnectionStatus)
        {
            string errorMessage = currentConnectionStatus ? string.Empty : "Connection error detected";
            connectionState.UpdateConnectionStatus(currentConnectionStatus, errorMessage);
            
            OnConnectionStatusChanged?.Invoke(currentConnectionStatus);
        }
    }

    // Public Accessor for ConnectionState
    public ConnectionState GetConnectionState()
    {
        return connectionState;
    }

}
