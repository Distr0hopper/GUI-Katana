[System.Serializable] // Ensure that class is not MonoBehaviour or ScriptableObject (just Plain Old C# Object)
public class ConnectionState
{
    public bool isConnected { get; set; }
    public string RobotIP { get; set; }
    public string LastError { get; set; }

    public ConnectionState(string robotIP)
    {
        isConnected = false;
        RobotIP = robotIP;
        LastError = string.Empty;
    }

    public void UpdateConnectionStatus(bool isConnected, string error = "")
    {
        this.isConnected = isConnected;
        LastError = error;
    }
}
