using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ConnectionLabel : MonoBehaviour
{
    [SerializeField] public UIDocument UIDocument;

    #region UIElements 
    private Label connectionLabel;
    private VisualElement connectionStatusInner;
    private VisualElement connectionStatusOuter;

    #endregion

    #region Private Properties
    // Convert colors from 0-255 to 0-1

    // rgba(255, 63, 52,1.0) [reg orange]
    private Color outerRed = new Color(255f/255f, 63f/255f, 52f/255f, 1f);

    //rgba(5, 196, 107,1.0) [green teal]
    private Color outerGreen = new Color(5f/255f, 196f/255f, 107f/255f, 1f);
    
    // rgba(255, 94, 87,1.0) [sunset orange]
    private Color innerRed = new Color(255f/255f, 94f/255f, 87f/255f, 1f);
    
    //rgba(11, 232, 129,1.0) [minty green]
    private Color innerGreen = new Color(11f/255f, 232f/255f, 129f/255f, 1f);
    #endregion

    private ConnectionState connectionState;

    void Awake(){
        // Get the root of the UI
        var root = UIDocument.rootVisualElement;

        // Get the elements for the connection status
        connectionLabel = root.Q<Label>("ConnectionLabel");
        connectionStatusInner = root.Q<VisualElement>("ConnectionInner");
        connectionStatusOuter = root.Q<VisualElement>("ConnectionOuter");
    }

    public void SetConnectionState(ConnectionState connectionState)
    {
        this.connectionState = connectionState;
        connectionState.OnStateChanged += UpdateConnectionStatus;
        UpdateConnectionStatus(); // Initialize UI with the current state
    }

    private void UpdateConnectionStatus()
    {
        if (connectionState.isConnected)
        {
            connectionLabel.text = "Connected";
            connectionStatusInner.style.backgroundColor = innerGreen;
            connectionStatusOuter.style.backgroundColor = outerGreen;
        }
        else
        {
            connectionLabel.text = "Disconnected";
            connectionStatusInner.style.backgroundColor = innerRed;
            connectionStatusOuter.style.backgroundColor = outerRed;
        }
    }

}
