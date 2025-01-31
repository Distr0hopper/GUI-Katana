using System.Collections;
using System.Collections.Generic;
using RosMessageTypes.Std;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;

public class OutgoingMessageController : MonoBehaviour
{

    private ROSConnection ros;
    [SerializeField] private string breakTopic = "/ebreak";
    [SerializeField] private string maxVelocityTopic = "/max_vel";
    [SerializeField] private string maxSteeringTopic = "/max_steering";
    [SerializeField] private string wallDistanceTopic = "/wall_distance";
    private Float32Msg maxVelMessage = new Float32Msg();
    private Float32Msg maxSteeringMessage = new Float32Msg();
    private BoolMsg ebreakMessage = new BoolMsg();

    private RobotModel robotModel;
    private bool publishersRegistered = false;


    public void SetRobotModel(RobotModel model)
    {
        robotModel = model;

        // Subscribe to the ebreak change event
        robotModel.OnEmergencyStopChanged += OnEmergencyStopChanged;
        robotModel.OnMaxVelocityChanged += OnMaxVelocityChanged;
        robotModel.OnMaxSteeringChanged += OnMaxSteeringChanged;
        robotModel.OnWallDistanceChanged += OnWallDistanceChanged;
    }

    public void SetConnectionController(ConnectionController controller)
    {
        ros = controller.GetROSConnection();
    }
    
    public void RegisterPublisher()
    {
        if (publishersRegistered){
            Debug.LogWarning("Publishers already registered. Skipping registration.");
            return;
        }

        if (ros != null)
        {
            ros.RegisterPublisher<BoolMsg>(breakTopic);
            ros.RegisterPublisher<Float32Msg>(maxVelocityTopic);
            ros.RegisterPublisher<Float32Msg>(maxSteeringTopic);
            ros.RegisterPublisher<Float32Msg>(wallDistanceTopic);

            publishersRegistered = true;
        }
        else
        {
            Debug.LogError("EbreakPublisher: ROSConnection is null. Cannot register publisher.");
        }
    }


    private void OnMaxVelocityChanged()
    {
        try
        {
            maxVelMessage.data = robotModel.maxVelocity;
            ros.Publish(breakTopic, maxVelMessage);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error publishing velocity: {e.Message}");
        }
    }

    private void OnWallDistanceChanged()
    {
        try
        {
            maxVelMessage.data = robotModel.wallDistance;
            ros.Publish(wallDistanceTopic, maxVelMessage);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error publishing wall distance: {e.Message}");
        }
    }

    private void OnMaxSteeringChanged()
    {
        try
        {
            maxSteeringMessage.data = robotModel.maxSteering;
            //ros.Publish(maxSteeringTopic, maxSteeringMessage);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error publishing steering: {e.Message}");
        }
    }

    private void OnEmergencyStopChanged(bool isActive)
    {
        try
        {
            ebreakMessage.data = isActive;
            ros.Publish(breakTopic, ebreakMessage);
            Debug.Log($"Published Ebreak: {ebreakMessage.data}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error publishing ebreak: {e.Message}");
        }
    }

    void OnDestroy()
    {
        if (robotModel != null)
        {
            robotModel.OnEmergencyStopChanged -= OnEmergencyStopChanged;
        }
    }

}
