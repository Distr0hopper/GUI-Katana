using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;

public class IncomingMessageController : MonoBehaviour
{


    private ROSConnection ROSConnection;
    [SerializeField] private string velocityTopic = "/car/velocity";
    [SerializeField] private string cameraTopic = "/tag_visualization";
    [SerializeField] private string stateTopic = "/robot_mode";
    [SerializeField] private string wheelVelocityTopic = "/wheel_velocities";
    [SerializeField] private string maxVelocityTopic = "/max_vel";
    [SerializeField] private string maxSteeringTopic = "/max_steering";
    private VelocityController VelocityController;
    private CameraStreamController CameraStreamController;
    private ModeController ModeController;
    public void SetConnectionController(ConnectionController controller)
    {
        ROSConnection = controller.GetROSConnection();
    }

    public void SetVelocityController(VelocityController controller)
    {
        VelocityController = controller;
    }

    public void SetCameraStreamController(CameraStreamController controller)
    {
        CameraStreamController = controller;
    }

    public void SetStateController(ModeController controller)
    {
        ModeController = controller;
    }


    public void InitilaizeSubscribers()
    {
        // Get Velocity Data
        ROSConnection.Subscribe<Float32Msg>(velocityTopic, msg => {
            VelocityController.OnVelocityReceived(msg);
        });

        // Get Camera Data
        ROSConnection.Subscribe<ImageMsg>(cameraTopic, msg => {
            CameraStreamController.RenderImageStream(msg);
        });

        // Get State Data
        ROSConnection.Subscribe<StringMsg>(stateTopic, msg => {
            ModeController.OnStateDataReceived(msg);
         });

         // Get wheel velocity data
        ROSConnection.Subscribe<Float32MultiArrayMsg>(wheelVelocityTopic, msg => {
            VelocityController.OnWheelVelocitiesReceived(msg);
        });

        // Get max velocity data
        ROSConnection.Subscribe<Float32Msg>(maxVelocityTopic, msg => {
            VelocityController.OnMaxVelocityReceived(msg);
        });

        // Get max steering data
        ROSConnection.Subscribe<Float32Msg>(maxSteeringTopic, msg => {
            VelocityController.OnMaxSteeringReceived(msg);
        });
    }
}
