using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;

public class SpeedPublisher : MonoBehaviour
{
    private ROSConnection ros; // Reference to ROS connection
    [SerializeField] private string topicName = "/robot/speed";
    private Float32Msg speedMessage = new Float32Msg(); // ROS message type
    private RobotModel robotModel;

    public void SetRobotModel(RobotModel model)
    {
        robotModel = model;

        // Subscribe to the speed change event
        robotModel.OnSpeedChanged += OnSpeedChanged;
    }

    public void SetConnectionController(ConnectionController controller)
    {
        ros = controller.GetROSConnection();
        RegisterPublisher();
    }

    private void RegisterPublisher()
    {
        ros.RegisterPublisher<Float32Msg>(topicName);
    }

    private void OnSpeedChanged(float newSpeed)
    {
        if (ros == null)
        {
            Debug.LogWarning("ROSConnection not initialized yet.");
            return;
        }

        PublishSpeed(newSpeed);
    }

    private void PublishSpeed(float newSpeed)
    {
        speedMessage.data = newSpeed;
        ros.Publish(topicName, speedMessage);
        //Debug.Log($"Published Speed: {speedMessage.data}");
    }
}
