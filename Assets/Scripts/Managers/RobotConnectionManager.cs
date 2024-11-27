using System.Collections;
using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector;
using Unity.VisualScripting;
using UnityEngine;
using RosMessageTypes.Sensor;
using UnityEngine.UIElements;

public class RobotConnectionManager : MonoBehaviour
{
    private ConnectionState connectionState { get; set; }
    [SerializeField] private string katana_ip = "213.65.204.181";
    private ROSConnection rosConnection { get; set; }

    private float connectionCheckDelay = 1.0f; 
    private float lastConnectionCheckTime;
    

    void Start()
    {
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
        }
    }

    private void CheckConnection()
    {
        // Logic to check the actual connection status
        bool isConnected = !rosConnection.HasConnectionError;
        string errorMessage = isConnected ? string.Empty : "Connection error detected";

        // Update the ConnectionState
        connectionState.UpdateConnectionStatus(isConnected, errorMessage);
    }

    public void SetConnectionState(ConnectionState connectionState)
    {
        this.connectionState = connectionState;
    }

}
