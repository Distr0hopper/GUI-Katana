using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ConnectionLabel : MonoBehaviour
{
    [SerializeField] public UIDocument UIDocument;

    #region UIElements 
    private VisualElement indicatorGreen;
    private VisualElement indicatorYellow;
    private VisualElement indicatorRed;
    private Label ipAddressLabel;

    #endregion

    #region Private Properties
    // Convert colors from 0-255 to 0-1

    // rgba(255, 63, 52,1.0) [reg orange]
    private Color passiveRed = new Color(255f/255f, 63f/255f, 52f/255f, 0.3f);
    private Color activeRed = new Color(255f/255f, 63f/255f, 52f/255f, 1f);
    
    //rgba(11, 232, 129,1.0) [minty green]
    private Color passiveGreen = new Color(11f/255f, 232f/255f, 129f/255f, 0.3f);
    private Color activeGreen = new Color(11f/255f, 232f/255f, 129f/255f, 1f);

    // rgba(255, 221, 89,1.0) [yriel yellow]
    private Color passiveYellow = new Color(255f/255f, 221f/255f, 89f/255f, 0.3f);
    private Color activeYellow = new Color(255f/255f, 221f/255f, 89f/255f, 1f);
    #endregion

    private ConnectionState connectionState;

    void Awake(){
        // Get the root of the UI
        var root = UIDocument.rootVisualElement;

        // Get the elements for the connection status
        indicatorGreen = root.Q<VisualElement>("indicatorGreen");
        indicatorYellow = root.Q<VisualElement>("indicatorYellow");
        indicatorRed = root.Q<VisualElement>("indicatorRed");
        ipAddressLabel = root.Q<Label>("IPAddr");
    }

    void Start()
    {
        // Set the IP address label
        ipAddressLabel.text = connectionState.RobotIP;
    }

    public void SetConnectionState(ConnectionState connectionState)
    {
        this.connectionState = connectionState;
        connectionState.OnStateChanged += UpdateConnectionStatus;
        UpdateConnectionStatus(); // Initialize UI with the current state
    }

    private void UpdateConnectionStatus()
    {
        switch (connectionState.Status)
        {
            case ConnectionState.ConnectionStatus.Connected:
                indicatorGreen.style.backgroundColor = activeGreen;
                indicatorYellow.style.backgroundColor = passiveYellow;
                indicatorRed.style.backgroundColor = passiveRed;
                break;
            case ConnectionState.ConnectionStatus.Disconnected:
                indicatorGreen.style.backgroundColor = passiveGreen;
                indicatorYellow.style.backgroundColor = passiveYellow;
                indicatorRed.style.backgroundColor = activeRed;
                break;
            case ConnectionState.ConnectionStatus.Pending:
                indicatorGreen.style.backgroundColor = passiveGreen;
                indicatorYellow.style.backgroundColor = activeYellow;
                indicatorRed.style.backgroundColor = passiveRed;
                break;
        }
    }
}
