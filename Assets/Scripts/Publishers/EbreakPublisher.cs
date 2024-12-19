using System.Collections;
using System.Collections.Generic;
using RosMessageTypes.Std;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;

public class EbreakPublisher : MonoBehaviour
{

    private ROSConnection ros;
    [SerializeField] private string topicName = "/ebreak";
    private BoolMsg ebreakMessage = new BoolMsg();

    private RobotModel robotModel;
    public void SetRobotModel(RobotModel model)
    {
        robotModel = model;

        // Subscribe to the ebreak change event
        robotModel.OnEmergencyStopChanged += OnEmergencyStopChanged;
    }

    public void SetConnectionController(ConnectionController controller)
    {
        ros = controller.GetROSConnection();
        RegisterPublisher();
    }
    
    private void RegisterPublisher()
    {
        if (ros != null)
        {
            ros.RegisterPublisher<BoolMsg>(topicName);
            Debug.Log($"EbreakPublisher: Registered publisher on topic {topicName}");
        }
        else
        {
            Debug.LogError("EbreakPublisher: ROSConnection is null. Cannot register publisher.");
        }
    }

        private void OnEmergencyStopChanged(bool isActive)
    {
        if (ros == null)
        {
            Debug.LogError("EbreakPublisher: ROSConnection is null. Cannot publish ebrake state.");
            return;
        }

        ebreakMessage.data = isActive;
        ros.Publish(topicName, ebreakMessage);

        Debug.Log($"EbreakPublisher: Published ebrake state: {isActive}");
    }

    void OnDestroy()
    {
        if (robotModel != null)
        {
            robotModel.OnEmergencyStopChanged -= OnEmergencyStopChanged;
        }
    }

}
