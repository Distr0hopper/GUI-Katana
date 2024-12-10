using System.Diagnostics;
using UnityEditor.PackageManager;
using UnityEngine;

[System.Serializable] // Ensure that class is not MonoBehaviour or ScriptableObject (just Plain Old C# Object)
public class ConnectionState
{
    public bool isConnected { get; set; }
    public string RobotIP { get; set; }
    public string LastError { get; set; }

    // Event to notify listeners when the connection state changes
    public delegate void ConnectionStateChangedHandler();
    public event ConnectionStateChangedHandler OnStateChanged;

    public enum ConnectionStatus{
        Connected,
        Pending,
        Disconnected
    }

    public ConnectionStatus Status { get; set; }

    public ConnectionState(string robotIP)
    {
        Status = ConnectionStatus.Disconnected;
        RobotIP = robotIP;
        LastError = string.Empty;
    }

    public void UpdateConnectionStatus(ConnectionStatus status, string error = "")
{
    if (Status != status || LastError != error)
        {
            Status = status;
            LastError = error;

            UnityEngine.Debug.Log($"Connection state updated: {Status}");
            OnStateChanged?.Invoke();
        }
}

}
