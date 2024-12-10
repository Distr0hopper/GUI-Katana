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
    [SerializeField] private string robotIP = "130.239.221.73";
    private ROSConnection rosConnection { get; set; }

    private float connectionCheckDelay = 1.0f; 
    private float pendingTimeout = 3.0f;
    private Coroutine pendingCoroutine; 
    

    void Start()
    {
       if (connectionState == null)
        {
            connectionState = new ConnectionState(robotIP);
        }

        rosConnection = ROSConnection.GetOrCreateInstance();
        rosConnection.RosIPAddress = robotIP;
        rosConnection.Connect();
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
