using System.Collections;
using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector;
using Unity.VisualScripting;
using UnityEngine;
using RosMessageTypes.Sensor;
using UnityEngine.UIElements;

public class ConnectionController : MonoBehaviour
{
    private ConnectionState connectionState { get; set; }
    private ROSConnection rosConnection { get; set; }

    private float connectionCheckDelay = 1.0f; 
    private float pendingTimeout = 3.0f;
    private Coroutine pendingCoroutine; 
    
    public ROSConnection GetROSConnection()
    {
        return rosConnection;
    }

    public void Initialize(ConnectionState state)
    {
        connectionState = state;
        rosConnection = ROSConnection.GetOrCreateInstance();
        rosConnection.RosIPAddress = connectionState.RobotIP;
        rosConnection.Connect();

        Debug.Log("ConnectionController initialized.");
    }

    public void UpdateRobotIP(string newIP)
    {
        if (connectionState == null)
        {
            Debug.LogError("ConnectionState is not initialized. Cannot update RobotIP.");
            return;
        }

        // Update the RobotIP in the existing connection state
        connectionState.RobotIP = newIP;

        // Update the ROSConnection with the new IP
        if (rosConnection == null)
        {
            rosConnection = ROSConnection.GetOrCreateInstance();
        }

        rosConnection.RosIPAddress = newIP;
        rosConnection.Connect();

        Debug.Log($"ConnectionController: Robot IP updated to {newIP}");

        // Trigger the ConnectionLabel to update via the OnStateChanged event
        connectionState.NotifyStateChanged();
    }

        
    void Update()
    {
        // Check connection status at intervals defined by connectionCheckDelay
        if (Time.time % connectionCheckDelay < Time.deltaTime) // Poll connection status periodically
        {
            CheckConnection();
        }
    }

    private void CheckConnection()
    {
         bool isConnected = !rosConnection.HasConnectionError;
        if (isConnected)
        {
            if (connectionState.Status != ConnectionState.ConnectionStatus.Connected)
            {
                connectionState.UpdateConnectionStatus(ConnectionState.ConnectionStatus.Connected);
                StopPendingCoroutine(); // Ensure no pending transition
            }
        }
        else
        {
            if (connectionState.Status == ConnectionState.ConnectionStatus.Connected)
            {
                // Move to pending state
                connectionState.UpdateConnectionStatus(ConnectionState.ConnectionStatus.Pending);
                StartPendingCoroutine(); // Start countdown to "disconnected"
            }
        }
    }

    private void StartPendingCoroutine()
    {
        StopPendingCoroutine();
        pendingCoroutine = StartCoroutine(PendingTimeoutCoroutine());
    }

    private void StopPendingCoroutine()
    {
        if (pendingCoroutine != null)
        {
            StopCoroutine(pendingCoroutine);
            pendingCoroutine = null;
        }
    }

    private IEnumerator PendingTimeoutCoroutine()
    {
        yield return new WaitForSeconds(pendingTimeout);

        if (connectionState.Status == ConnectionState.ConnectionStatus.Pending)
        {
            connectionState.UpdateConnectionStatus(ConnectionState.ConnectionStatus.Disconnected, "Connection timed out.");
        }
    }

    public void SetConnectionState(ConnectionState connectionState)
    {
        this.connectionState = connectionState;
    }

}
