using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using System.Text.RegularExpressions;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] UIDocument UIDocument;
    #region UIElements
    private VisualElement settingsPanel;
    private Button closeSettingsButton;
    private TextField ipInput;
    private Label invalidIpLabel;
    private Button setIpButton;
    private Button settingsButton; // This is in the Main panel on the Top

    private VisualElement invalidIp; // Overlay for graying out the set button

    #endregion
  
    private ConnectionController connectionController;

    public void SetConnectionController(ConnectionController connectionController)
    {
        this.connectionController = connectionController;
        ipInput.value = connectionController.GetROSConnection().RosIPAddress;
    }

    void Awake(){
        var root = UIDocument.rootVisualElement;
        settingsPanel = root.Q<VisualElement>("SettingsPanel");
        closeSettingsButton = root.Q<Button>("CloseSettingsButton");
        ipInput = root.Q<TextField>("IPAddrInput");
        invalidIpLabel = root.Q<Label>("InvalidIPLabel");
        setIpButton = root.Q<Button>("SetIpButton");
        invalidIp = root.Q<VisualElement>("IPInvalid");
        settingsButton = root.Q<Button>("SettingsButton");

        if (settingsPanel == null) Debug.LogError("SettingsPanel not found in the UI.");
        if (closeSettingsButton == null) Debug.LogError("CloseSettingsButton not found in the UI.");
        if (ipInput == null) Debug.LogError("IPAddrInput not found in the UI.");
        if (invalidIpLabel == null) Debug.LogError("InvalidIpLabel not found in the UI.");
        if (setIpButton == null) Debug.LogError("SetIpButton not found in the UI.");
        if (invalidIp == null) Debug.LogError("InvalidIp not found in the UI.");
        if (settingsButton == null) Debug.LogError("SettingsButton not found in the UI.");

        settingsPanel.style.display = DisplayStyle.Flex; // Show the settings panel
        invalidIpLabel.style.display = DisplayStyle.None; // Hide the invalid IP label

        // Event handlers
        closeSettingsButton.clicked += OnCloseSettingsButtonClicked;
        setIpButton.clicked += OnSetIpButtonClicked;
        settingsButton.clicked += OnSettingsButtonClicked;
        ipInput.RegisterCallback<ChangeEvent<string>>(OnIpInputChanged);

        // Initial state 
        UpdateSetButtonState(true);
        invalidIp.style.display = DisplayStyle.None;
    }

    private void OnCloseSettingsButtonClicked(){
        settingsPanel.AddToClassList("startscreen-up");
    }

    private void OnSettingsButtonClicked(){
        settingsPanel.RemoveFromClassList("startscreen-up");
    }

    private void OnSetIpButtonClicked()
    {
        string ip = ipInput.text;
        Debug.Log($"IP Address set to: {ip}");
        // Send event that IP address changed
        // If current IP address is different from the one in the UI, update
        if (connectionController.GetROSConnection().RosIPAddress != ip){
            connectionController.UpdateRobotIP(ip);
        }
        settingsPanel.AddToClassList("startscreen-up");
    }

    private void OnIpInputChanged(ChangeEvent<string> evt)
    {
        string ip = evt.newValue;
        if (IsValidIpAddress(ip))
        {
            invalidIpLabel.style.display = DisplayStyle.None; // Hide error
            UpdateSetButtonState(true); // Enable the set button
        }
        else
        {
            invalidIpLabel.style.display = DisplayStyle.Flex; // Show error
            UpdateSetButtonState(false); // Disable the set button
        }
    }


    private bool IsValidIpAddress(string ip)
    {
        // Simple regex for validating IPv4 addresses
        string ipPattern =
            @"^([0-9]{1,3}\.){3}[0-9]{1,3}$";
        if (!Regex.IsMatch(ip, ipPattern))
            return false;

        // Check if each octet is between 0 and 255
        string[] octets = ip.Split('.');
        foreach (string octet in octets)
        {
            if (!int.TryParse(octet, out int value) || value < 0 || value > 255)
                return false;
        }

        return true;
    }

    private void UpdateSetButtonState(bool isEnabled)
    {
        if (invalidIp != null)
        {
            // Use overlay to gray out the button
            invalidIp.style.display = isEnabled ? DisplayStyle.None : DisplayStyle.Flex;
        }
        else
        {
            // Directly enable/disable the button if overlay isn't used
            setIpButton.SetEnabled(isEnabled);
        }
    }
}
